using E_Commerce_System_ERD___Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Threading.Channels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;

namespace E_Commerce_System_ERD___Models
{
    internal class Program
    {

       // Static in-memory context — accessible by all functions without passing parameters
        public static E_CommerceContext context = new E_CommerceContext();


        //1-Register a New User
        public static void RegisterUser() 
        {
            Console.WriteLine("\n=== Register New User ===");

            //user enter the data:
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            //validate
            if (username == null)
            {
                Console.WriteLine("Error: Username cannot be empty.");
                return;

            }

            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            Console.Write("Enter password: ");
            string passwordHash = Console.ReadLine();
            Console.Write("Enter full name: ");
            string fullName = Console.ReadLine();
            Console.Write("Enter phone number (optional, press Enter to skip): ");
            string phone = Console.ReadLine();
            Console.Write("Enter address (optional, press Enter to skip): ");
            string address = Console.ReadLine();
            //-------------------------------------------------------

            // Create a new User object from user inputs:
            User newUser = new User
            {
                username = username,
                email= email,
                passwordHash= passwordHash,
                fullName= fullName,
                phoneNumber= phone,
                address= address,
                registrationDate = DateTime.Now,
                isActive = true
            };
            //-------------------------------------------------------

            // Add the new user entity to the context
            context.Users.Add(newUser);
            //-------------------------------------------------------

            //save the changes to database 
            context.SaveChanges();
            //-------------------------------------------------------

            // Display the assigned userId after saving
            Console.WriteLine($"User registered successfully. Assigned ID: {newUser.userId}");

        }
        //==================================================================================================

        //2-Add a New Category (Required before adding products)
        public static void AddCategory()
        {
            Console.WriteLine("\n=== Add New Category ===");

            Console.Write("Enter category name: ");
            string name = Console.ReadLine();

            bool isDuplicate = context.Categories.Any(c => c.categoryName.ToLower() == name.ToLower());
            if (isDuplicate)
            {
                Console.WriteLine($"Error: A category named '{name}' already exists in the database!");
                return; 
            }
            Console.Write("Enter category description (optional): ");
            string description = Console.ReadLine();

            // 1. Create a new Category object
            Category newCategory = new Category
            {
                categoryName = name,
                description = description
            };

            // 2. Add the category entity to the context
            context.Categories.Add(newCategory);

            // 3. Save changes to the database
            context.SaveChanges();

            Console.WriteLine($"Category '{newCategory.categoryName}' added successfully! Assigned ID: {newCategory.categoryId}");
        }

