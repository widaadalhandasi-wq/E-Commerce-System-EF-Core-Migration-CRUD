using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Repositories;
using Backend_session_10_SoC.Services;

namespace Backend_session_10_SoC.Presentations
{
    public class OrderPresentation
    {
        private OrderService orderService;
        private UserService userService;
        private ProductService productService;
        private UserRepository userRepo;

        public OrderPresentation(OrderService orderService, UserService userService,
                                 ProductService productService, UserRepository userRepo)
        {
            this.orderService   = orderService;
            this.userService    = userService;
            this.productService = productService;
            this.userRepo       = userRepo;
        }

        public void PlaceOrder()
        {
            Console.WriteLine("\n=== Place New Order ===");

            Console.Write("Enter user ID: ");
            int userId = int.Parse(Console.ReadLine());

            Console.Write("Enter shipping address: ");
            string shippingAddress = Console.ReadLine();

            Console.WriteLine("Payment methods: 1-CreditCard  2-DebitCard  3-PayPal  4-Cash");
            Console.Write("Choose: ");
            int payChoice = int.Parse(Console.ReadLine());
            string[] methods = { "CreditCard", "DebitCard", "PayPal", "Cash" };
            string paymentMethod = methods[payChoice - 1];

            int orderId = orderService.CreateOrder(userId, shippingAddress, paymentMethod);

            bool addingProducts = true;
            while (addingProducts == true)
            {
                List<Product> available = productService.GetAvailable();
                Console.WriteLine("\nAvailable products:");
                foreach (Product p in available)
                    Console.WriteLine($"  ID: {p.productId}  |  {p.productName}  |  {p.price:C}  |  Stock: {p.stockQuantity}");

                Console.Write("Enter product ID to add (0 to finish): ");
                int productId = int.Parse(Console.ReadLine());
                if (productId == 0) break;

                Console.Write("Enter quantity: ");
                int qty = int.Parse(Console.ReadLine());

                orderService.AddItemToOrder(orderId, productId, qty);
                Console.WriteLine("Item added to order.");

                Console.WriteLine("do you want to add extra products? Y or N");
                string response = Console.ReadLine().Trim().ToLower();
                if (response != "y")
                {
                    addingProducts = false;
                }
            }

            Console.WriteLine($"\nOrder placed! Order ID: {orderId}");
        }

        public void CancelOrder()
        {
            Console.WriteLine("\n=== Cancel an Order ===");

            List<Order> orders = orderService.GetActiveOrders();
            foreach (Order o in orders)
                Console.WriteLine($"  ID: {o.orderId}  |  Date: {o.orderDate:d}  |  Status: {o.status}  |  Total: {o.totalAmount:C}");

            Console.Write("Enter order ID to cancel: ");
            int orderId = int.Parse(Console.ReadLine());

            orderService.CancelOrder(orderId);

            Console.WriteLine($"Order {orderId} cancelled and stock restored.");
        }

        public void ViewOrderHistory()
        {
            Console.WriteLine("\n=== Order History with Full Details ===");

            List<User> users = userService.GetAll();
            Console.WriteLine("Users:");
            foreach (User u in users)
                Console.WriteLine($"  ID: {u.userId}  |  {u.Name}");

            Console.Write("Enter user ID: ");
            int userId = int.Parse(Console.ReadLine());

            User user = orderService.GetUserWithOrderHistory(userId, userRepo);

            Console.WriteLine($"\nOrder history for {user.fullName}:");

            foreach (Order o in user.Orders)
            {
                Console.WriteLine($"\n  Order ID: {o.orderId}  |  Date: {o.orderDate:d}" +
                                  $"  |  Status: {o.status}  |  Total: {o.totalAmount:C}");

                foreach (OrderItem item in o.OrderItems)
                    Console.WriteLine($"    - {item.Product.productName}  x{item.quantity}  @ {item.unitPrice:C}");
            }

            decimal grandTotal = 0;
            foreach (Order o in user.Orders)
                grandTotal += o.totalAmount;

            Console.WriteLine($"\n  TOTAL SPENT: {grandTotal:C}");
        }
    }
}
