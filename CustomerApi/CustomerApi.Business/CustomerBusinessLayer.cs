using CustomerApi.Abstractions.Interfaces;
using CustomerApi.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Business
{
    public class CustomerBusinessLayer
    {
        private readonly CustomerMapper _customerMapper;
        private readonly ICustomerStorageProvider _storageProvider;

        public CustomerBusinessLayer(ICustomerStorageProvider storageProvider,
            CustomerMapper customerMapper
            )
        {
            _storageProvider = storageProvider;
            _customerMapper = customerMapper;
        }

        /// <summary>
        /// Add the given <see cref="CustomerModel"/> to the data store.
        /// </summary>
        /// <param name="customer">The model for the customer to add to the data store.</param>
        /// <returns></returns>
        public async Task AddCustomerAsync(CustomerModel customer)
        {
            CustomerDto customerDto = _customerMapper.Map(customer);
            customerDto.LastUpdatedDate = DateTime.UtcNow;

            // Look up if the id exists for a record already, if one was set in the model.
            if (!customerDto.CustomerId.Equals(Guid.Empty))
            {
                CustomerDto existingCustomer = await _storageProvider.GetCustomerAsync(customerDto.CustomerId);

                if (existingCustomer != null)
                {
                    throw new ArgumentException($"Existing customer for customterId {customerDto.CustomerId}, do not specify customerId if you want one to be generated for you.");
                }
            }

            await _storageProvider.AddCustomerAsync(customerDto);
        }

        /// <summary>
        /// Updates the customer record for the customer with the given customerId.
        /// </summary>
        /// <param name="customerId">The Id of the customer to update.</param>
        /// <param name="customer">The model to update the customer with.</param>
        /// <returns></returns>
        public async Task UpdateCustomerAsync(Guid customerId, CustomerModel customer)
        {
            // We want to check if the item exists, 
            // or there will an exception thrown when trying to update a record doesn't exist.
            // This is here as controlling the action to take on a missing record is a business logic decision.
            CustomerDto foundCustomer = await _storageProvider.GetCustomerAsync(customerId);

            if (foundCustomer == null)
            {
                throw new ArgumentException($"Supplied Customer does not exist for Id {customer.CustomerId}. Perhaps you wanted to call Add Customer instead?");
            }

            // Ideally we would do this another way, but there is only a few fields to map, so this is faster.
            foundCustomer.FirstName = customer.FirstName;
            foundCustomer.LastName = customer.LastName;
            foundCustomer.DateOfBirth = customer.DateOfBirth;
            foundCustomer.LastUpdatedDate = DateTime.UtcNow;

            await _storageProvider.UpdateCustomerAsync(foundCustomer);
        }

        /// <summary>
        /// Marks the customer with the given Id as deleted.
        /// </summary>
        /// <param name="customerId">The Id of the customer to mark as deleted.</param>
        /// <returns></returns>
        public async Task DeleteCustomerAsync(Guid customerId)
        {
            CustomerDto customerDto = await _storageProvider.GetCustomerAsync(customerId);

            if (customerDto != null)
            {
                customerDto.LastUpdatedDate = DateTime.UtcNow;
                customerDto.IsDeleted = true;
                await _storageProvider.DeleteCustomerAsync(customerDto);
            }
        }

        /// <summary>
        /// Gets the customer with the given Id.
        /// </summary>
        /// <param name="customerId">The Id of the customer to get</param>
        /// <returns><see cref="Task{CustomerModel}"/></returns>
        public async Task<CustomerModel> GetCustomerAsync(Guid customerId)
        {
            CustomerModel customerModel = null;

            CustomerDto customerDto = await _storageProvider.GetCustomerAsync(customerId);

            if (customerDto != null)
            {
                customerModel = _customerMapper.Map(customerDto);
            }

            return customerModel;
        }

        /// <summary>
        /// Search for customers using the supplied search phrase.
        /// </summary>
        /// <param name="searchPhrase">The search term.</param>
        /// <returns><see cref="Task{IEnumerable{CustomerModel}}"/></returns>
        public async Task<IEnumerable<CustomerModel>> SearchCustomersAsync(string searchPhrase)
        {
            IEnumerable<CustomerModel> customerModels = Enumerable.Empty<CustomerModel>();
            
            IEnumerable<CustomerDto> customerDtos = await _storageProvider.SearchCustomersAsync(searchPhrase);

            if (customerDtos.Any())
            {
                customerModels = _customerMapper.Map(customerDtos);
            }

            return customerModels;
        }

        /// <summary>
        /// Gets all customers that haven't been marked as deleted.
        /// </summary>
        /// <returns><see cref="Task{IEnumerable{CustomerModel}}"/></returns>
        public async Task<IEnumerable<CustomerModel>> GetAllCustomersAsync()
        {
            IEnumerable<CustomerModel> customerModels = Enumerable.Empty<CustomerModel>();
            
            IEnumerable<CustomerDto> customerDtos = await _storageProvider.GetAllCustomersAsync();

            if (customerDtos.Any())
            {
                customerModels = _customerMapper.Map(customerDtos);
            }

            return customerModels;
        }
    }
}
