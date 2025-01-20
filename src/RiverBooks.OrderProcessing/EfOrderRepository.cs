using Microsoft.EntityFrameworkCore;


namespace RiverBooks.OrderProcessing;

internal class EfOrderRepository(OrderProcessingDbContext context) : IOrderRepository
{
    private readonly OrderProcessingDbContext _context = context;

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task<List<Order>> ListAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
