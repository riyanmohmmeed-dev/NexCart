namespace NexCart.Application.Products.Queries.GetProducts;

public class ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Sku { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Currency { get; init; } = "USD";
    public decimal? CompareAtPrice { get; init; }
    public decimal DiscountPercentage { get; init; }
    public int StockQuantity { get; init; }
    public bool IsInStock { get; init; }
    public bool IsActive { get; init; }
    public bool IsFeatured { get; init; }
    public string? ImageUrl { get; init; }
    public string? Brand { get; init; }
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public double AverageRating { get; init; }
    public int ReviewCount { get; init; }
    public DateTime CreatedAt { get; init; }
}
