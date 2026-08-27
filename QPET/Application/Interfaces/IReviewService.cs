using QPET.Application.DTOs;
using QPET.Domain.Entities;

namespace QPET.Application.Interfaces
{
    public interface IReviewService
    {
        Task<int> SubmitAsync(
            CreateReviewRequest request);

        Task<List<Review>> GetForWebsiteAsync(
            int limit);

        Task<List<Review>> GetAllAsync();

        Task<bool> DeleteAsync(int reviewId);
    }
}