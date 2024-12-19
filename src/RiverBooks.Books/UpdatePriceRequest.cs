namespace RiverBooks.Books;

public record UpdatePriceRequest(Guid Id, decimal NewPrice);
