using System.Text.Json.Serialization;
using Basket.Domain.Common;
using Basket.Domain.Events;
using Basket.Domain.Exceptions;

namespace Basket.Domain;

public class ShoppingCart : IAggregateRoot
{
    [JsonInclude]
    public string UserName { get; private set; } = string.Empty;

    [JsonInclude]
    public List<CartItem> Items { get; private set; } = [];

    public ShoppingCart()
    {
    }

    public ShoppingCart(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new InvalidUserNameException();

        UserName = userName;
    }

    public decimal TotalPrice => Items.Sum(item => item.TotalPrice);

    public void AddItem(CartItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

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

    public void RemoveItem(Guid productId, string? color = null)
    {
        if (productId == Guid.Empty)
            throw new InvalidProductIdException();

        var itemToRemove = Items.FirstOrDefault(x => x.ProductId == productId && x.Color == color);

        if (itemToRemove != null)
        {
            Items.Remove(itemToRemove);
        }
    }

    public void UpdateItemQuantity(Guid productId, int quantity, string? color = null)
    {
        if (productId == Guid.Empty)
            throw new InvalidProductIdException();

        var item = Items.FirstOrDefault(x => x.ProductId == productId && x.Color == color) ?? throw new CartItemNotFoundException(productId);
        item.UpdateQuantity(quantity);
    }

    public void Clear()
    {
        Items.Clear();
    }

    public bool IsEmpty => Items.Count == 0;

    public BasketCheckoutEvent CreateCheckoutEvent(
        string firstName,
        string lastName,
        string emailAddress,
        string addressLine,
        string country,
        string state,
        string zipCode,
        string cardName,
        string cardNumber,
        string expiration,
        string cvv,
        int paymentMethod)
    {
        return new BasketCheckoutEvent(
            UserName: UserName,
            TotalPrice: TotalPrice,
            FirstName: firstName,
            LastName: lastName,
            EmailAddress: emailAddress,
            AddressLine: addressLine,
            Country: country,
            State: state,
            ZipCode: zipCode,
            CardName: cardName,
            CardNumber: cardNumber,
            Expiration: expiration,
            CVV: cvv,
            PaymentMethod: paymentMethod,
            Items: [.. Items.Select(item => new BasketCheckoutItem(
                ProductId: item.ProductId,
                ProductName: item.ProductName,
                Price: item.Price,
                Quantity: item.Quantity,
                Color: item.Color))],
            OccurredOn: DateTime.UtcNow);
    }
}
