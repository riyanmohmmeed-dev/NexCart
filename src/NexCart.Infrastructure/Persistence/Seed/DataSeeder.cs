using NexCart.Domain.Entities;

namespace NexCart.Infrastructure.Persistence.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(NexCartDbContext context)
    {
        if (context.Categories.Any()) return; // Already seeded

        // Categories
        var electronics = Category.Create("Electronics", "Phones, Laptops, Gadgets & Accessories", "https://images.unsplash.com/photo-1498049794561-7780e7231661?w=400");
        var clothing = Category.Create("Clothing", "Fashion & Apparel for Men and Women", "https://images.unsplash.com/photo-1489987707025-afc232f7ea0f?w=400");
        var home = Category.Create("Home & Garden", "Furniture, Decor & Kitchen Essentials", "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=400");
        var sports = Category.Create("Sports & Fitness", "Equipment, Activewear & Outdoor Gear", "https://images.unsplash.com/photo-1461896836934-bd45ba8dbf7d?w=400");
        var books = Category.Create("Books", "Bestsellers, Fiction, Non-Fiction & Education", "https://images.unsplash.com/photo-1495446815901-a7297e633e8d?w=400");

        context.Categories.AddRange(electronics, clothing, home, sports, books);
        await context.SaveChangesAsync();

        // Products
        var products = new List<Product>
        {
            CreateProduct("AirPods Pro Max", "Premium over-ear headphones with Active Noise Cancellation, Spatial Audio, and 30-hour battery life.", "APM-001", 549.99m, 45, electronics.Id, "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400", "Apple", true, 599.99m),
            CreateProduct("MacBook Pro 16\"", "M3 Max chip, 36GB RAM, 1TB SSD. The ultimate pro laptop for developers and creators.", "MBP-016", 3499.99m, 20, electronics.Id, "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=400", "Apple", true, 3999.99m),
            CreateProduct("Samsung Galaxy S24 Ultra", "6.8\" Dynamic AMOLED, 200MP camera, S Pen built-in, 5000mAh battery.", "SGS-24U", 1299.99m, 60, electronics.Id, "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?w=400", "Samsung", true),
            CreateProduct("Sony WH-1000XM5", "Industry-leading noise cancelling wireless headphones with 30-hour battery.", "SWH-XM5", 349.99m, 80, electronics.Id, "https://images.unsplash.com/photo-1546435770-a3e426bf472b?w=400", "Sony", false, 399.99m),
            CreateProduct("Mechanical Gaming Keyboard", "RGB backlit with Cherry MX switches, aluminum frame, USB-C.", "KBD-MEC", 149.99m, 100, electronics.Id, "https://images.unsplash.com/photo-1541140532154-b024d705b90a?w=400", "Corsair", false),
            CreateProduct("Premium Denim Jacket", "Classic wash denim with vintage distressing. 100% organic cotton.", "JKT-DNM", 129.99m, 75, clothing.Id, "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=400", "Levi's", true, 179.99m),
            CreateProduct("Slim Fit Chinos", "Stretch cotton twill chinos in midnight navy. Perfect for business casual.", "CHN-SLM", 69.99m, 120, clothing.Id, "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?w=400", "Bonobos", false),
            CreateProduct("Running Sneakers Air Max", "Lightweight mesh upper, React foam cushioning, breathable design.", "SNK-AIR", 179.99m, 90, clothing.Id, "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400", "Nike", true, 219.99m),
            CreateProduct("Minimalist Desk Lamp", "LED desk lamp with 5 brightness levels, wireless charging base.", "LMP-DSK", 89.99m, 50, home.Id, "https://images.unsplash.com/photo-1507473885765-e6ed057ab6fe?w=400", "IKEA", false),
            CreateProduct("Cast Iron Dutch Oven", "6-quart enameled cast iron. Perfect for soups, stews, and baking.", "KTN-DVO", 299.99m, 30, home.Id, "https://images.unsplash.com/photo-1585837146751-a44118595586?w=400", "Le Creuset", true, 349.99m),
            CreateProduct("Yoga Mat Premium", "6mm thick, non-slip surface, alignment lines, eco-friendly TPE material.", "YGA-MAT", 49.99m, 150, sports.Id, "https://images.unsplash.com/photo-1601925260368-ae2f83cf8b7f?w=400", "Manduka", false),
            CreateProduct("Adjustable Dumbbell Set", "5-52.5 lbs adjustable dumbbells. Replace 15 sets with one compact design.", "DUM-ADJ", 399.99m, 25, sports.Id, "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=400", "Bowflex", true, 499.99m),
            CreateProduct("Atomic Habits", "James Clear's bestseller on building good habits and breaking bad ones.", "BK-ATOM", 16.99m, 200, books.Id, "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=400", "Penguin", true),
            CreateProduct("Clean Code", "Robert C. Martin's essential guide to agile software craftsmanship.", "BK-CLNC", 39.99m, 80, books.Id, "https://images.unsplash.com/photo-1532012197267-da84d127e765?w=400", "Pearson", true),
            CreateProduct("Design Patterns", "Gang of Four classic: reusable elements of object-oriented software design.", "BK-DSGN", 54.99m, 40, books.Id, "https://images.unsplash.com/photo-1497633762265-9d179a990aa6?w=400", "Addison-Wesley", false),
        };

        context.Products.AddRange(products);

        // Customers
        var customer1 = Customer.Create("Riyan", "Mohammed", "riyan@example.com");
        customer1.SetShippingAddress(new Domain.ValueObjects.Address("123 Main St", "Mumbai", "Maharashtra", "400001", "India"));
        var customer2 = Customer.Create("Sarah", "Johnson", "sarah.j@example.com");
        customer2.SetShippingAddress(new Domain.ValueObjects.Address("456 Oak Ave", "San Francisco", "CA", "94102", "USA"));

        context.Customers.AddRange(customer1, customer2);
        await context.SaveChangesAsync();
    }

    private static Product CreateProduct(string name, string desc, string sku, decimal price, int stock,
        Guid categoryId, string? image, string? brand, bool featured, decimal? compareAtPrice = null)
    {
        var product = Product.Create(name, desc, sku, price, stock, categoryId, image, brand, featured);
        product.ClearDomainEvents(); // Don't fire events during seed
        if (compareAtPrice.HasValue) product.SetCompareAtPrice(compareAtPrice.Value);
        return product;
    }
}
