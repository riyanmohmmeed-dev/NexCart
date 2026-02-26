using Microsoft.EntityFrameworkCore;
using NexCart.Domain.Entities;
using NexCart.Domain.Enums;
using NexCart.Domain.Interfaces;

namespace NexCart.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly NexCartDbContext _context;

    public OrderRepository(NexCartDbContext context) => _context = context;

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Orders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default) =>
        await _context.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, ct);

    public async Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, Guid? customerId = null,
        string? status = null, CancellationToken ct = default)
    {
        var query = _context.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .AsQueryable();

        if (customerId.HasValue) query = query.Where(o => o.CustomerId == customerId.Value);
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            query = query.Where(o => o.Status == orderStatus);

        query = query.OrderByDescending(o => o.CreatedAt);

        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        await _context.Orders.AddAsync(order, ct);
        return order;
    }

    public void Update(Order order) => _context.Orders.Update(order);
}
