using NexCart.Domain.Common;
using NexCart.Domain.Enums;
using NexCart.Domain.Events;
using NexCart.Domain.ValueObjects;

namespace NexCart.Domain.Entities;

public class Order : AggregateRoot
{
    public string OrderNumber { get; private set; } = string.Empty;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Pending;
    public Money SubTotal { get; private set; } = Money.Zero();
    public Money ShippingCost { get; private set; } = Money.Zero();
    public Money Tax { get; private set; } = Money.Zero();
    public Money TotalAmount { get; private set; } = Money.Zero();
    public Address ShippingAddress { get; private set; } = null!;
    public string? Notes { get; private set; }

    // Navigation
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    private readonly List<OrderItem> _items = [];
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    public static Order Create(Guid customerId, Address shippingAddress, string? notes = null)
    {
        var order = new Order
        {
            CustomerId = customerId,
            ShippingAddress = shippingAddress,
            OrderNumber = GenerateOrderNumber(),
            Notes = notes
        };
        return order;
    }

    public void AddItem(Product product, int quantity)
    {
        var existingItem = _items.Find(i => i.ProductId == product.Id);
        if (existingItem is not null)
            throw new InvalidOperationException($"Product '{product.Name}' already exists in order. Remove it first.");

        product.ReduceStock(quantity);
        _items.Add(OrderItem.Create(Id, product, quantity));
        RecalculateTotals();
    }

    public void PlaceOrder()
    {
        if (_items.Count == 0)
            throw new InvalidOperationException("Cannot place an order with no items.");

        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderPlacedEvent(Id, OrderNumber, TotalAmount.Amount, CustomerId));
    }

    public void MarkAsProcessing()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be processed.");
        Status = OrderStatus.Processing;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Processing)
            throw new InvalidOperationException("Only processing orders can be shipped.");
        Status = OrderStatus.Shipped;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Only shipped orders can be delivered.");
        Status = OrderStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new InvalidOperationException("Cannot cancel shipped or delivered orders.");
        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkPaymentCompleted()
    {
        PaymentStatus = PaymentStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    private void RecalculateTotals()
    {
        var subTotal = _items.Sum(i => i.UnitPrice.Amount * i.Quantity);
        SubTotal = new Money(subTotal);
        Tax = new Money(Math.Round(subTotal * 0.08m, 2)); // 8% tax
        ShippingCost = subTotal >= 100 ? Money.Zero() : new Money(9.99m); // Free shipping over $100
        TotalAmount = new Money(SubTotal.Amount + Tax.Amount + ShippingCost.Amount);
    }

    private static string GenerateOrderNumber() =>
        $"NXC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";
}
