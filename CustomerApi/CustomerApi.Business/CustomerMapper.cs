using CustomerApi.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CustomerApi.Business
{
    public class CustomerMapper
    {
        /// <summary>
        /// Maps a single <see cref="CustomerModel"/> to an instance of <see cref="CustomerDto"/>
        /// </summary>
        /// <param name="customerModel">The <see cref="CustomerModel"/> to map.</param>
        /// <returns><see cref="CustomerDto"/></returns>
        public CustomerDto Map(CustomerModel customerModel)
        {
            CustomerDto customerDto = new CustomerDto()
            {
                CustomerId = customerModel.CustomerId,
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                DateOfBirth = customerModel.DateOfBirth
            };

            return customerDto;
        }

        /// <summary>
        /// Maps an <see cref="IEnumerable{CustomerModel}"/> to an <see cref="IEnumerable{CustomerDto}"/>
        /// </summary>
        /// <param name="customerModels">The <see cref="IEnumerable{CustomerModel}"/> to map.</param>
        /// <returns><see cref="IEnumerable{CustomerDto}"/></returns>
        public IEnumerable<CustomerDto> Map(IEnumerable<CustomerModel> customerModels)
        {
            IEnumerable<CustomerDto> customerDtos = customerModels.Select(c => Map(c));

            return customerDtos;
        }

        /// <summary>
        /// Maps a single <see cref="CustomerDto"/> to a <see cref="CustomerModel"/>
        /// </summary>
        /// <param name="customerDto">The <see cref="CustomerDto"/> to map.</param>
        /// <returns><see cref="CustomerModel"/></returns>
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

        /// <summary>
        /// Maps an <see cref="IEnumerable{CustomerDto}"/> to an <see cref="IEnumerable{CustomerModel}"/>
        /// </summary>
        /// <param name="customerDtos">The <see cref="IEnumerable{CustomerDto}"/> to map.</param>
        /// <returns><see cref="IEnumerable{CustomerModel}"/></returns>
        public IEnumerable<CustomerModel> Map(IEnumerable<CustomerDto> customerDtos)
        {
            IEnumerable<CustomerModel> customerModels = customerDtos.Select(c => Map(c));

            return customerModels;
        }
    }
}
