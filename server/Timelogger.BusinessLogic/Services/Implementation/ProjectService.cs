using System.Collections.Generic;
using System.Threading.Tasks;
using Timelogger.Entities;
using Microsoft.EntityFrameworkCore;
using Timelogger.DTO.Requests.Project;
using Timelogger.DTO.Responses.Project;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Timelogger.BusinessLogic.Services.Implementation
{
    public class ProjectService : IProjectService
    {
        private readonly ApiContext _context;

        public ProjectService(ApiContext context)
        {
            _context = context;
        }

        public async Task<GetProjectResponse> GetProjectAsync(int request)
        {
            var project = await _context.Projects.FirstAsync(p => p.Id == request);
            var response = new GetProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                CustomerId = project.CustomerId,
                Deadline = project.Deadline,
                IsFinished = project.IsFinished,
            };

            return response;
        }        

        public async Task<GetAllProjectsResponse> GetAllProjectsAsync(GetAllProjectsRequest request)
        {
            var projects = await _context.Projects.Where(p => p.DeveloperId == request.DeveloperId).ToListAsync();
            var response = new GetAllProjectsResponse
            {
                Projects = new List<GetProjectResponse>()
            };

            foreach (var project in projects) 
            {
                var singleProject = new GetProjectResponse
                {
                    Id = project.Id,
                    Name = project.Name,
                    CustomerId = project.CustomerId,
                    Deadline = project.Deadline,
                    IsFinished = project.IsFinished,
                };
                response.Projects.Add(singleProject);
            }

            return response;
        }

        public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
        {
            var project = new Project
            {
                Name = request.Name,
                CustomerId = request.CustomerId,
                DeveloperId = request.DeveloperId,
                Deadline = request.Deadline,
                IsFinished = request.IsFinished,
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return new CreateProjectResponse
            {
                Response = $"Project named {project.Name} with Id {project.Id} has been created."
            };
        }

        public async Task<UpdateProjectResponse> UpdateProjectAsync(UpdateProjectRequest request)
        {
            var project = await _context.Projects.FirstAsync(p => p.Id == request.Id);
            project.Name = request.Name;
            project.CustomerId = request.CustomerId;
            project.Deadline = request.Deadline;
            project.IsFinished = request.IsFinished;

            await _context.SaveChangesAsync();

            return new UpdateProjectResponse
            {
                Response = $"Project named {project.Name} with Id {project.Id} has been updated."
            };
        }

        public async Task<DeleteProjectResponse> DeleteProjectAsync(DeleteProjectRequest request)
        {
            var projects = new List<Project>();
            string projectsListString = string.Empty;
            foreach (var id in request.ProjectIds)
            {
                var project = await _context.Projects.Include(p => p.Timelogs).FirstOrDefaultAsync(p => p.Id == id);
                projects.Add(project);                
                projectsListString += $" {project.Id}";
            }

            _context.Projects.RemoveRange(projects);
            await _context.SaveChangesAsync();

            projectsListString = projectsListString.Substring(1, projectsListString.Length - 2);
            var response = new DeleteProjectResponse();
            if (request.ProjectIds.Count == 1)
            {
                response.Response = $"Project with Id {projectsListString} has been deleted!";
            }
            else
            {
                response.Response = $"Projects with Ids {projectsListString} have been deleted!";
            }

            return response;
        }

        public async Task<FinishProjectsResponse> FinishProjectAsync(FinishProjectsRequest request)
        {
            string projectLists = string.Empty;
            foreach (var id in request.ProjectIds)
            {
                var currentProject = await _context.Projects.FirstAsync(p => p.Id == id);
                currentProject.IsFinished = true;
                projectLists += $" {currentProject.Id}";
            }

            await _context.SaveChangesAsync();

            var responseString = string.Empty;
            if (request.ProjectIds.Count == 1)
            {
                responseString = $"Project with Id{projectLists} has been finished.";
            }
            else
            {
                responseString = $"Projects with Ids {projectLists} have been finished.";
            }

            return new FinishProjectsResponse
            {
                Response = responseString
            };
        }
    }
}