        //==================================================================================================
        //3-Add a New Product to a Category
        public static void AddProduct()
        {
            Console.WriteLine("\n=== Add Product ===");
            //Display all categories with context.Categories.ToList()
            List<Category> categories =context.Categories.ToList();
            foreach(Category c in categories) 
            {
                Console.WriteLine($"  ID: {c.categoryId}  |  {c.categoryName}  | {c.description} ");
            }
            //----------------------------------------------

            //Read category selection and all product details from the user
            Console.Write("Enter category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            Category category = context.Categories.FirstOrDefault(c => c.categoryId == categoryId);
            if (category == null)
            {
                Console.WriteLine("Error: Category not found!");
                return;
            }
             
            //----------------------------------------------
            Console.Write("Enter product name: ");
            string productName = Console.ReadLine();

            Console.Write("Enter description (optional): ");
            string desc = Console.ReadLine();

            Console.Write("Enter price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.Write("Enter stock quantity: ");
            int stock = int.Parse(Console.ReadLine());
            //----------------------------------------------


            Product newProduct = new Product
            {
                productName = productName,
                description= desc,
                price= price,
                stockQuantity=stock,
                createdAt = DateTime.Now,
                isAvailable = true,
                Category= category    // set navigation property directly insted of using catogryId , it well read it form the relationship.
            };

            //----------------------------------------------

            context.Products.Add(newProduct);    //add new product to the list 

            context.SaveChanges();

            Console.WriteLine($"Product added. ID: {newProduct.productId}");

        }

        //==================================================================================================

        //4- Place an Order
        public static void PlaceOrder()
        {
            Console.WriteLine("\n=== Place New Order ===");

            // Display all users so we know who is placing the order
            List<User> users = context.Users.ToList();
            Console.WriteLine("Available Users:");
            foreach (User u in users)
            {
                Console.WriteLine($"  ID: {u.userId} | Username: {u.username}");
            }

            Console.Write("Enter user ID: ");
            int userId = int.Parse(Console.ReadLine());
            User user = context.Users.FirstOrDefault(u => u.userId == userId);
            if (user == null)
            {
                Console.WriteLine("Error:User Id Not Found.");
                return;
            }

            //----------------------------------------------------------------
            Console.Write("Enter shipping address: ");
            string shippingAddress = Console.ReadLine();
            Console.Write("Enter payment method (e.g., Credit Card, Cash, PayPal): ");
            string paymentMethod = Console.ReadLine();
            
            Order newOrder = new Order
            {
                user = user,                             // Link the order to our user object
                orderDate = DateTime.Now,
                status = orderSatus.Pending,              // Initial status for a new order
                shippingAddress = shippingAddress,
                totalAmount = 0,                     // Will be calculated based on selected items
                orderItems = new List<OrderItem>(),     // Initialize the empty collection for items
                paymentMethod = paymentMethod
            };

            // Add the order to the context
            context.Orders.Add(newOrder);
            context.SaveChanges();

            //----------------------------------------------------------------
            // Add products to the order
            bool addingItems = true;
            while (addingItems)
            {
                Console.WriteLine("\nAvailable products:");
                foreach (Product p in context.Products.Where(p => p.isAvailable && p.stockQuantity > 0).ToList())
                {
                    Console.WriteLine($"  ID: {p.productId}  |  {p.productName}  |  {p.price:C}  |  Stock: {p.stockQuantity}");
                }

                //-------------------------------------

                Console.Write("Enter product ID to add (0 to finish): ");
                int productId = int.Parse(Console.ReadLine());
                if (productId == 0) break;

                Product product = context.Products.FirstOrDefault(p => p.productId == productId);
                if (product == null || !product.isAvailable)
                {
                    Console.WriteLine("Error: Product not found or unavailable.");
                    continue;
                }
                //-------------------------------------
                Console.Write("Enter quantity: ");
                int qty = int.Parse(Console.ReadLine());

                if (qty > product.stockQuantity)
                {
                    Console.WriteLine($"Error: Insufficient stock. Only {product.stockQuantity} available.");
                    continue;
                }
                //-------------------------------------

                OrderItem item = new OrderItem
                {
                    orderId = newOrder.orderId,      // Uses the ID generated from step 1
                    productId = product.productId,
                    quantity = qty,
                    unitPrice = product.price        // Snapshot price at the time of ordering
                };
                //---------------------------------------

                // Add the item to context
                context.OrderItems.Add(item);

                // Update stock quantity and aggregate running total amount
                product.stockQuantity -= qty;
                newOrder.totalAmount += item.unitPrice * qty;
            }

            
            context.SaveChanges();

            Console.WriteLine($"\nOrder placed successfully! Order ID: {newOrder.orderId} | Total: {newOrder.totalAmount:C}");
        }
        //==================================================================================================

        //5- Write a Product Review

        public static void WriteaProductReview()
        {
            Console.WriteLine("\n=== Write a Product Review ===");
            //Display available users

            List<User> users = context.Users.ToList();
            Console.WriteLine("Available Users:");
            foreach (User u in users)
            {
                Console.WriteLine($"  ID: {u.userId} | Username: {u.username}");
            }

            Console.Write("Enter user ID: ");
            int userId = int.Parse(Console.ReadLine());
            User user = context.Users.FirstOrDefault(u => u.userId == userId);
            if (user == null)
            {
                Console.WriteLine("Error:User Id Not Found.");
                return;
            }
            //----------------------------------------------------------------------------
            //Display available Products

            List<Product> Products = context.Products.ToList();
            Console.WriteLine("Available Products:");
            foreach (Product p in Products)
            {
                Console.WriteLine($"  ID: {p.productId} | product Name: {p.productName}");
            }

            Console.Write("Enter product ID To Review: ");
            int productId = int.Parse(Console.ReadLine());
            Product product = context.Products.FirstOrDefault(p => p.productId == productId);
            if (product == null)
            {
                Console.WriteLine("Error:User Id Not Found.");
                return;
            }
            //----------------------------------------------------------------------------
            //Enter Review rating and comment:

             Console.Write("Enter rating (1-5): ");
             int rating = int.Parse(Console.ReadLine());

            if (rating < 1 || rating > 5)
            {
                Console.WriteLine("Error: Rating must be an integer between 1 and 5.");
                return;
            }

             Console.Write("Enter comment (optional): ");
             string comment = Console.ReadLine();

            //--------------------------------------------

            // Create a Review 
            Review newReview = new Review
            {
                userId=userId,
                productId=productId,
                rating= rating,
                comment=comment,
                reviewDate=DateTime.Now
            };

            //--------------------------------------------

            context.Reviews.Add(newReview); 
            context.SaveChanges();

            Console.WriteLine($"Review submitted! Review ID: {newReview.reviewId}");

        }

        //==================================================================================================

        //6-Update Product Price and Availability

        public static void UpdateProduct()
        {
            Console.WriteLine("\n=== Update Product ===");
            // 1. Ask for Product ID
            Console.Write("Enter product ID : ");
            int productId = int.Parse(Console.ReadLine());

            //2-Fetch the product using context.Products.FirstOrDefault()
            Product product = context.Products.FirstOrDefault(p => p.productId == productId);
            if (product == null)
            {
                Console.WriteLine("Error:User Id Not Found.");
                return;
            }

            // 3. Read new inputs from the manager
            Console.Write("Enter New Price :");
            decimal newPrice = Decimal.Parse(Console.ReadLine());

            Console.Write("Is the product available? (true/false): ");
            bool newAvailability = bool.Parse(Console.ReadLine());

            // 4. Update the fields
            product.price = newPrice;
            product.isAvailable = newAvailability;

            //5- Call context.SaveChanges() 
            context.SaveChanges();

            //6-Confirm the update to the manager
            Console.WriteLine($"\nSuccess: Product '{product.productName}' updated successfully!");
        }


        //==================================================================================================

        //7-Cancel an Order

        public static void CancelanOrder()
        {
            Console.WriteLine("\n=== Cancel an Order ===");

            List<Order> allOrders = context.Orders.ToList();

            if (!allOrders.Any())
            {
                Console.WriteLine("There are no orders available in the system.");
                return;
            }

            Console.WriteLine("Available Orders in the System:");
            foreach (var o in allOrders)
            {
                Console.WriteLine($"  Order ID: {o.orderId} | Date: {o.orderDate.ToShortDateString()} | Total: {o.totalAmount:C} | Status: {o.status}");
            }

            //---------------------------------------------
            //Ask for Order ID
            Console.WriteLine("Enter Order Id to cancel:");
            int orderid=int.Parse(Console.ReadLine());

            // Fetch the order by ID using FirstOrDefault()
            Order order = context.Orders.FirstOrDefault(o => o.orderId== orderid);
            if (order == null)
            {
                Console.WriteLine("Error:Order Id Not Found.");
                return;
            }

            //Load all OrderItems for that order from context.OrderItems
            List<OrderItem> items = context.OrderItems.Where(oi => oi.orderId == orderid).ToList();

            // For each OrderItem, find the related product and restore its stockQuantity
            foreach (OrderItem item in items)
            {
                // Find the product related to this specific order item
                Product product = context.Products.FirstOrDefault(p => p.productId == item.productId);

                if (product != null)
                {
                    product.stockQuantity += item.quantity;
                    Console.WriteLine($"  -> Restored {item.quantity} units to Product: '{product.productName}'");
                }
            }

            //Set order.status = "Cancelled" 
            order.status = orderSatus.Cancelled;

            // call SaveChanges()
            context.SaveChanges();

            Console.WriteLine($"\nSuccess: Order ID {order.orderId} has been successfully Cancelled, and stock is restored!");
        }


        //==================================================================================================

        //8- Delete a Review

        public static void DeleteaReview() 
        {
            Console.WriteLine("\n=== Delete a Review ===");
            List<Review> reviews = context.Reviews.ToList();

            if (!reviews.Any())
            {
                Console.WriteLine("There are no reviews available in the system.");
                return;
            }

            Console.WriteLine("Available Reviews in the System:");
            foreach (var r in reviews)
            {
                Console.WriteLine($"  Review ID: {r.reviewId} | Rating: {r.rating}/5 | Comment: {r.comment}");
            }

                //--------------------------------------
                Console.WriteLine("Enter Review Id To Delete It:");
            int reviewId=int.Parse(Console.ReadLine());

            Review review = context.Reviews.FirstOrDefault(r => r.reviewId==reviewId);
            if (review == null)
            {
                Console.WriteLine("Error: Review ID not found in the database.");
                return; 
            }

            context.Reviews.Remove(review);
            context.SaveChanges();
            Console.WriteLine($"\nSuccess: Review ID {reviewId} has been deleted permanently from the database.");

        }
        //==================================================================================================

        //9- View All Products (Get All)

        public static void ViewAllProducts()
        {
            Console.WriteLine("\n=== All Products ===");
            //Use context.Products.ToList() to retrieve all products
            List<Product> products = context.Products.Include(p => p.Category).ToList();
            if (!products.Any())
            {
                Console.WriteLine("No products found in the catalog.");
                return;
            }
            //Display each product's details in a formatted list
            foreach (Product p in context.Products)
             {
                string availability = p.isAvailable ? "Yes" : "No";

                Console.WriteLine($"ID: {p.productId}  |  {p.productName}  |  Price: {p.price:C}" +
                                 $"  |  Stock: {p.stockQuantity}  |  Category: {p.Category.categoryName}" +
                                 $"  |  Available: {availability}");
             }

            

        }

        //==================================================================================================

        //10- Filter Products by Category and Price Range

        public static void FilterProducts() { }

        //==================================================================================================

        //11 -Get Category with All Its Products (Include)
        public static void GetCategorywithAllItsProducts() { }


        //==================================================================================================

        //12- View Order History with Full Details (ThenInclude)

        public static void ViewOrderHistory() { }

        //==================================================================================================

        //13- Product Summary Report (Projection + Lazy Loading)
        public static void ProductSummaryReport() { }









































        static void Main(string[] args)
        {
          
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n========================================");
                Console.WriteLine("        E-Commerce System");
                Console.WriteLine("========================================");
                Console.WriteLine(" 1  - Register User");
                Console.WriteLine(" 2  - Add Category");
                Console.WriteLine(" 3  - Add Product");
                Console.WriteLine(" 4  - Place Order");
                Console.WriteLine(" 5  - Write a Product Review");
                Console.WriteLine(" 6  - Update Product");
                Console.WriteLine(" 7  - Cancel an Order");
                Console.WriteLine(" 8  - Delete a Review");
                Console.WriteLine(" 9  - View All Products");
                Console.WriteLine(" 10  - Filter Products");
                Console.WriteLine(" 11 - Get Category with All Its Products");
                Console.WriteLine(" 12 - View Order History");
                Console.WriteLine(" 13 - Product Summary Report");
                Console.WriteLine(" 0  - Exit");
                Console.WriteLine("========================================");
                Console.Write("Select option: ");

                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1: RegisterUser(); break;
                    case 2: AddCategory(); break;
                    case 3: AddProduct(); break;
                    case 4: PlaceOrder(); break;
                    case 5: WriteaProductReview(); break;
                    case 6: UpdateProduct(); break;
                    case 7: CancelanOrder(); break;
                    case 8: DeleteaReview(); break;
                    case 9: ViewAllProducts(); break;
                    case 10: FilterProducts(); break;
                    case 11: GetCategorywithAllItsProducts(); break;
                    case 12: ViewOrderHistory(); break;
                    case 13: ProductSummaryReport(); break;
                    case 0: exit = true; break;
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

