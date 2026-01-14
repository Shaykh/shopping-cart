namespace Basket.Domain;

public class ShoppingCart
{
    public string UserName { get; set; } = string.Empty;
    public List<CartItem> Items { get; set; } = new();

    public ShoppingCart()
    {
    }

    public ShoppingCart(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("User name cannot be null or empty.", nameof(userName));
        
        UserName = userName;
    }

    public decimal TotalPrice => Items.Sum(item => item.TotalPrice);

    public void AddItem(CartItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        var existingItem = Items.FirstOrDefault(x => x.ProductId == item.ProductId && x.Color == item.Color);
        
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(item.Quantity);
        }
        else
        {
            Items.Add(item);
        }
    }

    public void RemoveItem(string productId, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product ID cannot be null or empty.", nameof(productId));

        var itemToRemove = Items.FirstOrDefault(x => x.ProductId == productId && x.Color == color);
        
        if (itemToRemove != null)
        {
            Items.Remove(itemToRemove);
        }
    }

    public void UpdateItemQuantity(string productId, int quantity, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product ID cannot be null or empty.", nameof(productId));

        var item = Items.FirstOrDefault(x => x.ProductId == productId && x.Color == color);
        
        if (item == null)
            throw new InvalidOperationException($"Item with ProductId '{productId}' not found in cart.");

        item.UpdateQuantity(quantity);
    }

    public void Clear()
    {
        Items.Clear();
    }

    public bool IsEmpty => Items.Count == 0;
}
