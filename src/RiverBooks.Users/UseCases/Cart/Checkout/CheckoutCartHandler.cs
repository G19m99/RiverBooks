using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts;

namespace RiverBooks.Users.UseCases.Cart.Checkout;

internal class CheckoutCartHandler(IApplicationUserRepository userRepository, IMediator mediator) : IRequestHandler<CheckoutCartCommand, Result<Guid>>
{
    private readonly IApplicationUserRepository _userRepository = userRepository;
    private readonly IMediator _mediator = mediator;

    public async Task<Result<Guid>> Handle(CheckoutCartCommand cmd, CancellationToken ct)
    {
        ApplicationUser user = await _userRepository.GetUserWithCartByEmailAsync(cmd.Email);

        if (user is null) return Result.Unauthorized();

        if (user.CartItems.Count == 0) return Result.Invalid();

        List<OrderItemDetails> items = user.CartItems.Select(oi =>
        new OrderItemDetails(
             oi.Id,
             oi.Description,
             oi.Quantity,
             oi.UnitPrice
        )).ToList();

        CreateOrderCommand createOrderCmd = new(
            Guid.Parse(user.Id),
            cmd.ShippingAddressId,
            cmd.BillingAddressId,
            items
        );

        var result = await _mediator.Send(createOrderCmd, ct);

        if (!result.IsSuccess)
        {
            //return the unsuccesful order
            return result.Map(o => o.OrderId);
        }

        user.ClearCart();
        await _userRepository.SaveChangesAsync();

        return Result.Success(result.Value.OrderId);
    }
}