using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Timelogger.DTO.Requests.Project;
using Timelogger.DTO.Responses.Developer;
using Timelogger.DTO.Responses.Project;

namespace Timelogger.BusinessLogic.Services
{
    public interface IDeveloperService
    {
        Task<GetDeveloperResponse> GetDeveloperAsync(int id);

        Task<GetAllDevelopersResponse> GetAllDevelopersAsync();
    }
}
