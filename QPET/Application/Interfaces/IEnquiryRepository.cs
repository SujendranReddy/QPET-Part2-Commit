using QPET.Domain.Entities;

namespace QPET.Application.Interfaces
{
    public interface IEnquiryRepository
    {
        Task<Enquiry> AddAsync(Enquiry enquiry);

        Task<List<Enquiry>> GetAllAsync(
            EnquiryStatus? status = null);

        Task<Enquiry?> GetByIdAsync(int enquiryId);

        Task UpdateAsync(Enquiry enquiry);
    }
}