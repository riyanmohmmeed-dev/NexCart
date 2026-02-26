using NexCart.Domain.Entities;

namespace NexCart.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default);
    Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, Guid? customerId = null,
        string? status = null, CancellationToken ct = default);
    Task<Order> AddAsync(Order order, CancellationToken ct = default);
    void Update(Order order);
}
