using NexCart.Domain.Common;

namespace NexCart.Domain.Events;

public class ProductCreatedEvent : IDomainEvent
{
    public Guid ProductId { get; }
    public string ProductName { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductCreatedEvent(Guid productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
    }
}
