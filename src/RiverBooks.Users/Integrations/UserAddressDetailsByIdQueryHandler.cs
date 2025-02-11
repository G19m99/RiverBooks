using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Integrations;

internal class UserAddressDetailsByIdQueryHandler(IReadOnlyUserStreetAddressRepository addressRepo) 
    : IRequestHandler<UserAddressDetailsByIdQuery, Result<UserAddressDetails>>
{
    private readonly IReadOnlyUserStreetAddressRepository _addressRepo = addressRepo;

    public async Task<Result<UserAddressDetails>> Handle(UserAddressDetailsByIdQuery req, CancellationToken ct)
    {
        UserStreetAddress? address = await _addressRepo.GetById(req.AddressId);
        if (address == null) return Result.NotFound();

        Guid userId = Guid.Parse(address.UserId);
        UserAddressDetails details = new(
            userId,
            address.Id,
            address.StreetAddress.Street1,
            address.StreetAddress.Street2,
            address.StreetAddress.City,
            address.StreetAddress.State,
            address.StreetAddress.PostalCode,
            address.StreetAddress.Country
            );

        return details;
    }
}
