using CustomerApi.Abstractions.Models;
using CustomerApi.Business;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.Test
{
    public class CustomerBusinessLayerTests : IClassFixture<CustomerBusinessLayerTestFixture>
    {
        private readonly CustomerBusinessLayerTestFixture _fixture;

        public CustomerBusinessLayerTests(CustomerBusinessLayerTestFixture testFixture)
        {
            _fixture = testFixture;
        }

        [Fact]
        public void AddCustomerAsync_AddNewCustomer_AddsCustomer()
        {
            CustomerBusinessLayer businessLayer = _fixture.CustomerBusinessLayer;
            CustomerModel customerModel = CustomerBusinessLayerTestFixture.InputCustomerModel;

            Func<Task> test = async () => await businessLayer.AddCustomerAsync(customerModel);

            test.Should()
                .NotThrow();
        }

        [Fact]
        public void AddCustomerAsync_AddExistingCustomer_ThrowsArgumentException()
        {
            CustomerBusinessLayer businessLayer = _fixture.CustomerBusinessLayer;
            CustomerModel customerModel = CustomerBusinessLayerTestFixture.CustomerModelWithId;

            Func<Task> test = async () => await businessLayer.AddCustomerAsync(customerModel);

            test.Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void UpdateCustomerAsync_UpdateCustomer_UpdatesCustomer()
        {
            CustomerBusinessLayer businessLayer = _fixture.CustomerBusinessLayer;
            CustomerModel customerModel = CustomerBusinessLayerTestFixture.InputCustomerModel;
            Guid id = new Guid(CustomerBusinessLayerTestFixture.CustomerGuid);

            Func<Task> test = async () => await businessLayer.UpdateCustomerAsync(id, customerModel);

            test.Should()
                .NotThrow();
        }

        [Fact]
        public void GetAllCustomersAsync_GetAllCustomers_GetsAllCustomersAsync()
        {
            CustomerBusinessLayer businessLayer = _fixture.CustomerBusinessLayer;
            IEnumerable<CustomerModel> customerModels = _fixture.CustomerModels;
            IEnumerable<CustomerModel> customerModelsResult = Enumerable.Empty<CustomerModel>();

            Func<Task> test = async () => 
            {
                customerModelsResult = await businessLayer.GetAllCustomersAsync();
            };

            test.Should()
                .NotThrow();

            customerModelsResult.Should()
                .NotBeNullOrEmpty()
                .And
                .BeEquivalentTo(customerModels);

        }

        [Fact]
        public void GetCustomerAsync_GetExistingCustomer_GetsExistingCustomer()
        {
            CustomerBusinessLayer businessLayer = _fixture.CustomerBusinessLayer;
            Guid id = new Guid(CustomerBusinessLayerTestFixture.CustomerGuid);
            CustomerModel customerModelExpected = CustomerBusinessLayerTestFixture.CustomerModelWithId;
            CustomerModel customerModelResult = null;

            Func<Task> test = async () =>
            {
                customerModelResult = await businessLayer.GetCustomerAsync(id);
            };

            test.Should()
                .NotThrow();

            customerModelResult.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(customerModelExpected);
        }

        [Fact]
        public void GetCustomerAsync_CustomerNotExistant_ReturnsNull()
        {
            CustomerBusinessLayer businessLayer = _fixture.CustomerBusinessLayer;
            // Random Guid, so we know it's (probably) not there.
            Guid id = Guid.NewGuid();
            CustomerModel customerModelExpected = CustomerBusinessLayerTestFixture.CustomerModelWithId;
            CustomerModel customerModelResult = null;

            Func<Task> test = async () =>
            {
                customerModelResult = await businessLayer.GetCustomerAsync(id);
            };

            test.Should()
                .NotThrow();

            customerModelResult.Should()
                .BeNull();
        }
    }
}
