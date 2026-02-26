using Microsoft.EntityFrameworkCore;
using NexCart.Domain.Entities;
using NexCart.Domain.Interfaces;

namespace NexCart.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly NexCartDbContext _context;

    public ProductRepository(NexCartDbContext context) => _context = context;

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Product?> GetBySlugAsync(string slug, CancellationToken ct = default) =>
        await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Slug == slug, ct);

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null,
        Guid? categoryId = null, bool? isActive = null,
        string? sortBy = null, bool sortDescending = false,
        CancellationToken ct = default)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (isActive.HasValue) query = query.Where(p => p.IsActive == isActive.Value);
        if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId.Value);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search) || p.Brand!.Contains(search));

        query = sortBy?.ToLowerInvariant() switch
        {
            "price" => sortDescending ? query.OrderByDescending(p => p.Price.Amount) : query.OrderBy(p => p.Price.Amount),
            "name" => sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "rating" => query.OrderByDescending(p => p.AverageRating),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Product>> GetFeaturedAsync(int count = 8, CancellationToken ct = default) =>
        await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsFeatured && p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync(ct);

    public async Task<Product> AddAsync(Product product, CancellationToken ct = default)
    {
        await _context.Products.AddAsync(product, ct);
        return product;
    }

    public void Update(Product product) => _context.Products.Update(product);

    public void Delete(Product product) => _context.Products.Remove(product);
}
