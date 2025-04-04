﻿using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.UseCases.Cart.Checkout;

public record CheckoutCartCommand(
    string Email,
    Guid ShippingAddressId,
    Guid BillingAddressId
    ) : IRequest<Result<Guid>>;
