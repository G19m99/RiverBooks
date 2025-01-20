using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.Checkout;
using System.Security.Claims;

namespace RiverBooks.Users.CartItemsEndpoints;

internal class Checkout(IMediator mediator) : Endpoint<CheckoutRequest, CheckoutResponse>
{
    private readonly IMediator _mediator = mediator;
    public override void Configure()
    {
        Post("cart/checkout");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(CheckoutRequest req, CancellationToken ct)
    {
        string email = User.FindFirstValue("EmailAddress")!;

        CheckoutCartCommand cmd = new(email, req.ShippingAddressId, req.BillingAddressId);

        Result<Guid> result = await _mediator.Send(cmd, ct);

        if (result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
        }
        else
        {
            await SendOkAsync(new CheckoutResponse(result.Value), ct);
        }
    }
}

internal record CheckoutResponse(Guid NewOrderId);
internal record CheckoutRequest(Guid ShippingAddressId, Guid BillingAddressId);