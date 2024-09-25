using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Timelogger.DTO.Requests.Project;
using Timelogger.DTO.Requests.Timelog;
using Timelogger.DTO.Responses.Project;
using Timelogger.DTO.Responses.Timelog;

namespace Timelogger.BusinessLogic.Services
{
    public interface ITimelogService
    {
        Task<GetTimelogResponse> GetTimelogAsync(int request);

        Task<GetAllTimelogsResponse> GetAllTimelogsAsync(GetAllTimelogsRequest request);

        Task<CreateTimelogResponse> CreateTimelogAsync(CreateTimelogRequest request);

        Task<UpdateTimelogResponse> UpdateTimelogAsync(UpdateTimelogRequest request);

        Task<DeleteTimelogResponse> DeleteTimelogAsync(DeleteTimelogRequest request);
    }
}
