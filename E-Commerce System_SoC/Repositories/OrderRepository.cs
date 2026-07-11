using Backend_session_10_SoC.Models;

namespace Backend_session_10_SoC.Repositories
{
    public class OrderRepository
    {
        private EcommerceContext context;

        public OrderRepository(EcommerceContext context)
        {
            this.context = context;
        }

        public List<Order> GetAll()
        {
            return context.Orders.ToList();
        }

        public List<Order> GetActiveOrders()
        {
            return context.Orders
                .Where(o => o.status != "Cancelled")
                .ToList();
        }

        public Order GetById(int orderId)
        {
            return context.Orders.FirstOrDefault(o => o.orderId == orderId);
        }

        public List<OrderItem> GetItemsByOrderId(int orderId)
        {
            return context.OrderItems
                .Where(i => i.orderId == orderId)
                .ToList();
        }

        public void AddOrder(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public void AddOrderItem(OrderItem item)
        {
            context.OrderItems.Add(item);
            context.SaveChanges();
        }

        public void Update()
        {
            context.SaveChanges();
        }
    }
}
