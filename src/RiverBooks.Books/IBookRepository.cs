namespace RiverBooks.Books;

internal interface IBookRepository : IReadonlyBookRepository
{
    Task AddAsync(Book book);
    Task DeleteAsync(Book book);
    Task SaveChangesAsync();
}
