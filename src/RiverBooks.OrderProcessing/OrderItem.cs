using Ardalis.GuardClauses;

namespace RiverBooks.OrderProcessing;

public class OrderItem(Guid bookId, int quantity, decimal unitPrice, string description)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookId { get; set; } = Guard.Against.Default(bookId);
    public int Quantity { get; set; } = Guard.Against.Negative(quantity);
    public decimal UnitPrice { get; set; } = Guard.Against.Negative(unitPrice);
    public string Description { get; set; } = Guard.Against.NullOrEmpty(description);
}
