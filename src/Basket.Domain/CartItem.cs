using System.Text.Json.Serialization;
using Basket.Domain.Exceptions;

namespace Basket.Domain;

public class CartItem
{
    [JsonInclude]
    public Guid ProductId { get; private set; }

    [JsonInclude]
    public string ProductName { get; private set; } = string.Empty;

    [JsonInclude]
    public decimal Price { get; private set; }

    [JsonInclude]
    public int Quantity { get; private set; }

    [JsonInclude]
    public string? Color { get; private set; }

    public decimal TotalPrice => Price * Quantity;

    public CartItem()
    {
    }

    public CartItem(Guid productId, string productName, decimal price, int quantity, string? color = null)
    {
        if (productId == Guid.Empty)
            throw new InvalidProductIdException();

        if (string.IsNullOrWhiteSpace(productName))
            throw new InvalidProductNameException();

        if (price < 0)
            throw new InvalidPriceException();

        if (quantity <= 0)
            throw new InvalidQuantityException();

        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
        Color = color;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new InvalidQuantityException();

        Quantity = newQuantity;
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new InvalidQuantityException();

        Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new InvalidQuantityException();

        if (Quantity - amount < 0)
            throw new InvalidQuantityOperationException("Cannot decrease quantity below zero.");

        Quantity -= amount;
    }
}
