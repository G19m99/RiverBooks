using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

internal class ApplicationUserRepository(UsersDbContext context) : IApplicationUserRepository
{
    private readonly UsersDbContext _context = context;
    public Task<ApplicationUser> GetUserWithCartByEmailAsync(string email)
    {
        return _context.ApplicationUsers.Include(user => user.CartItems).SingleAsync(user => user.Email == email);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}