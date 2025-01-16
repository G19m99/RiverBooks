namespace RiverBooks.Books.Contracts;

public record BookDetailsResponse(Guid Id, string Title, string Author, decimal Price);
