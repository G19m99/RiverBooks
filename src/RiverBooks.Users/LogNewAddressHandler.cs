using MediatR;
using Microsoft.Extensions.Logging;

namespace RiverBooks.Users;

internal class LogNewAddressHandler(ILogger<LogNewAddressHandler> logger) : INotificationHandler<AddressAddedEvent>
{
    public Task Handle(AddressAddedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("[DE handler] New address to user {user}: {address}", 
            notification.NewAddress.UserId,  
            notification.NewAddress.StreetAddress
        );

        return Task.CompletedTask;
    }
}