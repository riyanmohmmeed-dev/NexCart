using NexCart.Domain.Common;

namespace NexCart.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation
    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }
    public ICollection<Category> SubCategories { get; private set; } = [];
    public ICollection<Product> Products { get; private set; } = [];

    private Category() { } // EF Core

    public static Category Create(string name, string description, string? imageUrl = null, Guid? parentCategoryId = null)
    {
        var category = new Category
        {
            Name = name,
            Description = description,
            Slug = GenerateSlug(name),
            ImageUrl = imageUrl,
            ParentCategoryId = parentCategoryId
        };
        return category;
    }

    public void Update(string name, string description, string? imageUrl)
    {
        Name = name;
        Description = description;
        Slug = GenerateSlug(name);
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string GenerateSlug(string name) =>
        name.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("&", "and");
}
