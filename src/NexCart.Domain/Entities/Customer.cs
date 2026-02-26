using NexCart.Domain.Common;
using NexCart.Domain.ValueObjects;

namespace NexCart.Domain.Entities;

public class Customer : AggregateRoot
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string? Phone { get; private set; }
    public Address? ShippingAddress { get; private set; }
    public Address? BillingAddress { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? UserId { get; private set; } // Links to Identity/Auth system

    // Navigation
    public ICollection<Order> Orders { get; private set; } = [];
    public ICollection<Review> Reviews { get; private set; } = [];

    public string FullName => $"{FirstName} {LastName}";

    private Customer() { } // EF Core

    public static Customer Create(string firstName, string lastName, string email, string? userId = null)
    {
        return new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = new Email(email),
            UserId = userId
        };
    }

    public void UpdateProfile(string firstName, string lastName, string? phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetShippingAddress(Address address)
    {
        ShippingAddress = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetBillingAddress(Address address)
    {
        BillingAddress = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
