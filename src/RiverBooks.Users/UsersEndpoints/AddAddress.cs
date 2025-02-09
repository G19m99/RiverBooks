

using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User;
using System.Security.Claims;

namespace RiverBooks.Users.UsersEndpoints;

internal class AddAddress(IMediator mediator) : Endpoint<AddAddressRequest>
{
    private readonly IMediator _mediator = mediator;

    public override void Configure()
    {
        Post("/users/addresses");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(AddAddressRequest req, CancellationToken ct)
    {
        string email = User.FindFirstValue("EmailAddress")!;

        var command = new AddAddressToUserCommand(
            email,
            req.Street1,
            req.Street2,
            req.City,
            req.State,
            req.PostalCode,
            req.Country
            );

        Result results = await _mediator.Send(command, ct);

        if (results.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await SendOkAsync(ct);
    }
}
