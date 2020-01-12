using CustomerApi.Abstractions.Interfaces;
using CustomerApi.Abstractions.Models;
using CustomerApi.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerBusinessLayer _customerBusinessLayer;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(CustomerBusinessLayer customerBusinessLayer, ILogger<CustomersController> logger)
        {
            _customerBusinessLayer = customerBusinessLayer;
            _logger = logger;
        }

        // GET /customers
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<IEnumerable<CustomerModel>>> Get()
        {
            try
            {
                return Ok(await _customerBusinessLayer.GetAllCustomersAsync());
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Exception occured getting customers");
                return BadRequest();
            }

        }

        // GET /customers/0f32959d-0e54-47f2-9da4-d6c0a52ca070
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> Get(Guid id)
        {
            try
            {
                return Ok(await _customerBusinessLayer.GetCustomerAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception occured getting customer {id}");
                return BadRequest();
            }
        }

        // GET /customers/search?searchPhrase=Jack
        [HttpGet]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> Search([FromQuery] string searchPhrase)
        {
            try
            {
                return Ok(await _customerBusinessLayer.SearchCustomersAsync(searchPhrase));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Error occured for search term '{searchPhrase}'");
                return BadRequest();
            }
        }

        // POST /customers
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] CustomerInputModel customer)
        {
            try
            {
                CustomerModel customerModel = (CustomerModel)customer;
                await _customerBusinessLayer.AddCustomerAsync(customerModel);
                return Accepted();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error adding customer");
                return BadRequest();
            }
        }

        // PUT /customers/0F906772-CADA-40A7-BD59-B47DDD1C75B0
        [HttpPut("{customerId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(Guid customerId, [FromBody] CustomerInputModel customer)
        {
            try
            {
                CustomerModel customerModel = (CustomerModel)customer;
                await _customerBusinessLayer.UpdateCustomerAsync(customerId, customerModel);
                return Accepted();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error updating customer");
                return BadRequest();
            }
        }

        // DELETE /customers/0F906772-CADA-40A7-BD59-B47DDD1C75B0
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _customerBusinessLayer.DeleteCustomerAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogInformation(ex, $"Cannot delete non-existent customer {id}");
                return BadRequest();
            }
        }
    }
}
