using MediatR;
using NexCart.Application.Common.Models;

namespace NexCart.Application.Orders.Commands.PlaceOrder;

public record OrderItemRequest(Guid ProductId, int Quantity);

public record PlaceOrderCommand(
    Guid CustomerId,
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country,
    string? Notes,
    List<OrderItemRequest> Items
) : IRequest<Result<OrderDto>>;

public class OrderDto
{
    public Guid Id { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string PaymentStatus { get; init; } = string.Empty;
    public decimal SubTotal { get; init; }
    public decimal ShippingCost { get; init; }
    public decimal Tax { get; init; }
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = "USD";
    public Guid CustomerId { get; init; }
    public string? CustomerName { get; init; }
    public string? Notes { get; init; }
    public List<OrderItemDto> Items { get; init; } = [];
    public DateTime CreatedAt { get; init; }
}

public class OrderItemDto
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string? ProductImageUrl { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalPrice { get; init; }
}
