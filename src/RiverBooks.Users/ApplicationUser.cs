
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

internal class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    
    private readonly List<CartItem> _cartItems = [];
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    private readonly List<UserStreetAddress> _addresses = [];
    public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();

    internal UserStreetAddress AddAddress(Address address)
    {
        Guard.Against.Null(address);

        var existingAddress = _addresses.FirstOrDefault(a => a.StreetAddress == address);
        if(existingAddress != null)
        {
            return existingAddress;
        }
        var newAddress = new UserStreetAddress(Id, address);
        _addresses.Add(newAddress);

        return newAddress;
    }

    public void AddItemToCart(CartItem item)
    {
        Guard.Against.Null(item);

        CartItem? existingBook = _cartItems.SingleOrDefault(cItems => cItems.BookId == item.BookId);
        if(existingBook != null)
        {
            existingBook.UpdateQuantity(existingBook.Quantity + item.Quantity);
            existingBook.UpdateDescription(item.Description);
            existingBook.UpdatePrice(item.UnitPrice);
            return;
        }
        _cartItems.Add(item);
    }

    internal void ClearCart()
    {
        _cartItems.Clear();
    }
}

public record Address(
    string Street1,
    string Street2,
    string City,
    string State,
    string PostalCode,
    string Country
);

public class UserStreetAddress
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Address Address { get; set; } = default!;

    public UserStreetAddress(string userId, Address address)
    {
        UserId = Guard.Against.NullOrEmpty(userId);
        Address = Guard.Against.Null(address);
    }

    //EF
    private UserStreetAddress() {}

public class CartItem
{
    public CartItem(Guid bookId, string description, int quantity, decimal unitPrice)
    {
        BookId = Guard.Against.Default(bookId);
        Description = Guard.Against.NullOrEmpty(description);
        Quantity = Guard.Against.Negative(quantity);
        UnitPrice = Guard.Against.Negative(unitPrice);
    }

    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public void UpdateQuantity(int quantity)
    {
        Quantity = Guard.Against.Negative(quantity);
    }

    internal void UpdateDescription(string description)
    {
        Description = Guard.Against.NullOrEmpty(description);
    }

    internal void UpdatePrice(decimal unitPrice)
    {
        UnitPrice = Guard.Against.Negative(unitPrice);
    }
}