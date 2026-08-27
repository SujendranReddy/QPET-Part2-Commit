using QPET.Application.Interfaces;
using QPET.Domain.Entities;

namespace QPET.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(
            IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public Task<List<Branch>> GetAllAsync()
        {
            return _branchRepository.GetAllAsync();
        }

        public Task<Branch?> GetByIdAsync(int branchId)
        {
            return _branchRepository.GetByIdAsync(branchId);
        }
    }
}