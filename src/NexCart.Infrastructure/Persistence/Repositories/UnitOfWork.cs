using NexCart.Domain.Interfaces;

namespace NexCart.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly NexCartDbContext _context;

    public IProductRepository Products { get; }
    public IOrderRepository Orders { get; }
    public ICategoryRepository Categories { get; }
    public ICustomerRepository Customers { get; }

    public UnitOfWork(NexCartDbContext context)
    {
        _context = context;
        Products = new ProductRepository(context);
        Orders = new OrderRepository(context);
        Categories = new CategoryRepository(context);
        Customers = new CustomerRepository(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);
}
