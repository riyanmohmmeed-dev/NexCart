using NexCart.Domain.Common;
using NexCart.Domain.ValueObjects;

namespace NexCart.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = null!;
    public string ProductName { get; private set; } = string.Empty;
    public string? ProductImageUrl { get; private set; }

    // Navigation
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public Money TotalPrice => UnitPrice.Multiply(Quantity);

    private OrderItem() { }

    internal static OrderItem Create(Guid orderId, Product product, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        return new OrderItem
        {
            OrderId = orderId,
            ProductId = product.Id,
            ProductName = product.Name,
            ProductImageUrl = product.ImageUrl,
            UnitPrice = product.Price,
            Quantity = quantity
        };
    }
}
