using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.Cart.ListItem;
using System.Security.Claims;

namespace RiverBooks.Users.CartItemsEndpoints;

internal class ListCartItems(IMediator mediator) : EndpointWithoutRequest<CartResponse>
{
    private readonly IMediator _mediator = mediator;

    public override void Configure()
    {
        Get("/cart");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string? email = User.FindFirstValue("EmailAddress");

        ListCartItemsQuery query = new(email!);
        Result<List<CartItemDto>> results = await _mediator.Send(query, ct);
        
        if(results.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
        }
        else
        {
            CartResponse response = new(results.Value);

            await SendOkAsync(response, ct);
        }
    }
}
