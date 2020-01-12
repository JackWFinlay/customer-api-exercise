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
            return await GetOrThrowErrorAsync(_customerBusinessLayer.GetAllCustomersAsync());
        }

        // GET /customers/0f32959d-0e54-47f2-9da4-d6c0a52ca070
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> Get(Guid id)
        {
            return await _customerBusinessLayer.GetCustomerAsync(id);
        }

        // GET /customers/search?searchPhrase=Jack
        [HttpGet]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> Search([FromQuery] string searchPhrase)
        {
            return await GetOrThrowErrorAsync(_customerBusinessLayer.SearchCustomersAsync(searchPhrase), searchPhrase);
        }

        // POST /customers
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] CustomerModel customer)
        {
            try
            {
                await _customerBusinessLayer.AddCustomerAsync(customer);
                return Accepted();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error adding customer");
                return BadRequest("Could not add customer");
            }
        }

        // PUT /customers/0F906772-CADA-40A7-BD59-B47DDD1C75B0
        [HttpPut("{customerId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task Put(Guid customerId, [FromBody] CustomerModel customer)
        {
            try
            {
                await _customerBusinessLayer.UpdateCustomerAsync(customerId, customer);
                Accepted();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error updating customer");
                BadRequest($"Could not update customer {customerId}");
            }
        }

        // DELETE /customers/0F906772-CADA-40A7-BD59-B47DDD1C75B0
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task Delete(Guid id)
        {
            await _customerBusinessLayer.DeleteCustomerAsync(id);
            Accepted();
        }

        private async Task<ActionResult<IEnumerable<CustomerModel>>> GetOrThrowErrorAsync(Task<IEnumerable<CustomerModel>> task, string searchPhrase = null)
        {
            try
            {
                return Ok(await task);
            }
            catch (Exception ex)
            {
                if (searchPhrase != null)
                {
                    _logger.LogInformation(ex, $"Exception occured getting customers for search term '{searchPhrase}'");
                }
                else
                {
                    _logger.LogInformation(ex, "Exception occured getting customers");
                }

                return BadRequest();
            }
        }
    }
}
