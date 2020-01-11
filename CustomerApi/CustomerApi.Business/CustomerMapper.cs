using CustomerApi.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CustomerApi.Business
{
    public class CustomerMapper
    {
        public CustomerDto Map(CustomerModel customerModel)
        {
            CustomerDto customerDto = new CustomerDto()
            {
                CustomerId = customerModel.CustomerId,
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                DateOfBirth = customerModel.DateOfBirth,
                LastUpdatedDate = DateTime.UtcNow
            };

            return customerDto;
        }

        public IEnumerable<CustomerDto> Map(IEnumerable<CustomerModel> customerModels)
        {
            IEnumerable<CustomerDto> customerDtos = customerModels.Select(c => Map(c));

            return customerDtos;
        }

        public CustomerModel Map(CustomerDto customerDto)
        {
            CustomerModel customerModel = new CustomerModel()
            {
                CustomerId = customerDto.CustomerId,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                DateOfBirth = customerDto.DateOfBirth
            };

            return customerModel;
        }

        public IEnumerable<CustomerModel> Map(IEnumerable<CustomerDto> customerDtos)
        {
            IEnumerable<CustomerModel> customerModels = customerDtos.Select(c => Map(c));

            return customerModels;
        }
    }
}
