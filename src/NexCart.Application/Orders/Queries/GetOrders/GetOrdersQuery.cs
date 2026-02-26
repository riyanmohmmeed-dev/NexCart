using MediatR;
using NexCart.Application.Common.Models;
using NexCart.Application.Orders.Commands.PlaceOrder;
using NexCart.Domain.Interfaces;

namespace NexCart.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(int Page = 1, int PageSize = 10, Guid? CustomerId = null, string? Status = null)
    : IRequest<Result<PagedResult<OrderDto>>>;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, Result<PagedResult<OrderDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrdersHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<PagedResult<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken ct)
    {
        var (orders, totalCount) = await _unitOfWork.Orders.GetPagedAsync(
            request.Page, request.PageSize, request.CustomerId, request.Status, ct);

        var dtos = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            Status = o.Status.ToString(),
            PaymentStatus = o.PaymentStatus.ToString(),
            SubTotal = o.SubTotal.Amount,
            ShippingCost = o.ShippingCost.Amount,
            Tax = o.Tax.Amount,
            TotalAmount = o.TotalAmount.Amount,
            CustomerId = o.CustomerId,
            CustomerName = o.Customer?.FullName,
            Notes = o.Notes,
            Items = o.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductImageUrl = i.ProductImageUrl,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice.Amount,
                TotalPrice = i.TotalPrice.Amount
            }).ToList(),
            CreatedAt = o.CreatedAt
        }).ToList();

        return Result<PagedResult<OrderDto>>.Success(new PagedResult<OrderDto>
        {
            Items = dtos, TotalCount = totalCount, Page = request.Page, PageSize = request.PageSize
        });
    }
}
