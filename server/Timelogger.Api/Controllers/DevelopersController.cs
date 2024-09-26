using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System;
using Timelogger.BusinessLogic.Services;
using Timelogger.DTO.Requests.Customer;

namespace Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    public class DevelopersController : Controller
    {
        private readonly IDeveloperService _developerService;
        private readonly ILogger<CustomersController> _logger;

        public DevelopersController(IDeveloperService developerService, ILogger<CustomersController> logger)
        {
            _developerService = developerService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var customer = await _developerService.GetDeveloperAsync(id);
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
        public async Task<IActionResult> Get()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var response = await _developerService.GetAllDevelopersAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while getting all customers!");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
