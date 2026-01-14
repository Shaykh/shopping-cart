namespace Basket.Domain;

public class CartItem
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Color { get; set; }

    public decimal TotalPrice => Price * Quantity;

    public CartItem()
    {
    }

    public CartItem(string productId, string productName, decimal price, int quantity, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product ID cannot be null or empty.", nameof(productId));
        
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));
        
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
        Color = color;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
        
        Quantity = newQuantity;
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        
        Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        
        if (Quantity - amount < 0)
            throw new InvalidOperationException("Cannot decrease quantity below zero.");
        
        Quantity -= amount;
    }
}
