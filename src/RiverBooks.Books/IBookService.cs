namespace RiverBooks.Books;

internal interface IBookeService
{
    Task<List<BookDto>> ListBooksAsync();
    Task<BookDto> GetBookByIdAsync(Guid id);
    Task CreateBookAsync(BookDto book);
    Task DeleteBookAsync(Guid id);
    Task UpdateBookPriceAsync(Guid id, decimal price);
}