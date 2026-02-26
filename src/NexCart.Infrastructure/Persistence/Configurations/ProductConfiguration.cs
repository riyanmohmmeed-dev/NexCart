using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Entities;

namespace NexCart.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(2000).IsRequired();
        builder.Property(p => p.Slug).HasMaxLength(220).IsRequired();
        builder.Property(p => p.Sku).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Brand).HasMaxLength(100);
        builder.Property(p => p.ImageUrl).HasMaxLength(500);

        // Value Object: Money (Price)
        builder.OwnsOne(p => p.Price, money =>
        {
            money.Property(m => m.Amount).HasColumnName("Price").HasColumnType("decimal(18,2)").IsRequired();
            money.Property(m => m.Currency).HasColumnName("PriceCurrency").HasMaxLength(3).HasDefaultValue("USD");
        });

        // Value Object: Money (CompareAtPrice)
        builder.OwnsOne(p => p.CompareAtPrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("CompareAtPrice").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("CompareAtPriceCurrency").HasMaxLength(3);
        });

        // Image gallery as JSON
        builder.Property(p => p.ImageGallery).HasConversion(
            v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
            v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        builder.HasIndex(p => p.Slug).IsUnique();
        builder.HasIndex(p => p.Sku).IsUnique();
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => p.IsFeatured);

        builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);

        // Ignore domain events (not persisted)
        builder.Ignore(p => p.DomainEvents);
    }
}
