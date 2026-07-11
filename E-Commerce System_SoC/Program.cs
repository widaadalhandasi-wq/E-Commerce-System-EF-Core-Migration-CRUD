using Backend_session_10_SoC.Presentations;
using Backend_session_10_SoC.Repositories;
using Backend_session_10_SoC.Services;

namespace Backend_session_10_SoC
{
    public class Program
    {
        static void Main(string[] args)
        {
            // ── Manual wiring ──────────────────────────────────────────────----
            // Create the context once — shared by all repositories
            EcommerceContext context = new EcommerceContext();

            // Repositories — data access layer
            UserRepository     userRepo     = new UserRepository(context);
            CategoryRepository categoryRepo = new CategoryRepository(context);
            ProductRepository  productRepo  = new ProductRepository(context);
            OrderRepository    orderRepo    = new OrderRepository(context);
            ReviewRepository   reviewRepo   = new ReviewRepository(context);

            // Services — business logic layer
            UserService     userService     = new UserService(userRepo);
            CategoryService categoryService = new CategoryService(categoryRepo);
            ProductService  productService  = new ProductService(productRepo, categoryRepo);
            OrderService    orderService    = new OrderService(orderRepo, productRepo);
            ReviewService   reviewService   = new ReviewService(reviewRepo);

            // Presentations — user interaction layer
            UserPresentation     userPresentation     = new UserPresentation(userService);
            CategoryPresentation categoryPresentation = new CategoryPresentation(categoryService);
            ProductPresentation  productPresentation  = new ProductPresentation(productService, categoryService);
            OrderPresentation    orderPresentation    = new OrderPresentation(orderService, userService, productService, userRepo);
            ReviewPresentation   reviewPresentation   = new ReviewPresentation(reviewService, productService, userService);

            // ── Menu loop ──────────────────────────────────────────────────
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n============================================");
                Console.WriteLine("          E-Commerce System");
                Console.WriteLine("============================================");
                Console.WriteLine(" 1  - Register User");
                Console.WriteLine(" 2  - Add Product");
                Console.WriteLine(" 3  - Place Order");
                Console.WriteLine(" 4  - Write Review");
                Console.WriteLine(" 5  - Update Product");
                Console.WriteLine(" 6  - Cancel Order");
                Console.WriteLine(" 7  - Delete Review");
                Console.WriteLine(" 8  - View All Products");
                Console.WriteLine(" 9  - Filter Products by Category & Price");
                Console.WriteLine(" 10 - Get Category with Products");
                Console.WriteLine(" 11 - View Order History");
                Console.WriteLine(" 12 - View Product Reviews");
                Console.WriteLine(" 13 - Product Summary Report");
                Console.WriteLine(" 0  - Exit");
                Console.WriteLine("============================================");
                Console.Write("Select option: ");

                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:  userPresentation.RegisterUser();                  break;
                    case 2:  productPresentation.AddProduct();                 break;
                    case 3:  orderPresentation.PlaceOrder();                   break;
                    case 4:  reviewPresentation.WriteReview();                 break;
                    case 5:  productPresentation.UpdateProduct();              break;
                    case 6:  orderPresentation.CancelOrder();                  break;
                    case 7:  reviewPresentation.DeleteReview();                break;
                    case 8:  productPresentation.ViewAllProducts();            break;
                    case 9:  productPresentation.FilterProducts();             break;
                    case 10: categoryPresentation.GetCategoryWithProducts();   break;
                    case 11: orderPresentation.ViewOrderHistory();             break;
                    case 12: reviewPresentation.ViewProductReviews();          break;
                    case 13: productPresentation.ProductSummaryReport();       break;
                    case 0:  exit = true;                                      break;
                    default: Console.WriteLine("Invalid option. Please try again."); break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.WriteLine("Goodbye!");
        }
    }
}
