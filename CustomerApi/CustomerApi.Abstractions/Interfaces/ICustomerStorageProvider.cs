using CustomerApi.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApi.Abstractions.Interfaces
{
    public interface ICustomerStorageProvider
    {
        Task AddCustomerAsync(CustomerDto customer);
        Task UpdateCustomerAsync(CustomerDto customer);
        Task DeleteCustomerAsync(CustomerDto customer);
        Task<CustomerDto> GetCustomer(Guid customerId);
        Task<IEnumerable<CustomerDto>> SearchCustomers(string searchPhrase);
        Task<IEnumerable<CustomerDto>> GetAllCustomers();
    }
}
