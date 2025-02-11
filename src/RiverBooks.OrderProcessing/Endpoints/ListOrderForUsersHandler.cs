using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Interfaces;

namespace RiverBooks.OrderProcessing.Endpoints;

internal class ListOrderForUsersHandler(IOrderRepository repo) : IRequestHandler<ListOrderForUserQuery, Result<List<OrderSummary>>>
{
    private readonly IOrderRepository _repo = repo;
    public async Task<Result<List<OrderSummary>>> Handle(ListOrderForUserQuery request, CancellationToken cancellationToken)
    {
        //TODO: filter by user
        List<Order> orders = await _repo.ListAsync();

        List<OrderSummary> summaries = orders.Select(o => new OrderSummary
        {
            OrderId = o.Id,
            UserId = o.UserId,
            DateCreated = o.DateCreated,
            Total = o.OrderItems.Sum(oi => oi.UnitPrice)//TODO get order items
        }).ToList();

        return summaries;
    }
}
