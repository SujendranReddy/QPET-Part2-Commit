using QPET.Domain.Entities;

namespace QPET.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> AddAsync(Review review);

        Task<List<Review>> GetLatestAsync(int limit);

        Task<List<Review>> GetAllAsync();

        Task<Review?> GetByIdAsync(int reviewId);

        Task<bool> DeleteAsync(int reviewId);
    }
}