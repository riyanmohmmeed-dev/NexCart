using Microsoft.EntityFrameworkCore;
using NexCart.Domain.Entities;
using NexCart.Domain.Interfaces;

namespace NexCart.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly NexCartDbContext _context;

    public CategoryRepository(NexCartDbContext context) => _context = context;

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<Category>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default) =>
        await _context.Categories
            .Where(c => includeInactive || c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

    public async Task<Category> AddAsync(Category category, CancellationToken ct = default)
    {
        await _context.Categories.AddAsync(category, ct);
        return category;
    }

    public void Update(Category category) => _context.Categories.Update(category);
    public void Delete(Category category) => _context.Categories.Remove(category);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly NexCartDbContext _context;

    public CustomerRepository(NexCartDbContext context) => _context = context;

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.Email.Value == email.ToLowerInvariant(), ct);

    public async Task<Customer?> GetByUserIdAsync(string userId, CancellationToken ct = default) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId, ct);

    public async Task<(IReadOnlyList<Customer> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var query = _context.Customers.Where(c => c.IsActive);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search) || c.Email.Value.Contains(search));

        var totalCount = await query.CountAsync(ct);
        var items = await query.OrderBy(c => c.LastName).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, totalCount);
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken ct = default)
    {
        await _context.Customers.AddAsync(customer, ct);
        return customer;
    }

    public void Update(Customer customer) => _context.Customers.Update(customer);
}
