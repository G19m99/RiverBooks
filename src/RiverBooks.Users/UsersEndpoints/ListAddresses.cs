using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User;
using System.Security.Claims;

namespace RiverBooks.Users.UsersEndpoints;

internal class ListAddresses(IMediator mediator) : EndpointWithoutRequest<AddressListResponse>
{
    private readonly IMediator _mediator = mediator;

    public override void Configure()
    {
        Get("/users/addresses");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var email = User.FindFirstValue("EmailAddress")!;

        var query = new ListAddressesQuery(email);
        var result = await _mediator.Send(query, ct);

        if(result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
        }

        var res = new AddressListResponse();
        res.Addresses = result.Value;

        await SendAsync(res, cancellation: ct);
    }
}
