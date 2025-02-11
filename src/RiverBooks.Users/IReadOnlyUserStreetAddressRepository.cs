namespace RiverBooks.Users;

internal interface IReadOnlyUserStreetAddressRepository
{
    Task<UserStreetAddress?> GetById(Guid addressId);
}
