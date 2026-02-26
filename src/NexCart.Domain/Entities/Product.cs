using NexCart.Domain.Common;
using NexCart.Domain.Events;
using NexCart.Domain.ValueObjects;

namespace NexCart.Domain.Entities;

public class Product : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public Money Price { get; private set; } = Money.Zero();
    public Money? CompareAtPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public int LowStockThreshold { get; private set; } = 10;
    public bool IsActive { get; private set; } = true;
    public bool IsFeatured { get; private set; }
    public string? ImageUrl { get; private set; }
    public List<string> ImageGallery { get; private set; } = [];
    public string? Brand { get; private set; }
    public double AverageRating { get; private set; }
    public int ReviewCount { get; private set; }

    // Navigation
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public ICollection<Review> Reviews { get; private set; } = [];
    public ICollection<OrderItem> OrderItems { get; private set; } = [];

    private Product() { } // EF Core

    public static Product Create(
        string name, string description, string sku,
        decimal price, int stockQuantity, Guid categoryId,
        string? imageUrl = null, string? brand = null, bool isFeatured = false)
    {
        var product = new Product
        {
            Name = name,
            Description = description,
            Slug = GenerateSlug(name),
            Sku = sku.ToUpperInvariant(),
            Price = new Money(price),
            StockQuantity = stockQuantity,
            CategoryId = categoryId,
            ImageUrl = imageUrl,
            Brand = brand,
            IsFeatured = isFeatured
        };

        product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.Name));
        return product;
    }

    public void Update(string name, string description, decimal price, int stockQuantity,
        Guid categoryId, string? imageUrl, string? brand, bool isFeatured)
    {
        Name = name;
        Description = description;
        Slug = GenerateSlug(name);
        Price = new Money(price);
        StockQuantity = stockQuantity;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
        Brand = brand;
        IsFeatured = isFeatured;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCompareAtPrice(decimal amount) => CompareAtPrice = new Money(amount);

    public void AddToGallery(string imageUrl) => ImageGallery.Add(imageUrl);

    public bool IsInStock => StockQuantity > 0;

    public bool IsLowStock => StockQuantity <= LowStockThreshold && StockQuantity > 0;

    public decimal DiscountPercentage =>
        CompareAtPrice is not null && CompareAtPrice.Amount > 0
            ? Math.Round((1 - Price.Amount / CompareAtPrice.Amount) * 100, 0)
            : 0;

    public void ReduceStock(int quantity)
    {
        if (quantity > StockQuantity)
            throw new InvalidOperationException($"Insufficient stock for product '{Name}'. Available: {StockQuantity}, Requested: {quantity}");

        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RestoreStock(int quantity)
    {
        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRating(double newRating)
    {
        AverageRating = ((AverageRating * ReviewCount) + newRating) / (ReviewCount + 1);
        ReviewCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string GenerateSlug(string name) =>
        name.ToLowerInvariant().Replace(" ", "-").Replace("&", "and");
}
