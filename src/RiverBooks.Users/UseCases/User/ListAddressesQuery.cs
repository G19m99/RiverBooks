using Ardalis.Result;
using MediatR;
using RiverBooks.Users.UsersEndpoints;

namespace RiverBooks.Users.UseCases.User;

internal record ListAddressesQuery(string Email) : IRequest<Result<List<UserAddressDto>>>;
