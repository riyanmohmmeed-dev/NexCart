using NexCart.Domain.Common;

namespace NexCart.Domain.Entities;

public class Review : BaseEntity
{
    public int Rating { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Comment { get; private set; } = string.Empty;
    public bool IsVerifiedPurchase { get; private set; }

    // Navigation
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;

    private Review() { }

    public static Review Create(Guid productId, Guid customerId, int rating, string title, string comment, bool isVerifiedPurchase = false)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));

        return new Review
        {
            ProductId = productId,
            CustomerId = customerId,
            Rating = rating,
            Title = title,
            Comment = comment,
            IsVerifiedPurchase = isVerifiedPurchase
        };
    }
}
