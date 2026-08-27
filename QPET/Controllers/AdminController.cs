using Microsoft.AspNetCore.Authorization;
using QPET.Application.DTOs;
using QPET.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using DomainEnquiry = QPET.Domain.Entities.Enquiry;
using EnquiryStatus = QPET.Domain.Entities.EnquiryStatus;

namespace QPET.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IEnquiryService _enquiryService;
        private readonly IProductService _productService;
        private readonly UserManager<AdminUser> _userManager;

        private readonly SignInManager<AdminUser> _signInManager;

        public AdminController(

    IFileStorageService fileStorageService,
    IReviewService reviewService,
    IEnquiryService enquiryService,
    UserManager<AdminUser> userManager,
    IProductService productService,
    SignInManager<AdminUser> signInManager)
        {
            _reviewService = reviewService;
            _fileStorageService = fileStorageService;
            _enquiryService = enquiryService;
            _productService = productService;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(
    string? returnUrl = null)
        {
            if (
                User.Identity?.IsAuthenticated
                == true)
            {
                return RedirectToAction(
                    nameof(Dashboard)
                );
            }


            return View(
                new AdminLoginViewModel
                {
                    ReturnUrl =
                        returnUrl
                }
            );
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
     AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var admin =
                await _userManager.FindByEmailAsync(
                    model.EmailAddress.Trim());

            if (admin == null || !admin.IsActive)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email address or password.");

                return View(model);
            }

            var result =
                await _signInManager.PasswordSignInAsync(
                    admin,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) &&
                    Url.IsLocalUrl(model.ReturnUrl))
                {
                    return LocalRedirect(model.ReturnUrl);
                }

                return RedirectToAction(nameof(Dashboard));
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This account is temporarily locked. Please try again later.");

                return View(model);
            }

            ModelState.AddModelError(
                string.Empty,
                "Invalid email address or password.");

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Dashboard()
        {
            var products =
                await _productService.GetAllAsync();

            var enquiries =
                await _enquiryService.GetAllAsync();

            var reviews =
                await _reviewService.GetAllAsync();

            var recentEnquiries =
                enquiries
                    .Take(5)
                    .Select(
                        enquiry =>
                        {
                            return new
                                AdminDashboardEnquiryViewModel
                            {
                                EnquiryId =
                                    enquiry.EnquiryId,

                                CustomerName =
                                    enquiry.Customer?.FullName
                                    ?? "Unknown customer",

                                CustomerEmail =
                                    enquiry.Customer?.EmailAddress
                                    ?? "Email not available",

                                BranchName =
                                    enquiry.Branch?.BranchName
                                    ?? "Unknown branch",

                                Subject =
                                    enquiry.Subject,

                                CreatedDate =
                                    enquiry.CreatedDate.ToString(
                                        "dd MMM yyyy"),

                                Status =
                                    enquiry.Status ==
                                        EnquiryStatus.InProgress
                                            ? "In Progress"
                                            : enquiry.Status.ToString()
                            };
                        }
                    )
                    .ToList();


            var model =
                new AdminDashboardViewModel
                {
                    ActiveProductCount =
                        products.Count(
                            product =>
                                product.IsActive
                        ),

                    NewEnquiryCount =
                        enquiries.Count(
                            enquiry =>
                                enquiry.Status ==
                                EnquiryStatus.New
                        ),

                    OpenEnquiryCount =
                        enquiries.Count(
                            enquiry =>
                                enquiry.Status ==
                                    EnquiryStatus.New ||
                                enquiry.Status ==
                                    EnquiryStatus.InProgress
                        ),

                    ReviewCount =
                        reviews.Count,

                    RecentEnquiries =
                        recentEnquiries
                };


            return View(model);
        }


        public async Task<IActionResult> Enquiries(
            EnquiryStatus? status = null)
        {
            var model =
                new AdminEnquiryListViewModel
                {
                    Enquiries =
                        await _enquiryService.GetAllAsync(
                            status),

                    SelectedStatus =
                        status
                };


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EnquiryDetails(int id)
        {
            var enquiry =
                await _enquiryService.GetByIdAsync(id);


            if (enquiry == null)
            {
                ViewBag.EnquiryNotFound =
                    true;

                return View(
                    new DomainEnquiry());
            }
            return View(enquiry);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadEnquiryAttachment(
            int enquiryId,
            int attachmentId)
        {
            var enquiry =
                await _enquiryService.GetByIdAsync(enquiryId);

            if (enquiry == null)
            {
                return NotFound();
            }

            var attachment =
                enquiry.Attachments.FirstOrDefault(
                    item =>
                        item.AttachmentId == attachmentId);

            if (attachment == null)
            {
                return NotFound();
            }

            var stream =
                await _fileStorageService.OpenReadAsync(
                    attachment.FilePath);

            if (stream == null)
            {
                return NotFound();
            }

            return File(
                stream,
                attachment.ContentType,
                attachment.OriginalFileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEnquiryStatus(
            int id,
            EnquiryStatus status)
        {
            var updated =
                await _enquiryService.UpdateStatusAsync(
                    id,
                    status);


            TempData["EnquiryMessage"] =
                updated
                    ? "Enquiry status updated successfully."
                    : "The enquiry status could not be updated.";


            return RedirectToAction(
                nameof(EnquiryDetails),
                new
                {
                    id
                }
            );
        }


        public async Task<IActionResult> Products()
        {
            var model =
                new AdminProductCatalogueViewModel
                {
                    Products =
                        await _productService.GetAllAsync(),

                    Categories =
                        await _productService.GetCategoriesAsync(),

                    SubCategories =
                        await _productService.GetSubCategoriesAsync()
                };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            await PopulateDatabaseProductLookupsAsync();

            return View(
                new AdminProductViewModel
                {
                    IsActive = true
                });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(
    AdminProductViewModel model)
        {
            ValidateProductImage(model);

            if (!ModelState.IsValid)
            {
                await PopulateDatabaseProductLookupsAsync();

                return View(model);
            }

            using var imageStream =
                model.Image?.OpenReadStream();

            var images =
                model.Image != null &&
                model.Image.Length > 0 &&
                imageStream != null
                    ? new List<UploadedFile>
                    {
                new UploadedFile
                {
                    OriginalFileName =
                        model.Image.FileName,

                    ContentType =
                        model.Image.ContentType,

                    Length =
                        model.Image.Length,

                    Content =
                        imageStream
                }
                    }
                    : new List<UploadedFile>();

            var request =
                new SaveProductRequest
                {
                    ProductName =
                        model.ProductName,

                    CategoryId =
                        model.CategoryId,

                    SubCategoryId =
                        model.SubCategoryId!.Value,

                    NeckFinish =
                        model.NeckFinish,

                    CapacityVolume =
                        model.CapacityVolume,

                    Description =
                        model.Description,

                    Images =
                        images,

                    IsActive =
                        model.IsActive
                };

            try
            {
                var productId =
                    await _productService.CreateAsync(request);

                TempData["ProductMessage"] =
                    $"Product created successfully. Product ID: {productId}";

                return RedirectToAction(nameof(Products));
            }
            catch (ArgumentException exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    exception.Message);

                await PopulateDatabaseProductLookupsAsync();

                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product =
                await _productService.GetByIdAsync(id);

            if (product == null)
            {
                ViewBag.ProductNotFound = true;

                return View(
                    new AdminProductViewModel());
            }

            await PopulateDatabaseProductLookupsAsync();

            var primaryImage =
                product.ProductImages
                    .FirstOrDefault(image => image.IsPrimary)
                ?? product.ProductImages.FirstOrDefault();

            var model =
                new AdminProductViewModel
                {
                    ProductId = product.ProductId,
                    CategoryId = product.CategoryId,
                    SubCategoryId = product.SubCategoryId,
                    ProductName = product.ProductName,
                    NeckFinish = product.NeckFinish,
                    CapacityVolume = product.CapacityVolume,
                    Description = product.Description,
                    IsActive = product.IsActive,
                    ExistingImageUrl = primaryImage?.ImageUrl
                };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(
            int id,
            AdminProductViewModel model)
        {
            var existingProduct =
                await _productService.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            model.ProductId = id;
            ValidateProductImage(model);

            if (!ModelState.IsValid)
            {
                await PopulateDatabaseProductLookupsAsync();

                model.ExistingImageUrl =
                    existingProduct.ProductImages
                        .FirstOrDefault(image => image.IsPrimary)
                        ?.ImageUrl;

                return View(model);
            }

            using var imageStream =
                model.Image?.OpenReadStream();

            var images =
                model.Image != null &&
                model.Image.Length > 0 &&
                imageStream != null
                    ? new List<UploadedFile>
                    {
                        new UploadedFile
                        {
                            OriginalFileName = model.Image.FileName,
                            ContentType = model.Image.ContentType,
                            Length = model.Image.Length,
                            Content = imageStream
                        }
                    }
                    : new List<UploadedFile>();

            var request =
                new SaveProductRequest
                {
                    ProductId = id,
                    ProductName = model.ProductName,
                    CategoryId = model.CategoryId,
                    SubCategoryId = model.SubCategoryId!.Value,
                    NeckFinish = model.NeckFinish,
                    CapacityVolume = model.CapacityVolume,
                    Description = model.Description,
                    Images = images,
                    IsActive = model.IsActive
                };

            try
            {
                var updated =
                    await _productService.UpdateAsync(request);

                if (!updated)
                {
                    return NotFound();
                }

                TempData["ProductMessage"] =
                    "Product updated successfully.";

                return RedirectToAction(nameof(Products));
            }
            catch (ArgumentException exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    exception.Message);

                await PopulateDatabaseProductLookupsAsync();

                model.ExistingImageUrl =
                    existingProduct.ProductImages
                        .FirstOrDefault(image => image.IsPrimary)
                        ?.ImageUrl;

                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted =
                await _productService.DeleteAsync(id);

            TempData["ProductMessage"] =
                deleted
                    ? "Product deleted successfully."
                    : "The product could not be found.";

            return RedirectToAction(nameof(Products));
        }


        public async Task<IActionResult> Reviews()
        {
            var reviews =
                await _reviewService.GetAllAsync();

            return View(reviews);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var deleted =
                await _reviewService.DeleteAsync(id);

            TempData["ReviewMessage"] =
                deleted
                    ? "Review deleted successfully."
                    : "The review could not be found.";

            return RedirectToAction(nameof(Reviews));
        }


        private async Task PopulateDatabaseProductLookupsAsync()
        {
            ViewBag.Categories =
                await _productService.GetCategoriesAsync();

            ViewBag.SubCategories =
                await _productService.GetSubCategoriesAsync();
        }


        private void ValidateProductImage(
            AdminProductViewModel model)
        {
            if (model.Image == null)
            {
                return;
            }


            var extension =
                Path.GetExtension(
                    model.Image.FileName
                )
                .ToLowerInvariant();


            var allowedExtensions =
                new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };


            if (
                !allowedExtensions.Contains(
                    extension
                ))
            {
                ModelState.AddModelError(
                    nameof(model.Image),
                    "Only JPG, JPEG, PNG and WEBP images are allowed."
                );
            }
        }
    }
}
