using QPET.Domain.Entities;

namespace QPET.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByEmailAsync(
            string emailAddress);

        Task<Customer> AddAsync(Customer customer);

        Task UpdateAsync(Customer customer);
    }
}