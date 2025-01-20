
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

internal class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    
    private readonly List<CartItem> _cartItems = [];
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

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