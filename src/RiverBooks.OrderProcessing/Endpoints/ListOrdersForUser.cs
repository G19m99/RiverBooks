using Ardalis.Result;
using FastEndpoints;
using MediatR;
using System.Security.Claims;

namespace RiverBooks.OrderProcessing.Endpoints;

internal class ListOrdersForUser(IMediator mediator) : EndpointWithoutRequest<ListOrdersForUserResponse>
{
    private readonly IMediator _mediator = mediator;
    public override void Configure()
    {
        Get("/orders");
        Claims("EmailAddress");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string emailAddress = User.FindFirstValue("EmailAddress")!;

        var query = new ListOrderForUserQuery(emailAddress);

        var response = await _mediator.Send(query, ct);

        if (response.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
        }
        else
        {
            ListOrdersForUserResponse results = new()
            {
                Orders = response.Value
                .Select(o => new OrderSummary
                {
                    DateCreated = o.DateCreated,
                    DateShipped = o.DateShipped,
                    Total = o.Total,
                    UserId = o.UserId,
                    OrderId = o.OrderId
                })
                .ToList()
            };

            await SendAsync(results, cancellation: ct);
        }
    }
}
