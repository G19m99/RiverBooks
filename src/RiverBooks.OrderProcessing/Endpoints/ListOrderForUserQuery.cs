using Ardalis.Result;
using MediatR;

namespace RiverBooks.OrderProcessing.Endpoints;

public record ListOrderForUserQuery(string EmailAddress) : IRequest<Result<List<OrderSummary>>>;
