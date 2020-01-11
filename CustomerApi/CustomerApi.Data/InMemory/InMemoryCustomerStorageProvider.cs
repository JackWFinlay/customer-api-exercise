using CustomerApi.Abstractions.Interfaces;
using CustomerApi.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data.InMemory
{
    public class InMemoryCustomerStorageProvider : ICustomerStorageProvider
    {
        private readonly CustomerContext _context;

        public InMemoryCustomerStorageProvider(CustomerContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(CustomerDto customer)
        {

            _context.Add(customer);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(CustomerDto customer)
        {
            _context.Remove(customer);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
        {
            IEnumerable<CustomerDto> customers = await _context.Customers
                                                            .ToListAsync();

            return customers;
        }

        public async Task<CustomerDto> GetCustomer(Guid customerId)
        {
            CustomerDto customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == customerId);

            return customer;
        }

        public async Task<IEnumerable<CustomerDto>> SearchCustomers(string searchPhrase)
        {
            IEnumerable<CustomerDto> customers = await _context.Customers
                                                     .Where(c => c.FirstName.Contains(searchPhrase) 
                                                        || c.LastName.Contains(searchPhrase)
                                                        || ($"{c.FirstName} {c.LastName}").Contains(searchPhrase))
                                                     .ToListAsync();


            return customers;
        }

        public async Task UpdateCustomerAsync(CustomerDto customer)
        {
            _context.Update(customer);

            await _context.SaveChangesAsync();
        }
    }
}
