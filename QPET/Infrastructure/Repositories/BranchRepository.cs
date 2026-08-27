using Microsoft.EntityFrameworkCore;
using QPET.Application.Interfaces;
using QPET.Data;
using QPET.Domain.Entities;

namespace QPET.Infrastructure.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;

        public BranchRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Branch>> GetAllAsync()
        {
            return await _context.Branches
                .AsNoTracking()
                .OrderBy(branch => branch.BranchName)
                .ToListAsync();
        }

        public async Task<Branch?> GetByIdAsync(
            int branchId)
        {
            return await _context.Branches
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    branch =>
                        branch.BranchId == branchId);
        }
    }
}
