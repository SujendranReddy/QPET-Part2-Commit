using Microsoft.EntityFrameworkCore;
using QPET.Application.Interfaces;
using QPET.Data;
using QPET.Domain.Entities;

namespace QPET.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByEmailAsync(
            string emailAddress)
        {
            var normalizedEmail =
                emailAddress.Trim().ToLower();

            return await _context.Customers
                .FirstOrDefaultAsync(
                    customer =>
                        customer.EmailAddress.ToLower() ==
                        normalizedEmail);
        }

        public async Task<Customer> AddAsync(
            Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task UpdateAsync(
            Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}