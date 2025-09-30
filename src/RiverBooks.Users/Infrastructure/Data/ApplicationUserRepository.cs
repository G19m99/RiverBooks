using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data;

internal class ApplicationUserRepository(UsersDbContext context) : IApplicationUserRepository
{
    private readonly UsersDbContext _context = context;

    public Task<ApplicationUser> GetUserWithAddressesByEmailAsync(string email)
    {
        return _context.ApplicationUsers
            .Include(user => user.Addresses)
            .SingleAsync(user => user.Email == email); 
    }

    public Task<ApplicationUser> GetUserWithCartByEmailAsync(string email)
    {
        return _context.ApplicationUsers.Include(user => user.CartItems).SingleAsync(user => user.Email == email);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
