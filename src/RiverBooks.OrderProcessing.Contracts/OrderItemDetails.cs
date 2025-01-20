namespace RiverBooks.OrderProcessing.Contracts;

public record OrderItemDetails(Guid BookId, string Description, int Quantity, decimal UnitPrice);
