using NexCart.Domain.Common;

namespace NexCart.Domain.Events;

public class OrderPlacedEvent : IDomainEvent
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public decimal TotalAmount { get; }
    public Guid CustomerId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public OrderPlacedEvent(Guid orderId, string orderNumber, decimal totalAmount, Guid customerId)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        TotalAmount = totalAmount;
        CustomerId = customerId;
    }
}
