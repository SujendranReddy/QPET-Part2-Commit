using Microsoft.EntityFrameworkCore;
using QPET.Application.Interfaces;
using QPET.Data;
using QPET.Domain.Entities;

namespace QPET.Infrastructure.Repositories
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly ApplicationDbContext _context;

        public EnquiryRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Enquiry> AddAsync(
            Enquiry enquiry)
        {
            _context.Enquiries.Add(enquiry);
            await _context.SaveChangesAsync();

            return enquiry;
        }

        public async Task<List<Enquiry>> GetAllAsync(
            EnquiryStatus? status = null)
        {
            var query =
                BuildEnquiryQuery()
                    .AsNoTracking();

            if (status.HasValue)
            {
                query =
                    query.Where(
                        enquiry =>
                            enquiry.Status ==
                            status.Value);
            }

            return await query
                .OrderByDescending(
                    enquiry =>
                        enquiry.CreatedDate)
                .ToListAsync();
        }

        public async Task<Enquiry?> GetByIdAsync(
            int enquiryId)
        {
            return await BuildEnquiryQuery()
                .FirstOrDefaultAsync(
                    enquiry =>
                        enquiry.EnquiryId ==
                        enquiryId);
        }

        public async Task UpdateAsync(
            Enquiry enquiry)
        {
            _context.Enquiries.Update(enquiry);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Enquiry> BuildEnquiryQuery()
        {
            return _context.Enquiries
                .Include(enquiry => enquiry.Customer)
                .Include(enquiry => enquiry.Branch)
                .Include(enquiry => enquiry.Attachments);
        }
    }
}