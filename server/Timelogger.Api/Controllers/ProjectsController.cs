using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Timelogger.BusinessLogic.Services;
using Timelogger.BusinessLogic.Services.Implementation;
using Timelogger.DTO.Requests.Customer;
using Timelogger.DTO.Requests.Project;
using Timelogger.Entities;

namespace Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var project = await _projectService.GetProjectAsync(id);
                if (project == null)
                {
                    return NotFound();
                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while getting a project with Id {id}!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("getall")]
        public async Task<IActionResult> GetAll([FromBody][Required] GetAllProjectsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var projects = await _projectService.GetAllProjectsAsync(request);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while getting all projects!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody][Required] CreateProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                var response = await _projectService.CreateProjectAsync(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while creating a project!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("updateProject")]
        public async Task<IActionResult> Put([FromBody][Required] UpdateProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                var response = await _projectService.UpdateProjectAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while updating a project with Id {request.Id}!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody][Required] DeleteProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                var response = await _projectService.DeleteProjectAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while deliting selected projects!");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("finishProjects")]
        public async Task<IActionResult> FinishProjects([FromBody][Required] FinishProjectsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                var response = await _projectService.FinishProjectAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while finishing selected projects!");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
