namespace RiverBooks.OrderProcessing;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Address ShippingAddress { get; set; } = default!;
    public Address BillingAddress { get; set; } = default!;

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

    private void AddOrderItem(OrderItem item) => _orderItems.Add(item);

    //in order to handle domain events
    internal class Factory
    {
        public static Order Create(
            Guid UserId,
            Address ShippingAddress,
            Address BillingAddress,
            IEnumerable<OrderItem> orderItems)
        {
            Order order = new()
            {
                UserId = UserId,
                ShippingAddress = ShippingAddress,
                BillingAddress = BillingAddress
            };

            foreach (OrderItem item in orderItems)
            {
                order.AddOrderItem(item);
            }

            return order;
        }
    }

}
