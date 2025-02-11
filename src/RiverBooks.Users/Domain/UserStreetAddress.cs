using Ardalis.GuardClauses;

namespace RiverBooks.Users.Domain;

public class UserStreetAddress
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserId { get; private set; } = string.Empty;
    public Address StreetAddress { get; private set; } = default!;

    public UserStreetAddress(string userId, Address address)
    {
        UserId = Guard.Against.NullOrEmpty(userId);
        StreetAddress = Guard.Against.Null(address);
    }

    //EF
    private UserStreetAddress() { }
}
