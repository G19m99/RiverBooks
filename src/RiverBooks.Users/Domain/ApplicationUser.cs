using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;
using RiverBooks.SharedKernel;
using RiverBooks.Users.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiverBooks.Users.Domain;

internal class ApplicationUser : IdentityUser, IHaveDomainEvents
{
    public string FullName { get; set; } = string.Empty;

    private readonly List<CartItem> _cartItems = [];
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    private readonly List<UserStreetAddress> _addresses = [];
    public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();

    private List<DomainEventBase> _domainEvents = [];
    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();
    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
    void IHaveDomainEvents.ClearDomainEvents() => _domainEvents.Clear();

    internal UserStreetAddress AddAddress(Address address)
    {
        Guard.Against.Null(address);

        var existingAddress = _addresses.FirstOrDefault(a => a.StreetAddress == address);
        if (existingAddress != null)
        {
            return existingAddress;
        }
        var newAddress = new UserStreetAddress(Id, address);
        _addresses.Add(newAddress);

        var domainEvent = new AddressAddedEvent(newAddress);
        RegisterDomainEvent(domainEvent);

        return newAddress;
    }

    public void AddItemToCart(CartItem item)
    {
        Guard.Against.Null(item);

        CartItem? existingBook = _cartItems.SingleOrDefault(cItems => cItems.BookId == item.BookId);
        if (existingBook != null)
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
