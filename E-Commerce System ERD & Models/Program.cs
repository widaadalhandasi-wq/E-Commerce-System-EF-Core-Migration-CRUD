using E_Commerce_System_ERD___Models.Models;
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

            Console.Write("Enter username: ");
            string username = Console.ReadLine();
           
            context.Users.Add(new User
            {
                username= username,
                registrationDate = DateTime.Now,
                isActive = true
            }
            );
            context.SaveChanges();
            Console.WriteLine("User registered successfully");

        }



























        //2-Add a New Product to a Category
        public static void AddProduct() { }


        //3- Place an Order
        public static void PlaceOrder() { }


        //4- Write a Product Review

        public static void WriteaProductReview() { }


        //5-Update Product Price and Availability

        public static void UpdateProduct() { }



        //6-Cancel an Order

        public static void CancelanOrder() { }

        //7- Delete a Review

        public static void DeleteaReview() { }



        //8- View All Products (Get All)

        public static void ViewAllProducts() { }


        //9-  Filter Products by Category and Price Range

        public static void FilterProducts() { }


        //10 -Get Category with All Its Products (Include)
        public static void GetCategorywithAllItsProducts() { }



        //11- View Order History with Full Details (ThenInclude)

        public static void ViewOrderHistory() { }


        //12- Product Summary Report (Projection + Lazy Loading)
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
                Console.WriteLine(" 2  - Add Product");
                Console.WriteLine(" 3  - Place Order");
                Console.WriteLine(" 4  - Write a Product Review");
                Console.WriteLine(" 5  - Update Product");
                Console.WriteLine(" 6  - Cancel an Order");
                Console.WriteLine(" 7  - Delete a Review");
                Console.WriteLine(" 8  - View All Products");
                Console.WriteLine(" 9  - Filter Products");
                Console.WriteLine(" 10 - Get Category with All Its Products");
                Console.WriteLine(" 11 - View Order History");
                Console.WriteLine(" 12 - Product Summary Report");
                Console.WriteLine(" 0  - Exit");
                Console.WriteLine("========================================");
                Console.Write("Select option: ");

                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1: RegisterUser(); break;
                    case 2: AddProduct(); break;
                    case 3: PlaceOrder(); break;
                    case 4: WriteaProductReview(); break;
                    case 5: UpdateProduct(); break;
                    case 6: CancelanOrder(); break;
                    case 7: DeleteaReview(); break;
                    case 8: ViewAllProducts(); break;
                    case 9: FilterProducts(); break;
                    case 10: GetCategorywithAllItsProducts(); break;
                    case 11: ViewOrderHistory(); break;
                    case 12: ProductSummaryReport(); break;
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

