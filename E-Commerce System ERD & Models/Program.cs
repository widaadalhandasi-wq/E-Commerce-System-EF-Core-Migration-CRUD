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

            // load the order along with its related list of order items from the database in a single query by using eager loading --include--
            Order? order = context.Orders.Include(o => o.orderItems)
                                        .FirstOrDefault(o => o.orderId== orderid);
            if (order == null)
            {
                Console.WriteLine("Error:Order Id Not Found.");
                return;
            }

            //Load all OrderItems for that order from context.OrderItems
            //List<OrderItem> items = context.OrderItems.Where(oi => oi.orderId == orderid).ToList();

            // For each OrderItem, find the related product and restore its stockQuantity
            foreach (OrderItem item in order.orderItems)
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

            //if there is nothing in the product list:
            if (!products.Any())
            {
                Console.WriteLine("No products found in the catalog.");
                return;
            }

            //Display each product's details in a formatted list
            foreach (Product p in context.Products)
             {
                // Format availability status as text by using Ternary operator:
                string availability = p.isAvailable ? "Yes" : "No";

                Console.WriteLine($"ID: {p.productId}  |  {p.productName}  |  Price: {p.price:C}" +
                                 $"  |  Stock: {p.stockQuantity}  |  Category: {p.Category.categoryName}" +
                                 $"  |  Available: {availability}");
             }

            

        }

        //==================================================================================================

        //10- Filter Products by Category and Price Range
        public static void FilterProducts()
        {
            Console.WriteLine("\n=== Filter Products ===");

            //display List of Catogries:
            List<Category> categories = context.Categories.ToList();
            Console.WriteLine("Available Categories:");
            foreach (Category c in categories)
            {
                Console.WriteLine($"  ID: {c.categoryId} | Category Name: {c.categoryName}");
            }

            //user enter the data
            Console.WriteLine("Enter Catogry Id:");
            int catogryId = int.Parse(Console.ReadLine());

            // Validation: Check if the entered Category ID actually exists
            if (!categories.Any(c => c.categoryId == catogryId))
            {
                Console.WriteLine("Error: Category Id Not Found.");
                return;
            }

            Console.WriteLine("Enter Minimum Price You Want: ");
            decimal minPrice = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter Maximum Price You Want: ");
            decimal maxPrice = decimal.Parse(Console.ReadLine());

            //----------------------------------------------------------------

            //Bring the cheapest product by using MinBy()...
            Product? minProduct = context.Products.Where(p => p.categoryId == catogryId)
                                                  .OrderBy(p => p.price)
                                                  .FirstOrDefault();
            //Bring the expensive product by using MaxBy()...
            Product ? maxProduct = context.Products.Where(p => p.categoryId == catogryId)
                                                   .OrderByDescending(p => p.price)
                                                   .FirstOrDefault();

            //----------------------------------------------------------------
            //Chain .OrderBy(p => p.price) before .ToList()
            List<Product> productList = context.Products.Where(p => p.categoryId == catogryId && p.price >= minPrice && p.price <= maxPrice)
                                                        .OrderBy(p => p.price).ToList();

            //----------------------------------------------------------------
            // Display the filtered and sorted results
            Console.WriteLine($"\n=== Filtered Results (Total: {productList.Count}) ===");
            if (productList.Count == 0)
            {
                Console.WriteLine("No products found within this price range.");
            }
            else
            {
                foreach (Product p in productList)
                {
                    Console.WriteLine($"Product: {p.productName} | Price: {p.price:C}");
                }
            }

        }

        //==================================================================================================

        //11 -Get Category with All Its Products (Include)
        public static void GetCategorywithAllItsProducts() 
        {
            Console.WriteLine("\n=== Get Category with All Its Products ===");

            //Ask the user for a category ID
            Console.WriteLine("Enter Catogry Id:");
            int Id = int.Parse(Console.ReadLine());

            //Use context.Categories.Include(c => c.Products) and chain .FirstOrDefault() o get the SPECIFIC category.
            Category? chosenCategory = context.Categories.Include(c => c.products)
                                                   .FirstOrDefault(c => c.categoryId == Id);
            
            // Validation: Check if the category exists
            if (chosenCategory == null)
            {
                Console.WriteLine("Error: Category Id Not Found.");
                return;
            }

            // Display the category name and description
            Console.WriteLine($"\nCategory Name: {chosenCategory.categoryName}");
            Console.WriteLine($"Description: {chosenCategory.description}");
            Console.WriteLine("------------------------------------------");


            // List all its products (No second query fires — category.products is already populated)
            Console.WriteLine("Products in this category:");

            if (chosenCategory.products == null)
            {
                Console.WriteLine("  (No products available in this category)");
            }
            else
            {
                foreach (Product p in chosenCategory.products)
                {
                    Console.WriteLine($"  - Product ID: {p.productId} | Name: {p.productName} | Price: {p.price:C}");
                }
            }

        }


        //==================================================================================================


        //12- View Order History with Full Details (ThenInclude)
        public static void ViewOrderHistory()
        {
            Console.WriteLine("\n=== View Order History ===");
            //1- Ask the user for their userId
            Console.WriteLine("Enter User Id:");
            int Id = int.Parse(Console.ReadLine());

            //2-Enter to the user Lists , then select the orders for that user , from that order select the order items ,
            //from that order items select the product , and bring the first user who match the id that entred by user
            User? user = context.Users.Include(u => u.userOrders).ThenInclude(o => o.orderItems).ThenInclude(i => i.product).FirstOrDefault(u =>u.userId == Id);   //i => item

            // Validation: Check if user exists
            if (user == null)
            {
                Console.WriteLine("Error: User Id Not Found.");
                return;
            }

            Console.WriteLine($"\nOrder History for User: {user.fullName} (ID: {user.userId})");
            Console.WriteLine("==================================================");


            // Check if the user has any orders
            if (user.userOrders == null)
            {
                Console.WriteLine("This user hasn't placed any orders yet.");
                return;
            }

            // 3. Loop through user.userOrders 
            foreach (Order o in user.userOrders)
            {
                Console.WriteLine($"\n Order ID: {o.orderId} | Date: {o.orderDate} | Status: {o.status} | Total: {o.totalAmount:C}");
                Console.WriteLine("   Breakdown of Products:");

                // 4. Inside each order, loop through order.orderItems
                foreach (OrderItem item in o.orderItems)
                {
                    // 5. Display item.product.productName and item.unitPrice safely
                    if (item.product != null)
                    {
                        Console.WriteLine($"     - {item.product.productName} (Qty: {item.quantity}) | Unit Price at Purchase: {item.unitPrice:C}");
                    }
                    else
                    {
                        Console.WriteLine($"     - Unknown Product (Qty: {item.quantity}) | Unit Price at Purchase: {item.unitPrice:C}");
                    }
                }
                Console.WriteLine("   -----------------------------------------------");
            }
        }

        //==================================================================================================

        //13- Product Summary Report (Projection + Lazy Loading)
        public static void ProductSummaryReport()
        {
            Console.WriteLine("\n=== Product Summary Report (Dashboard) ===");

            // Part A: Projection into an anonymous object (Single efficient SQL query)
            var reportData = context.Products.Select(p => new
            {
                ProductName = p.productName,
                CategoryName = p.Category.categoryName, //  Navigation Property
                CurrentStock = p.stockQuantity,
                ReviewCount = p.Reviews.Count(),
                AvgRating = p.Reviews.Any() ? p.Reviews.Average(r => r.rating) : 0.0
            }).ToList();

            Console.WriteLine("\n---------------------------------------------------------------------------------");
            Console.WriteLine($"{"Product Name",-20} | {"Category",-15} | {"Stock",-6} | {"Reviews Count",-13} | {"Avg Rating",-10}");
            Console.WriteLine("---------------------------------------------------------------------------------");

            foreach (var item in reportData)
            {
                Console.WriteLine($"{item.ProductName,-20} | {item.CategoryName,-15} | {item.CurrentStock,-6} | {item.ReviewCount,-13} | {item.AvgRating,-10:F1}");
            }

            // Part B — Lazy Loading Demo 
            Console.WriteLine("\n=== Lazy Loading Demonstration ===");

            var singleProduct = context.Products.FirstOrDefault();

            if (singleProduct != null)
            {
                Console.WriteLine($"\nFetched Product: {singleProduct.productName}");
                Console.WriteLine("Notice: No reviews have been loaded from the DB yet...");

                Console.WriteLine("\n[Press Enter to access the Reviews navigation property...]");
                Console.ReadLine();

                if (singleProduct.Reviews != null && singleProduct.Reviews.Any())
                {
                    Console.WriteLine($"--> Successfully lazy-loaded {singleProduct.Reviews.Count()} reviews!");
                    foreach (Review review in singleProduct.Reviews)
                    {
                        Console.WriteLine($"   - Rating: {review.rating} | Comment: {review.comment}");
                    }
                }
                else
                {
                    Console.WriteLine("--> This product has no reviews (or lazy loading returned empty).");
                }
            }

        }



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
