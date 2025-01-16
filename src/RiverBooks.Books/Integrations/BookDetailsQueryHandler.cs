using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;

namespace RiverBooks.Books.Integrations;

internal class BookDetailsQueryHandler(IBookeService bookeService) : IRequestHandler<BookDetailsQuery, Result<BookDetailsResponse>>
{
    private readonly IBookeService _bookeService = bookeService;

    public async Task<Result<BookDetailsResponse>> Handle(BookDetailsQuery request, CancellationToken cancellationToken)
    {
        BookDto? book = await _bookeService.GetBookByIdAsync(request.BookId);

        if (book is null) return Result.NotFound();

        BookDetailsResponse response = new(book.Id, book.Title, book.Author, book.Price);

        return response;
    }
}