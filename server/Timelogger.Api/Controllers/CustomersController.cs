using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System;
using Timelogger.BusinessLogic.Services;
using Timelogger.BusinessLogic.Services.Implementation;
using Timelogger.DTO.Requests.Project;
using Timelogger.DTO.Requests.Customer;

namespace Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while getting customer with id {id}!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody][Required] GetAllCustomersRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var response = await _customerService.GetAllCustomersAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while getting all customers!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody][Required] CreateCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var response = await _customerService.CreateCustomerAsync(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while creating a customer!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody][Required] UpdateCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                var response = await _customerService.UpdateCustomerAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while updating a customer with Id {request.Id}!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody][Required] DeleteCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                var response = await _customerService.DeleteCustomerAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while deliting selected projects!");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
