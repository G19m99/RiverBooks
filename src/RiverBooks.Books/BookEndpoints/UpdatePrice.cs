using FastEndpoints;

namespace RiverBooks.Books.BookEndpoints;

internal class UpdatePrice(IBookeService bookService) : Endpoint<UpdatePriceRequest, BookDto>
{
    private readonly IBookeService _bookService = bookService;

    public override void Configure()
    {
        Post("/books/{Id}/priceHistory");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdatePriceRequest req, CancellationToken ct)
    {
        //TODO: handle book not found
        await _bookService.UpdateBookPriceAsync(req.Id, req.NewPrice);

        BookDto updatedBook = await _bookService.GetBookByIdAsync(req.Id);

        await SendAsync(updatedBook, cancellation: ct);
    }
}
