using Ardalis.Result;
using MediatR;
using RiverBooks.Users.CartItemsEndpoints;

namespace RiverBooks.Users.UseCases.ListItem;

public record ListCartItemsQuery(string Email) : IRequest<Result<List<CartItemDto>>>;
