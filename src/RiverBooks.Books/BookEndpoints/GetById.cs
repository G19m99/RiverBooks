using FastEndpoints;

namespace RiverBooks.Books.BookEndpoints;
internal class GetById(IBookeService bookService) : Endpoint<GetBookByIdRequest, BookDto>
{
    private readonly IBookeService _bookeService = bookService;

    public override void Configure()
    {
        Get("/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBookByIdRequest req, CancellationToken ct)
    {
        BookDto? book = await _bookeService.GetBookByIdAsync(req.Id);
        if (book == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await SendAsync(book, cancellation: ct);
    }
}
