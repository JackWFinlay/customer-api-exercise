﻿using CustomerApi.Abstractions.Interfaces;
using CustomerApi.Abstractions.Models;
using CustomerApi.Business;
using CustomerApi.Data.InMemory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApi.Test
{
    public class CustomerBusinessLayerTestFixture
    {
        public Mock<ICustomerStorageProvider> StorageProviderMock;
        public CustomerMapper CustomerMapper = new CustomerMapper();
        
        public CustomerBusinessLayer CustomerBusinessLayer;
        public const string CustomerGuid = "08F40D3F-4E8D-4DC8-982B-1CD7960EFB84";

        public static readonly CustomerModel InputCustomerModel = new CustomerModel()
        {
            DateOfBirth = DateTime.Parse("1993-05-22T12:00:00Z"),
            FirstName = "Jack",
            LastName = "Finlay"
        };

        public readonly IEnumerable<CustomerModel> CustomerModels = new List<CustomerModel>()
        {
            CustomerModelWithId
        };
        
        public static readonly CustomerModel CustomerModelWithId = new CustomerModel()
        {
            CustomerId = new Guid(CustomerGuid),
            DateOfBirth = DateTime.Parse("1993-05-22T12:00:00Z"),
            FirstName = "Jack",
            LastName = "Finlay"
        };

        private readonly Task<IEnumerable<CustomerDto>> _customerDtosTask = Task.FromResult(_customerDtoList);
        private readonly Task<CustomerDto> _customerDtoSingleTask = Task.FromResult(_customerDto);

        private static readonly CustomerDto _customerDto = new CustomerDto()
        {
            CustomerId = new Guid(CustomerGuid),
            DateOfBirth = DateTime.Parse("1993-05-22T12:00:00Z"),
            FirstName = "Jack",
            LastName = "Finlay",
            LastUpdatedDate = DateTime.UtcNow
        };

        private static readonly IEnumerable<CustomerDto> _customerDtoList = new List<CustomerDto>()
        {
            _customerDto
        };

        public CustomerBusinessLayerTestFixture()
        {
            StorageProviderMock = new Mock<ICustomerStorageProvider>();

            StorageProviderMock.Setup(mock => mock.AddCustomerAsync(It.IsAny<CustomerDto>()))
                               .Returns(Task.CompletedTask);

            StorageProviderMock.Setup(mock => mock.DeleteCustomerAsync(It.IsAny<CustomerDto>()))
                               .Returns(Task.CompletedTask);

            StorageProviderMock.Setup(mock => mock.GetAllCustomersAsync())
                               .Returns(_customerDtosTask);

            StorageProviderMock.Setup(mock => mock.GetCustomerAsync(It.Is<Guid>(g => g.Equals(new Guid(CustomerGuid)))))
                               .Returns(_customerDtoSingleTask);
            StorageProviderMock.Setup(mock => mock.GetCustomerAsync(It.Is<Guid>(g => !g.Equals(new Guid(CustomerGuid)))))
                               .Returns(Task.FromResult<CustomerDto>(null));

            StorageProviderMock.Setup(mock => mock.SearchCustomersAsync(It.IsAny<string>()))
                               .Returns(_customerDtosTask);

            StorageProviderMock.Setup(mock => mock.UpdateCustomerAsync(It.IsAny<CustomerDto>()))
                               .Returns(Task.CompletedTask);

            CustomerBusinessLayer = new CustomerBusinessLayer(StorageProviderMock.Object, CustomerMapper);
        }
    }
}
