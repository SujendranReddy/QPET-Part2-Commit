using Microsoft.AspNetCore.Mvc;
using QPET.Application.DTOs;
using QPET.Application.Interfaces;
using QPET.Models;

namespace QPET.Controllers
{
    public class ContactController : Controller
    {
        private const int MaximumAttachmentCount = 5;
        private const long MaximumAttachmentSize =
            10 * 1024 * 1024;

        private readonly IBranchService _branchService;
        private readonly IEnquiryService _enquiryService;

        public ContactController(
            IBranchService branchService,
            IEnquiryService enquiryService)
        {
            _branchService = branchService;
            _enquiryService = enquiryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? product = null)
        {
            await PopulateBranchesAsync();

            var model =
                new EnquiryViewModel();

            if (!string.IsNullOrWhiteSpace(product))
            {
                model.Subject =
                    "Enquiry about " +
                    product;

                model.Message =
                    "I would like more information about " +
                    product +
                    ", including available commercial quantities and product specifications.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitEnquiry(
            EnquiryViewModel model)
        {
            ValidateAttachments(model.Attachments);

            if (!ModelState.IsValid)
            {
                await PopulateBranchesAsync();

                return View(
                    "Index",
                    model);
            }

            var uploadedFiles =
                model.Attachments
                    .Where(file => file.Length > 0)
                    .Select(
                        file =>
                            new UploadedFile
                            {
                                OriginalFileName =
                                    Path.GetFileName(
                                        file.FileName),

                                ContentType =
                                    file.ContentType,

                                Length =
                                    file.Length,

                                Content =
                                    file.OpenReadStream()
                            })
                    .ToList();

            try
            {
                var request =
                    new CreateEnquiryRequest
                    {
                        BranchId =
                            model.BranchId,

                        FullName =
                            model.FullName,

                        PhoneNumber =
                            model.PhoneNumber,

                        EmailAddress =
                            model.EmailAddress,

                        Subject =
                            model.Subject,

                        Message =
                            model.Message,

                        Attachments =
                            uploadedFiles
                    };

                var enquiryId =
                    await _enquiryService.SubmitAsync(
                        request);

                return RedirectToAction(
                    nameof(Confirmation),
                    new
                    {
                        id = enquiryId
                    });
            }
            catch (ArgumentException exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    exception.Message);

                await PopulateBranchesAsync();

                return View(
                    "Index",
                    model);
            }
            finally
            {
                foreach (var file in uploadedFiles)
                {
                    file.Content.Dispose();
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(
            int id)
        {
            var enquiry =
                await _enquiryService.GetByIdAsync(id);

            if (enquiry == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await PopulateBranchesAsync();

            ViewBag.EnquiryReference =
                $"ENQ-{enquiry.EnquiryId}";

            return View(
                "Index",
                new EnquiryViewModel());
        }

        private async Task PopulateBranchesAsync()
        {
            ViewBag.Branches =
                await _branchService.GetAllAsync();
        }

        private void ValidateAttachments(
            IEnumerable<IFormFile> attachments)
        {
            var files =
                attachments
                    .Where(file => file.Length > 0)
                    .ToList();

            if (files.Count > MaximumAttachmentCount)
            {
                ModelState.AddModelError(
                    nameof(EnquiryViewModel.Attachments),
                    "A maximum of five attachments is allowed.");

                return;
            }

            var allowedExtensions =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase)
                {
                    ".pdf",
                    ".jpg",
                    ".jpeg",
                    ".png"
                };

            foreach (var file in files)
            {
                var extension =
                    Path.GetExtension(file.FileName);

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        nameof(EnquiryViewModel.Attachments),
                        "Only PDF, JPG, JPEG and PNG files are allowed.");

                    return;
                }

                if (file.Length > MaximumAttachmentSize)
                {
                    ModelState.AddModelError(
                        nameof(EnquiryViewModel.Attachments),
                        "Each attachment must be 10 MB or smaller.");

                    return;
                }
            }
        }
    }
}