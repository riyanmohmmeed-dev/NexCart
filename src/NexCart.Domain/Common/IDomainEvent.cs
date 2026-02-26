namespace NexCart.Domain.Common;

/// <summary>
/// Marker interface for domain events. Domain layer stays dependency-free.
/// MediatR dispatch is handled in the Infrastructure layer.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
