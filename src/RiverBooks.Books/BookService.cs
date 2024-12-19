
namespace RiverBooks.Books;

internal class BookService(IBookRepository bookRepository) : IBookeService
{
    private readonly IBookRepository _bookRepository = bookRepository;

    public async Task CreateBookAsync(BookDto newBook)
    {
        Book book = new(newBook.Id, newBook.Title, newBook.Author, newBook.Price);
        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(Guid id)
    {
        var bookToDelte = await _bookRepository.GetByIdAsync(id);

        if (bookToDelte != null)
        {
            await _bookRepository.DeleteAsync(bookToDelte);
            await _bookRepository.SaveChangesAsync();
        }
    }

    public async Task<BookDto> GetBookByIdAsync(Guid id)
    {
        Book book = await _bookRepository.GetByIdAsync(id) ?? throw new Exception("Book not found");
        
        return new(book.Id, book.Title, book.Author, book.Price);
    }

    public async Task<List<BookDto>> ListBooksAsync()
    {
        var books = await _bookRepository.ListAsync();

        List<BookDto> resultBooks = books.Select(book => new BookDto(book.Id, book.Title, book.Author, book.Price)).ToList();

        return resultBooks;
    }

    public async Task UpdateBookPriceAsync(Guid id, decimal price)
    {
        Book book = await _bookRepository.GetByIdAsync(id) ?? throw new Exception("Book not found");
        book.UpodatePrice(price);
        await _bookRepository.SaveChangesAsync();
    }
}