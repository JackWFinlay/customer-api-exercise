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

        /// <summary>
        /// Add a <see cref="CustomerDto"/> record to the database.
        /// </summary>
        /// <param name="customer">The <see cref="CustomerDto"/> record to store.</param>
        /// <returns></returns>
        public async Task AddCustomerAsync(CustomerDto customer)
        {
            _context.Add(customer);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Marks the given customer as deleted.
        /// </summary>
        /// <param name="customer">The <see cref="CustomerDto"/> record for the given customer.</param>
        /// <returns></returns>
        public async Task DeleteCustomerAsync(CustomerDto customer)
        {
            _context.Update(customer);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Returns all customers.
        /// </summary>
        /// <returns><see cref="Task{IEnumerable{CustomerDto}}"/></returns>
        public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
        {
            IEnumerable<CustomerDto> customers = await _context.Customers
                                                            .Where(c => !c.IsDeleted)
                                                            .ToListAsync();

            return customers;
        }

        /// <summary>
        /// Returns specified customer if they exist.
        /// </summary>
        /// <param name="customerId">Id of the customer to look for.</param>
        /// <returns><see cref="Task{CustomerDto}"/></returns>
        public async Task<CustomerDto> GetCustomer(Guid customerId)
        {
            CustomerDto customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == customerId && !c.IsDeleted);

            return customer;
        }

        /// <summary>
        /// Searches for customers with the given search phrase.
        /// </summary>
        /// <param name="searchPhrase">The search terms.</param>
        /// <returns><see cref="Task{IEnumerable{CustomerDto}}"/></returns>
        public async Task<IEnumerable<CustomerDto>> SearchCustomers(string searchPhrase)
        {
            IEnumerable<CustomerDto> customers = await _context.Customers
                                                     .Where(c => !c.IsDeleted 
                                                            && (c.FirstName.Contains(searchPhrase) 
                                                                || c.LastName.Contains(searchPhrase)
                                                                || ($"{c.FirstName} {c.LastName}").Contains(searchPhrase))
                                                           )
                                                     .ToListAsync();


            return customers;
        }

        /// <summary>
        /// Updates customer if they exist.
        /// </summary>
        /// <param name="customer">The customer to update.</param>
        /// <returns></returns>
        public async Task UpdateCustomerAsync(CustomerDto customer)
        {
            // We want to check if the item exists, 
            // or there will an exception thrown when trying to update a record doesn't exist.
            CustomerDto foundCustomer = await _context.Customers.FindAsync(customer.CustomerId);

            if (foundCustomer != null)
            { 
                _context.Update(customer);
                await _context.SaveChangesAsync();
            }       
        }
    }
}
