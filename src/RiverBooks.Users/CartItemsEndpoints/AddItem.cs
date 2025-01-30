using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.Cart.AddItem;
using System.Security.Claims;

namespace RiverBooks.Users.CartItemsEndpoints;

internal class AddItem(IMediator mediator) : Endpoint<AddCartItemRequest>
{
    private readonly IMediator _mediator = mediator;
    public override void Configure()
    {
        Post("/cart");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(AddCartItemRequest req, CancellationToken ct)
    {
        string? email = User.FindFirstValue("EmailAddress");

        AddItemToCartCommand command = new (req.BookId, req.Quantity, email!);

        Result result = await _mediator.Send(command, ct);

        if(result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
        }
        else
        {
            await SendOkAsync(ct);
        }
    }
}
