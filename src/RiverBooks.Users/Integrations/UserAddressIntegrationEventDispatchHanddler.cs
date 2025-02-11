using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.Integrations;

internal class UserAddressIntegrationEventDispatchHanddler : INotificationHandler<AddressAddedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserAddressIntegrationEventDispatchHanddler> _logger;

    public UserAddressIntegrationEventDispatchHanddler(IMediator mediator, ILogger<UserAddressIntegrationEventDispatchHanddler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(AddressAddedEvent notification, CancellationToken ct)
    {
        Guid userId = Guid.Parse(notification.NewAddress.UserId);

        UserAddressDetails addressDetails = new(
            userId,
            notification.NewAddress.Id,
            notification.NewAddress.StreetAddress.Street1,
            notification.NewAddress.StreetAddress.Street2,
            notification.NewAddress.StreetAddress.City,
            notification.NewAddress.StreetAddress.State,
            notification.NewAddress.StreetAddress.PostalCode,
            notification.NewAddress.StreetAddress.Country
        );

        await _mediator.Publish(new NewUserAddressAddedIntegrationEvent(addressDetails), ct);

        _logger.LogInformation("[DE handler] New address integration event sent for {user} new address {address}",
            notification.NewAddress.UserId, notification.NewAddress.StreetAddress);
    }
}
