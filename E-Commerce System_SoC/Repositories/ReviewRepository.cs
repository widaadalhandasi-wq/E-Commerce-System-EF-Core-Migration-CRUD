using Backend_session_10_SoC.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_session_10_SoC.Repositories
{
    public class ReviewRepository
    {
        private EcommerceContext context;

        public ReviewRepository(EcommerceContext context)
        {
            this.context = context;
        }

        public List<Review> GetAll()
        {
            return context.reviews.ToList();
        }

        public List<Review> GetByProductId(int productId)
        {
            return context.reviews
                .Include(r => r.U)
                .Where(r => r.productId == productId)
                .ToList();
        }

        public Review GetById(int reviewId)
        {
            return context.reviews.FirstOrDefault(r => r.reviewId == reviewId);
        }

        public void Add(Review review)
        {
            context.reviews.Add(review);
            context.SaveChanges();
        }

        public void Delete(Review review)
        {
            context.reviews.Remove(review);
            context.SaveChanges();
        }
    }
}
