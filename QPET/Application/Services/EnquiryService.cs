using QPET.Application.DTOs;
using QPET.Application.Interfaces;
using QPET.Domain.Entities;

namespace QPET.Application.Services
{
    public class EnquiryService : IEnquiryService
    {
        private const int MaximumAttachmentCount = 5;

        private readonly ICustomerRepository _customerRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IEnquiryRepository _enquiryRepository;
        private readonly IFileStorageService _fileStorageService;

        public EnquiryService(
            ICustomerRepository customerRepository,
            IBranchRepository branchRepository,
            IEnquiryRepository enquiryRepository,
            IFileStorageService fileStorageService)
        {
            _customerRepository = customerRepository;
            _branchRepository = branchRepository;
            _enquiryRepository = enquiryRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<int> SubmitAsync(
            CreateEnquiryRequest request)
        {
            var branch =
                await _branchRepository.GetByIdAsync(
                    request.BranchId);

            if (branch == null)
            {
                throw new ArgumentException(
                    "The selected branch is invalid.");
            }

            var attachments =
                request.Attachments.ToList();

            if (attachments.Count > MaximumAttachmentCount)
            {
                throw new ArgumentException(
                    "A maximum of five attachments is allowed.");
            }

            foreach (var attachment in attachments)
            {
                if (!_fileStorageService.Validate(attachment))
                {
                    throw new ArgumentException(
                        "One or more attachments are invalid.");
                }
            }

            var customer =
                await _customerRepository.GetByEmailAsync(
                    request.EmailAddress);

            if (customer == null)
            {
                customer =
                    await _customerRepository.AddAsync(
                        new Customer
                        {
                            FullName =
                                request.FullName.Trim(),

                            EmailAddress =
                                request.EmailAddress
                                    .Trim()
                                    .ToLower(),

                            PhoneNumber =
                                request.PhoneNumber.Trim()
                        });
            }
            else
            {
                customer.UpdateContactDetails(
                    request.FullName.Trim(),
                    request.PhoneNumber.Trim());

                await _customerRepository.UpdateAsync(customer);
            }

            var savedPaths =
                new List<string>();

            try
            {
                var enquiry =
                    new Enquiry
                    {
                        CustomerId =
                            customer.CustomerId,

                        BranchId =
                            branch.BranchId,

                        Subject =
                            request.Subject.Trim(),

                        Message =
                            request.Message.Trim(),

                        CreatedDate =
                            DateTime.UtcNow,

                        Status =
                            EnquiryStatus.New
                    };

                foreach (var attachment in attachments)
                {
                    var filePath =
                        await _fileStorageService.SaveAsync(
                            attachment,
                            "enquiries");

                    savedPaths.Add(filePath);

                    enquiry.AddAttachment(
                        new EnquiryAttachment
                        {
                            OriginalFileName =
                                attachment.OriginalFileName,

                            StoredFileName =
                                Path.GetFileName(filePath),

                            FilePath =
                                filePath,

                            ContentType =
                                attachment.ContentType,

                            UploadedDate =
                                DateTime.UtcNow
                        });
                }

                var createdEnquiry =
                    await _enquiryRepository.AddAsync(enquiry);

                return createdEnquiry.EnquiryId;
            }
            catch
            {
                foreach (var path in savedPaths)
                {
                    await _fileStorageService.DeleteAsync(path);
                }

                throw;
            }
        }

        public Task<List<Enquiry>> GetAllAsync(
            EnquiryStatus? status = null)
        {
            return _enquiryRepository.GetAllAsync(status);
        }

        public Task<Enquiry?> GetByIdAsync(int enquiryId)
        {
            return _enquiryRepository.GetByIdAsync(enquiryId);
        }

        public async Task<bool> UpdateStatusAsync(
            int enquiryId,
            EnquiryStatus status)
        {
            var enquiry =
                await _enquiryRepository.GetByIdAsync(
                    enquiryId);

            if (enquiry == null)
            {
                return false;
            }

            enquiry.UpdateStatus(status);

            await _enquiryRepository.UpdateAsync(enquiry);

            return true;
        }
    }
}