namespace RiverBooks.Books;

internal class BookService : IBookeService
{
    public List<BookDto> ListBooks() => [
            new BookDto(Guid.NewGuid(), "some test", ""),
            new BookDto(Guid.NewGuid(), "some test", ""),
            new BookDto(Guid.NewGuid(), "some test", "")
        ];
}