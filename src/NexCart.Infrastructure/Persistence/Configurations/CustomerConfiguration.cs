using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexCart.Domain.Entities;

namespace NexCart.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.UserId).HasMaxLength(100);

        builder.OwnsOne(c => c.Email, e =>
        {
            e.Property(x => x.Value).HasColumnName("Email").HasMaxLength(255).IsRequired();
            e.HasIndex(x => x.Value).IsUnique();
        });

        builder.OwnsOne(c => c.ShippingAddress, a =>
        {
            a.Property(x => x.Street).HasColumnName("ShippingStreet").HasMaxLength(200);
            a.Property(x => x.City).HasColumnName("ShippingCity").HasMaxLength(100);
            a.Property(x => x.State).HasColumnName("ShippingState").HasMaxLength(100);
            a.Property(x => x.ZipCode).HasColumnName("ShippingZipCode").HasMaxLength(20);
            a.Property(x => x.Country).HasColumnName("ShippingCountry").HasMaxLength(100);
        });

        builder.OwnsOne(c => c.BillingAddress, a =>
        {
            a.Property(x => x.Street).HasColumnName("BillingStreet").HasMaxLength(200);
            a.Property(x => x.City).HasColumnName("BillingCity").HasMaxLength(100);
            a.Property(x => x.State).HasColumnName("BillingState").HasMaxLength(100);
            a.Property(x => x.ZipCode).HasColumnName("BillingZipCode").HasMaxLength(20);
            a.Property(x => x.Country).HasColumnName("BillingCountry").HasMaxLength(100);
        });

        builder.Ignore(c => c.DomainEvents);
    }
}
