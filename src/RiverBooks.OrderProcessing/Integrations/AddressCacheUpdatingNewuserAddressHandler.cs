using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Infrastructure;
using RiverBooks.OrderProcessing.Interfaces;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Integrations;

internal class AddressCacheUpdatingNewuserAddressHandler(IOrderAddressCache addressCache, ILogger<AddressCacheUpdatingNewuserAddressHandler> logger) 
    : INotificationHandler<NewUserAddressAddedIntegrationEvent>
{
    private readonly IOrderAddressCache _addressCache = addressCache;
    private readonly ILogger<AddressCacheUpdatingNewuserAddressHandler> _logger = logger;

    public async Task Handle(NewUserAddressAddedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        OrderAddress orderAddress = new(
            notification.Details.AddressId,
            new Address(
                notification.Details.Street1,
                notification.Details.Street2,
                notification.Details.City,
                notification.Details.State,
                notification.Details.PostalCode,
                notification.Details.Country
            )
        );

        await _addressCache.StoreAsync(orderAddress);

        _logger.LogInformation("Cache update with new address {address}", orderAddress.Id);
    }
}
