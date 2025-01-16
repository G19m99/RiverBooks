namespace RiverBooks.Users.CartItemsEndpoints;

public record CartItemDto(Guid Id, Guid BookId, string Description, int Quantity, decimal Price);
