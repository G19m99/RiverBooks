namespace RiverBooks.Users;

internal sealed class AddressAddedEvent(UserStreetAddress newAddress) : DomainEventBase
{
    public UserStreetAddress NewAddress { get; set; } = newAddress;
}
