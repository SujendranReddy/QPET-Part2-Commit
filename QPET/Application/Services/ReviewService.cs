using QPET.Application.DTOs;
using QPET.Application.Interfaces;
using QPET.Domain.Entities;

namespace QPET.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBranchRepository _branchRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            IBranchRepository branchRepository)
        {
            _reviewRepository = reviewRepository;
            _branchRepository = branchRepository;
        }

        public async Task<int> SubmitAsync(
            CreateReviewRequest request)
        {
            var branch =
                await _branchRepository.GetByIdAsync(
                    request.BranchId);

            if (branch == null)
            {
                throw new ArgumentException(
                    "The selected branch is invalid.");
            }

            if (request.Rating < 1 ||
                request.Rating > 5)
            {
                throw new ArgumentException(
                    "The rating must be between one and five.");
            }

            if (string.IsNullOrWhiteSpace(
                    request.DisplayName))
            {
                throw new ArgumentException(
                    "Enter a display name.");
            }

            if (string.IsNullOrWhiteSpace(
                    request.ReviewMessage))
            {
                throw new ArgumentException(
                    "Enter a review message.");
            }

            var review =
                new Review
                {
                    BranchId =
                        request.BranchId,

                    DisplayName =
                        request.DisplayName.Trim(),

                    EmailAddress =
                        string.IsNullOrWhiteSpace(
                            request.EmailAddress)
                            ? null
                            : request.EmailAddress
                                .Trim()
                                .ToLower(),

                    Rating =
                        request.Rating,

                    ReviewMessage =
                        request.ReviewMessage.Trim(),

                    CreatedDate =
                        DateTime.UtcNow
                };

            var createdReview =
                await _reviewRepository.AddAsync(review);

            return createdReview.ReviewId;
        }

        public Task<List<Review>> GetForWebsiteAsync(
            int limit)
        {
            if (limit <= 0)
            {
                limit = 10;
            }

            return _reviewRepository.GetLatestAsync(limit);
        }

        public Task<List<Review>> GetAllAsync()
        {
            return _reviewRepository.GetAllAsync();
        }

        public Task<bool> DeleteAsync(int reviewId)
        {
            return _reviewRepository.DeleteAsync(reviewId);
        }
    }
}