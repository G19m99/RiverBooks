namespace RiverBooks.Users;

internal interface IApplicationUserRepository
{
    Task<ApplicationUser> GetUserWithCartByEmailAsync(string email);
    Task SaveChangesAsync();
}
