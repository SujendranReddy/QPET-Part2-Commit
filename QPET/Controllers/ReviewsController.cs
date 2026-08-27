using Microsoft.AspNetCore.Mvc;
using QPET.Application.DTOs;
using QPET.Application.Interfaces;
using QPET.Models;

namespace QPET.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IBranchService _branchService;

        public ReviewsController(
            IReviewService reviewService,
            IBranchService branchService)
        {
            _reviewService = reviewService;
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(
                await BuildPageModelAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(
            ReviewsPageViewModel model)
        {
            model.Review ??=
                new ReviewViewModel();

            if (!ModelState.IsValid)
            {
                model.Reviews =
                    await _reviewService
                        .GetForWebsiteAsync(10);

                model.Branches =
                    await _branchService
                        .GetAllAsync();

                return View(
                    "Index",
                    model);
            }

            try
            {
                var request =
                    new CreateReviewRequest
                    {
                        BranchId =
                            model.Review.BranchId,

                        DisplayName =
                            model.Review.DisplayName,

                        EmailAddress =
                            model.Review.EmailAddress,

                        Rating =
                            model.Review.Rating,

                        ReviewMessage =
                            model.Review.ReviewMessage
                    };

                await _reviewService.SubmitAsync(request);

                TempData["ReviewMessage"] =
                    "Thank you. Your review has been submitted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    exception.Message);

                model.Reviews =
                    await _reviewService
                        .GetForWebsiteAsync(10);

                model.Branches =
                    await _branchService
                        .GetAllAsync();

                return View(
                    "Index",
                    model);
            }
        }

        private async Task<ReviewsPageViewModel>
            BuildPageModelAsync()
        {
            return new ReviewsPageViewModel
            {
                Reviews =
                    await _reviewService
                        .GetForWebsiteAsync(10),

                Branches =
                    await _branchService
                        .GetAllAsync(),

                Review =
                    new ReviewViewModel()
            };
        }
    }
}