namespace RiverBooks.Books;

internal record CreateBookRequest(Guid? Id, string Title, string Author, decimal Price);
