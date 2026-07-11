using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Repositories;

namespace Backend_session_10_SoC.Services
{
    public class ReviewService
    {
        private ReviewRepository reviewRepo;

        public ReviewService(ReviewRepository reviewRepo)
        {
            this.reviewRepo = reviewRepo;
        }

        public List<Review> GetAll()
        {
            return reviewRepo.GetAll();
        }

        public List<Review> GetByProduct(int productId)
        {
            return reviewRepo.GetByProductId(productId);
        }

        public void AddReview(int userId, int productId, int rating, string comment)
        {
            Review review = new Review();
            review.userId     = userId;
            review.productId  = productId;
            review.rating     = rating;
            review.comment    = comment;
            review.reviewDate = DateTime.Now;

            reviewRepo.Add(review);
        }

        public int GetLastReviewId()
        {
            List<Review> all = reviewRepo.GetAll();
            return all[all.Count - 1].reviewId;
        }

        public void DeleteReview(int reviewId)
        {
            Review review = reviewRepo.GetById(reviewId);
            reviewRepo.Delete(review);
        }
    }
}
