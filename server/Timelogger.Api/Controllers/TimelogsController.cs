using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System;
using Timelogger.BusinessLogic.Services;
using Timelogger.DTO.Requests.Customer;
using Timelogger.DTO.Requests.Timelog;
using Timelogger.BusinessLogic.Services.Implementation;

namespace Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    public class TimelogsController : Controller
    {
        private readonly ITimelogService _timelogService;
        private readonly ILogger<TimelogsController> _logger;

        public TimelogsController(ITimelogService timelogService, ILogger<TimelogsController> logger)
        {
            _timelogService = timelogService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) // Display timelogs in days/hours/minutes in frontend based on the minutes from here.
        {
            try
            {
                var timelog = await _timelogService.GetTimelogAsync(id);
                if (timelog == null)
                {
                    return NotFound();
                }
                return Ok(timelog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while getting timelog with Id {id}!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody][Required] GetAllTimelogsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var response = await _timelogService.GetAllTimelogsAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while getting all timelogs!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody][Required] CreateTimelogRequest request) //TODO - frontend should allow for days/hours/minutes
                                                                                                 //and recalculate all that in minutes fore creating request
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var response = await _timelogService.CreateTimelogAsync(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while creating a timelog!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody][Required] UpdateTimelogRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                var response = await _timelogService.UpdateTimelogAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while updating a timelog with Id {request.Id}!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody][Required] DeleteTimelogRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                var response = await _timelogService.DeleteTimelogAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while deleting deliting selected timelogs!");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
