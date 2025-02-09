using Ardalis.Result;
using MediatR;
using RiverBooks.Users.UsersEndpoints;

namespace RiverBooks.Users.UseCases.User;

internal class ListAddressesQueryHandler(IApplicationUserRepository repo) : IRequestHandler<ListAddressesQuery, Result<List<UserAddressDto>>>
{
    private readonly IApplicationUserRepository _userRepo = repo;

    public async Task<Result<List<UserAddressDto>>> Handle(ListAddressesQuery request, CancellationToken ct)
    {
        ApplicationUser user = await _userRepo.GetUserWithAddressesByEmailAsync(request.Email);

        if (user == null)
        {
            return Result.Unauthorized();
        }

        return user.Addresses.Select(a => 
            new UserAddressDto(
                a.Id,
                a.StreetAddress.Street1,
                a.StreetAddress.Street2,
                a.StreetAddress.City,
                a.StreetAddress.State,
                a.StreetAddress.PostalCode,
                a.StreetAddress.Country
            )
        ).ToList();
    }
}