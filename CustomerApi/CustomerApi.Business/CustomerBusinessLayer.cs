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
            CustomerDto customerDto = _customerMapper.Map(customer);

            customerDto.CustomerId = customerId;

            await _storageProvider.UpdateCustomerAsync(customerDto);
        }

        /// <summary>
        /// Marks the customer with the given Id as deleted.
        /// </summary>
        /// <param name="customerId">The Id of the customer to mark as deleted.</param>
        /// <returns></returns>
        public async Task DeleteCustomerAsync(Guid customerId)
        {
            CustomerDto customerDto = await _storageProvider.GetCustomer(customerId);

            if (customerDto != null)
            {
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

            CustomerDto customerDto = await _storageProvider.GetCustomer(customerId);

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
            
            IEnumerable<CustomerDto> customerDtos = await _storageProvider.SearchCustomers(searchPhrase);

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
            
            IEnumerable<CustomerDto> customerDtos = await _storageProvider.GetAllCustomers();

            if (customerDtos.Any())
            {
                customerModels = _customerMapper.Map(customerDtos);
            }

            return customerModels;
        }
    }
}
