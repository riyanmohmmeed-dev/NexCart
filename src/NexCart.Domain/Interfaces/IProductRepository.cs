using NexCart.Domain.Entities;

namespace NexCart.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Product?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null,
        Guid? categoryId = null, bool? isActive = null,
        string? sortBy = null, bool sortDescending = false,
        CancellationToken ct = default);
    Task<IReadOnlyList<Product>> GetFeaturedAsync(int count = 8, CancellationToken ct = default);
    Task<Product> AddAsync(Product product, CancellationToken ct = default);
    void Update(Product product);
    void Delete(Product product);
}
