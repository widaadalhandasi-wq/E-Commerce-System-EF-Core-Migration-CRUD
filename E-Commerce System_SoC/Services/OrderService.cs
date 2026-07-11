using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Repositories;

namespace Backend_session_10_SoC.Services
{
    public class OrderService
    {
        private OrderRepository orderRepo;
        private ProductRepository productRepo;

        public OrderService(OrderRepository orderRepo, ProductRepository productRepo)
        {
            this.orderRepo   = orderRepo;
            this.productRepo = productRepo;
        }

        public List<Order> GetActiveOrders()
        {
            return orderRepo.GetActiveOrders();
        }

        public User GetUserWithOrderHistory(int userId, UserRepository userRepo)
        {
            return userRepo.GetWithOrdersAndItems(userId);
        }

        public int CreateOrder(int userId, string shippingAddress, string paymentMethod)
        {
            Order order = new Order();
            order.userId          = userId;
            order.orderDate       = DateTime.Now;
            order.totalAmount     = 0;
            order.status          = "Pending";
            order.shippingAddress = shippingAddress;
            order.paymentMethod   = paymentMethod;

            orderRepo.AddOrder(order);
            return order.orderId;
        }

        public void AddItemToOrder(int orderId, int productId, int quantity)
        {
            Product product = productRepo.GetById(productId);

            OrderItem item = new OrderItem();
            item.orderId   = orderId;
            item.productId = productId;
            item.quantity  = quantity;
            item.unitPrice = (decimal)product.price;  // price snapshot at time of order

            orderRepo.AddOrderItem(item);

            // Business rule: decrement stock immediately
            product.stockQuantity -= quantity;
            productRepo.Update();

            // Business rule: accumulate total on the order
            Order order = orderRepo.GetById(orderId);
            order.totalAmount += item.unitPrice * quantity;
            orderRepo.Update();
        }

        public void CancelOrder(int orderId)
        {
            Order order = orderRepo.GetById(orderId);
            List<OrderItem> items = orderRepo.GetItemsByOrderId(orderId);

            // Business rule: restore stock for every item in the cancelled order
            foreach (OrderItem item in items)
            {
                Product product = productRepo.GetById(item.productId);
                product.stockQuantity += item.quantity;
            }

            order.status = "Cancelled";
            orderRepo.Update();
        }
    }
}
