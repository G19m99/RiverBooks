using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data;

internal class UserStreetAddressRepository(UsersDbContext context) : IReadOnlyUserStreetAddressRepository
{
    private readonly UsersDbContext _context = context;

    public Task<UserStreetAddress?> GetById(Guid addressId)
    {
        return _context.UserStreetAddresses.SingleOrDefaultAsync(a => a.Id == addressId);
    }
}
