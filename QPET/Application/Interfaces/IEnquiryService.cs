using QPET.Application.DTOs;
using QPET.Domain.Entities;

namespace QPET.Application.Interfaces
{
    public interface IEnquiryService
    {
        Task<int> SubmitAsync(
            CreateEnquiryRequest request);

        Task<List<Enquiry>> GetAllAsync(
            EnquiryStatus? status = null);

        Task<Enquiry?> GetByIdAsync(int enquiryId);

        Task<bool> UpdateStatusAsync(
            int enquiryId,
            EnquiryStatus status);
    }
}