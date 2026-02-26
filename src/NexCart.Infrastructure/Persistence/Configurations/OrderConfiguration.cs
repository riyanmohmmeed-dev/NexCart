using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Entities;

namespace NexCart.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderNumber).HasMaxLength(30).IsRequired();
        builder.Property(o => o.Notes).HasMaxLength(500);

        builder.OwnsOne(o => o.SubTotal, m =>
        {
            m.Property(x => x.Amount).HasColumnName("SubTotal").HasColumnType("decimal(18,2)");
            m.Property(x => x.Currency).HasColumnName("SubTotalCurrency").HasMaxLength(3).HasDefaultValue("USD");
        });

        builder.OwnsOne(o => o.ShippingCost, m =>
        {
            m.Property(x => x.Amount).HasColumnName("ShippingCost").HasColumnType("decimal(18,2)");
            m.Property(x => x.Currency).HasColumnName("ShippingCostCurrency").HasMaxLength(3).HasDefaultValue("USD");
        });

        builder.OwnsOne(o => o.Tax, m =>
        {
            m.Property(x => x.Amount).HasColumnName("Tax").HasColumnType("decimal(18,2)");
            m.Property(x => x.Currency).HasColumnName("TaxCurrency").HasMaxLength(3).HasDefaultValue("USD");
        });

        builder.OwnsOne(o => o.TotalAmount, m =>
        {
            m.Property(x => x.Amount).HasColumnName("TotalAmount").HasColumnType("decimal(18,2)");
            m.Property(x => x.Currency).HasColumnName("TotalAmountCurrency").HasMaxLength(3).HasDefaultValue("USD");
        });

        builder.OwnsOne(o => o.ShippingAddress, a =>
        {
            a.Property(x => x.Street).HasColumnName("ShippingStreet").HasMaxLength(200);
            a.Property(x => x.City).HasColumnName("ShippingCity").HasMaxLength(100);
            a.Property(x => x.State).HasColumnName("ShippingState").HasMaxLength(100);
            a.Property(x => x.ZipCode).HasColumnName("ShippingZipCode").HasMaxLength(20);
            a.Property(x => x.Country).HasColumnName("ShippingCountry").HasMaxLength(100);
        });

        builder.HasIndex(o => o.OrderNumber).IsUnique();
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.CustomerId);

        builder.HasOne(o => o.Customer).WithMany(c => c.Orders).HasForeignKey(o => o.CustomerId);

        // Access private _items field
        builder.HasMany(o => o.Items).WithOne(i => i.Order).HasForeignKey(i => i.OrderId);
        builder.Metadata.FindNavigation(nameof(Order.Items))!.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(o => o.DomainEvents);
    }
}
