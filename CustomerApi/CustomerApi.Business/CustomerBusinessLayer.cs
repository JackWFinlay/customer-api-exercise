using CustomerApi.Abstractions.Interfaces;
using CustomerApi.Abstractions.Models;
using System;
using System.Collections.Generic;
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

        public async Task AddCustomerAsync(CustomerModel customer)
        {
            CustomerDto customerDto = _customerMapper.Map(customer);

            await _storageProvider.AddCustomerAsync(customerDto);
        }

        public async Task UpdateCustomerAsync(Guid customerId, CustomerModel customer)
        {
            CustomerDto customerDto = _customerMapper.Map(customer);
            customerDto.CustomerId = customerId;

            await _storageProvider.UpdateCustomerAsync(customerDto);
        }

        public async Task DeleteCustomerAsync(Guid customerId)
        {
            CustomerDto customerDto = await _storageProvider.GetCustomer(customerId);

            await _storageProvider.DeleteCustomerAsync(customerDto);
        }

        public async Task<CustomerModel> GetCustomerAsync(Guid customerId)
        {
            CustomerDto customerDto = await _storageProvider.GetCustomer(customerId);
            CustomerModel customerModel = _customerMapper.Map(customerDto);

            return customerModel;
        }

        public async Task<IEnumerable<CustomerModel>> SearchCustomersAsync(string searchPhrase)
        {
            IEnumerable<CustomerDto> customerDtos = await _storageProvider.SearchCustomers(searchPhrase);
            IEnumerable<CustomerModel> customerModels = _customerMapper.Map(customerDtos);

            return customerModels;
        }

        public async Task<IEnumerable<CustomerModel>> GetAllCustomersAsync()
        {
            IEnumerable<CustomerDto> customerDtos = await _storageProvider.GetAllCustomers();
            IEnumerable<CustomerModel> customerModels = _customerMapper.Map(customerDtos);

            return customerModels;
        }
    }
}
