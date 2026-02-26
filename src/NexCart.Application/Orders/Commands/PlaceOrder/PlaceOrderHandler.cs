using MediatR;
using NexCart.Application.Common.Models;
using NexCart.Domain.Entities;
using NexCart.Domain.Interfaces;
using NexCart.Domain.ValueObjects;

namespace NexCart.Application.Orders.Commands.PlaceOrder;

public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, Result<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public PlaceOrderHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<OrderDto>> Handle(PlaceOrderCommand request, CancellationToken ct)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId, ct);
        if (customer is null)
            return Result<OrderDto>.NotFound($"Customer with ID '{request.CustomerId}' not found.");

        var shippingAddress = new Address(request.Street, request.City, request.State, request.ZipCode, request.Country);
        var order = Order.Create(request.CustomerId, shippingAddress, request.Notes);

        foreach (var item in request.Items)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId, ct);
            if (product is null)
                return Result<OrderDto>.NotFound($"Product with ID '{item.ProductId}' not found.");
            if (!product.IsInStock)
                return Result<OrderDto>.Failure($"Product '{product.Name}' is out of stock.");

            order.AddItem(product, item.Quantity);
        }

        order.PlaceOrder();
        await _unitOfWork.Orders.AddAsync(order, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var dto = new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status.ToString(),
            PaymentStatus = order.PaymentStatus.ToString(),
            SubTotal = order.SubTotal.Amount,
            ShippingCost = order.ShippingCost.Amount,
            Tax = order.Tax.Amount,
            TotalAmount = order.TotalAmount.Amount,
            CustomerId = order.CustomerId,
            CustomerName = customer.FullName,
            Notes = order.Notes,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductImageUrl = i.ProductImageUrl,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice.Amount,
                TotalPrice = i.TotalPrice.Amount
            }).ToList(),
            CreatedAt = order.CreatedAt
        };

        return Result<OrderDto>.Created(dto);
    }
}
