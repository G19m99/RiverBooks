using Ardalis.Result;
using MediatR;
using RiverBooks.Users.CartItemsEndpoints;

namespace RiverBooks.Users.UseCases;

public record ListCartItemsQuery(string Email) : IRequest<Result<List<CartItemDto>>>;
