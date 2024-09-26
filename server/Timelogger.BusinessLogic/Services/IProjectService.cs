using System.Collections.Generic;
using System.Threading.Tasks;
using Timelogger.DTO.Requests.Project;
using Timelogger.DTO.Responses.Project;
using Timelogger.Entities;

namespace Timelogger.BusinessLogic.Services
{
    public interface IProjectService
    {
        Task<GetProjectResponse> GetProjectAsync(int id);

        Task<GetAllProjectsResponse> GetAllProjectsAsync(GetAllProjectsRequest request);

        Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request);

        Task<UpdateProjectResponse> UpdateProjectAsync(UpdateProjectRequest request);

        Task<DeleteProjectResponse> DeleteProjectAsync(DeleteProjectRequest request);

        Task<FinishProjectsResponse> FinishProjectAsync(FinishProjectsRequest request);
    }
}
