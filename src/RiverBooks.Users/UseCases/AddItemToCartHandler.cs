using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;

namespace RiverBooks.Users.UseCases;

internal class AddItemToCartHandler(IApplicationUserRepository userRepository, IMediator mediator) 
    : IRequestHandler<AddItemToCartCommand, Result>
{
    private readonly IApplicationUserRepository _userRepository = userRepository;
    private readonly IMediator _mediator = mediator;

    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = await _userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);
        if (user is null)
        {
            return Result.Unauthorized();
        }

        BookDetailsQuery query = new(request.BookId);

        Result<BookDetailsResponse> result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.NotFound) return Result.NotFound();

        BookDetailsResponse bookDetails = result.Value;
        string description = $"{bookDetails.Title} by {bookDetails.Author}";
        CartItem newCartItem = new(request.BookId, description, request.Quantity, bookDetails.Price);

        user.AddItemToCart(newCartItem);

        await _userRepository.SaveChangesAsync();

        return Result.Success();
    }
}
