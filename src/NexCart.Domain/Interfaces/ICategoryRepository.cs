using NexCart.Domain.Entities;

namespace NexCart.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Category>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Category> AddAsync(Category category, CancellationToken ct = default);
    void Update(Category category);
    void Delete(Category category);
}
