

using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace RiverBooks.Users.UsersEndpoints;

public record AddAddressRequest(
    string Street1,
    string Street2,
    string City,
    string State,
    string PostalCode,
    string Country);
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
            req.Street1,
            req.Street2,
            req.City,
            req.State,
            req.PostalCode,
            req.Country
            );

        var results = await _mediator.SendAsync(command, ct);

        if (results.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await SendOkAsync(ct);
    }
}
public record CreateUserRequest(string Email, string Password);
internal class Create : Endpoint<CreateUserRequest>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public Create(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var newUser = new ApplicationUser
        {
            Email = req.Email,
            UserName = req.Email.Trim(),
        };
        var res = await _userManager.CreateAsync(newUser, req.Password);

        await SendOkAsync(newUser, ct);
    }
}
