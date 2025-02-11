using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

internal class UserStreetAddressRepositroy(UsersDbContext context) : IReadOnlyUserStreetAddressRepository
{
    private readonly UsersDbContext _context = context;

    public Task<UserStreetAddress?> GetById(Guid addressId)
    {
        return _context.UserStreetAddresses.SingleOrDefaultAsync(a => a.Id == addressId);
    }
}
