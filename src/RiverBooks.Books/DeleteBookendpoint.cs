using FastEndpoints;

namespace RiverBooks.Books;
internal class DeleteBookendpoint(IBookeService bookeService) : Endpoint<DeleteBookRequest>
{
    private readonly IBookeService _bookeService = bookeService;
    public override void Configure()
    {
        Delete("/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteBookRequest req, CancellationToken ct)
    {
        //TODO: handle not found
        await _bookeService.DeleteBookAsync(req.Id);

        await SendNoContentAsync(ct);
    }
}
 