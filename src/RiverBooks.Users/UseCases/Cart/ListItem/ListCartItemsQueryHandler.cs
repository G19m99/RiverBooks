using Ardalis.Result;
using MediatR;
using RiverBooks.Users.CartItemsEndpoints;

namespace RiverBooks.Users.UseCases.Cart.ListItem;

internal class ListCartItemsQueryHandler(IApplicationUserRepository userRepository) : IRequestHandler<ListCartItemsQuery, Result<List<CartItemDto>>>
{
    private readonly IApplicationUserRepository _userRepository = userRepository;

    public async Task<Result<List<CartItemDto>>> Handle(ListCartItemsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserWithCartByEmailAsync(request.Email);

        if (user == null)
        {
            return Result.Unauthorized();
        }

        return user.CartItems
            .Select(item => new CartItemDto(item.Id, item.BookId, item.Description, item.Quantity, item.UnitPrice))
            .ToList();
    }
}