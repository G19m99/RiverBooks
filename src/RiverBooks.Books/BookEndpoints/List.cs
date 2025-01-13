using FastEndpoints;

namespace RiverBooks.Books.BookEndpoints;
internal class List(IBookeService bookService) : EndpointWithoutRequest<ListBooksResponse>
{
    private readonly IBookeService _bookService = bookService;

    public override void Configure()
    {
        Get("/books");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CancellationToken ct = default)
    {
        List<BookDto> books = await _bookService.ListBooksAsync();
        await SendAsync(new ListBooksResponse(books));
    }
}
