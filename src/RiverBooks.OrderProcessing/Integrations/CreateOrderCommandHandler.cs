using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Contracts;

namespace RiverBooks.OrderProcessing.Integrations;
internal class CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger, IOrderAddressCache cache) : IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ILogger<CreateOrderCommandHandler> _logger = logger;
    private readonly IOrderAddressCache _addressCache = cache;

    public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<OrderItem> items = request.OrderItems.Select(oi => new OrderItem(oi.BookId, oi.Quantity, oi.UnitPrice, oi.Description));

        Result<OrderAddress> shippingAddress = await _addressCache.GetByIdAsync(request.ShippingAddressId);
        Result<OrderAddress> billingAddress = await _addressCache.GetByIdAsync(request.BillingAddressId);

        Order newOrder = Order.Factory.Create(
            request.UserId,
            shippingAddress.Value.Address,
            billingAddress.Value.Address,
            items);

        await _orderRepository.AddAsync(newOrder);
        await _orderRepository.SaveChangesAsync();

        _logger.LogInformation("New Order Created {id}", newOrder.Id);
        return new OrderDetailsResponse(newOrder.Id);
    }
}
