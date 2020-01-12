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
        Task<CustomerDto> GetCustomerAsync(Guid customerId);
        Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchPhrase);
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    }
}
