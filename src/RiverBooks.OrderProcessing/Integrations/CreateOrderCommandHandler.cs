using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Contracts;
using Serilog;

namespace RiverBooks.OrderProcessing.Integrations;

internal class CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger) : IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ILogger<CreateOrderCommandHandler> _logger = logger;

    public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        //get the order items and map it into domain objects
        IEnumerable<OrderItem> items = request.OrderItems.Select(oi => new OrderItem(oi.BookId, oi.Quantity, oi.UnitPrice, oi.Description));

        //TODO: replace with the real address
        Address shippingAddress = new("123 any lane", "", "NoWhere", "NoState", "08701", "USA");
        Address billingAddress = new("123 any lane", "", "NoWhere", "NoState", "08701", "USA");

        Order newOrder = Order.Factory.Create(
            request.UserId,
            shippingAddress,
            billingAddress,
            items);

        await _orderRepository.AddAsync(newOrder);
        await _orderRepository.SaveChangesAsync();

        _logger.LogInformation("New Order Created {id}", newOrder.Id);
        return new OrderDetailsResponse(newOrder.Id);
    }
}
