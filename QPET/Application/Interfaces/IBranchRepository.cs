using QPET.Domain.Entities;

namespace QPET.Application.Interfaces
{
    public interface IBranchRepository
    {
        Task<List<Branch>> GetAllAsync();

        Task<Branch?> GetByIdAsync(int branchId);
    }
}