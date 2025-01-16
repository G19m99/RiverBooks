namespace RiverBooks.Users.CartItemsEndpoints;

public record AddCartItemRequest(Guid BookId, int Quantity);
