using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace RiverBooks.Users.UseCases.User;

internal record AddAddressToUserCommand(
    string Email,
    string Street1,
    string Street2,
    string City,
    string State,
    string PostalCode,
    string Country) : IRequest<Result>;

internal class AddAddressToUserHandler(ILogger<AddAddressToUserHandler> logger, IApplicationUserRepository repo) : IRequestHandler<AddAddressToUserCommand, Result>
{
    private readonly ILogger<AddAddressToUserHandler> _logger = logger;
    private readonly IApplicationUserRepository _repo = repo;

    public async Task<Result> Handle(AddAddressToUserCommand req, CancellationToken ct)
    {
        var user = await _repo.GetUserWithAddressesByEmail(req.Email);

        if (user == null) return Result.Unauthorized();

        Address addressToAdd = new(
            req.Street1,
            req.Street2,
            req.City,
            req.State,
            req.PostalCode,
            req.Country
        );

        var userAddress = user.AddAddress(addressToAdd);
        await _repo.SaveChangesAsync();

        _logger.LogInformation("[UserCase] added address {address} to user {user} (Total addresses {count})",
            userAddress.StreetAddress,
            req.Email,
            user.Adderesses.Count
            );

        return Result.Success();
    }
}
