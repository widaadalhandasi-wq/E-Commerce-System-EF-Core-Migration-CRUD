using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Services;

namespace Backend_session_10_SoC.Presentations
{
    public class ReviewPresentation
    {
        private ReviewService reviewService;
        private ProductService productService;
        private UserService userService;

        public ReviewPresentation(ReviewService reviewService, ProductService productService,
                                  UserService userService)
        {
            this.reviewService  = reviewService;
            this.productService = productService;
            this.userService    = userService;
        }

        public void WriteReview()
        {
            Console.WriteLine("\n=== Write a Review ===");

            List<User> users = userService.GetAll();
            Console.WriteLine("Users:");
            foreach (User u in users)
                Console.WriteLine($"  ID: {u.userId}  |  {u.Name}");
            Console.Write("Enter user ID: ");
            int userId = int.Parse(Console.ReadLine());

            List<Product> products = productService.GetAll();
            Console.WriteLine("\nProducts:");
            foreach (Product p in products)
                Console.WriteLine($"  ID: {p.productId}  |  {p.productName}");
            Console.Write("Enter product ID to review: ");
            int productId = int.Parse(Console.ReadLine());

            Console.Write("Enter rating (1-5): ");
            int rating = int.Parse(Console.ReadLine());

            Console.Write("Enter comment (optional): ");
            string comment = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(comment)) comment = null;

            reviewService.AddReview(userId, productId, rating, comment);

            int newId = reviewService.GetLastReviewId();
            Console.WriteLine($"Review submitted! Review ID: {newId}");
        }

        public void DeleteReview()
        {
            Console.WriteLine("\n=== Delete a Review ===");

            List<Review> reviews = reviewService.GetAll();
            foreach (Review r in reviews)
                Console.WriteLine($"  ID: {r.reviewId}  |  Rating: {r.rating}  |  Comment: {r.comment}");

            Console.Write("Enter review ID to delete: ");
            int reviewId = int.Parse(Console.ReadLine());

            reviewService.DeleteReview(reviewId);

            Console.WriteLine($"Review {reviewId} deleted.");
        }

        public void ViewProductReviews()
        {
            Console.WriteLine("\n=== Product Reviews ===");

            List<Product> products = productService.GetAll();
            Console.WriteLine("Products:");
            foreach (Product p in products)
                Console.WriteLine($"  ID: {p.productId}  |  {p.productName}");

            Console.Write("Enter product ID: ");
            int productId = int.Parse(Console.ReadLine());

            Product product = productService.GetById(productId);
            List<Review> reviews = reviewService.GetByProduct(productId);

            Console.WriteLine($"\nReviews for '{product.productName}':");
            foreach (Review r in reviews)
            {
                Console.WriteLine($"  [{r.rating}/5] by {r.U.fullName} on {r.reviewDate:d}");
                if (!string.IsNullOrWhiteSpace(r.comment))
                    Console.WriteLine($"    \"{r.comment}\"");
            }

            if (reviews.Count > 0)
            {
                double total = 0;
                foreach (Review r in reviews)
                    total += r.rating;
                double avg = total / reviews.Count;
                Console.WriteLine($"\n  Average Rating: {avg:F1} / 5.0  ({reviews.Count} reviews)");
            }
            else
            {
                Console.WriteLine("  No reviews yet.");
            }
        }
    }
}
