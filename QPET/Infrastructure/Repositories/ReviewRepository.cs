using Microsoft.EntityFrameworkCore;
using QPET.Application.Interfaces;
using QPET.Data;
using QPET.Domain.Entities;

namespace QPET.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return review;
        }

        public async Task<List<Review>> GetLatestAsync(int limit)
        {
            return await BuildReviewQuery()
                .OrderByDescending(review => review.CreatedDate)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<Review>> GetAllAsync()
        {
            return await BuildReviewQuery()
                .OrderByDescending(review => review.CreatedDate)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int reviewId)
        {
            return await BuildReviewQuery()
                .FirstOrDefaultAsync(
                    review =>
                        review.ReviewId == reviewId);
        }

        public async Task<bool> DeleteAsync(int reviewId)
        {
            var review =
                await _context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                return false;
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return true;
        }

        private IQueryable<Review> BuildReviewQuery()
        {
            return _context.Reviews
                .AsNoTracking()
                .Include(review => review.Branch);
        }
    }
}