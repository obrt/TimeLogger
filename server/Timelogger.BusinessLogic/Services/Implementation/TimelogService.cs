using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelogger.DTO.Requests.Timelog;
using Timelogger.DTO.Responses.Project;
using Timelogger.DTO.Responses.Timelog;
using Timelogger.Entities;

namespace Timelogger.BusinessLogic.Services.Implementation
{
    public class TimelogService : ITimelogService
    {
        private readonly ApiContext _context;

        public TimelogService(ApiContext context)
        {
            _context = context;
        }

        public async Task<GetTimelogResponse> GetTimelogAsync(int request)
        {
            var timelog = await _context.Timelogs.FirstAsync(x => x.Id == request);
            var response = new GetTimelogResponse
            {
                Id = timelog.Id,
                ProjectName = timelog.Project.Name,
                DeveloperName = timelog.Developer.FirstName + " " + timelog.Developer.LastName,
                TimeInMinutes = timelog.TimeInMinutes,
                ProjectId = timelog.ProjectId,
                DeveloperId = timelog.Developer.Id
            };

            return response;
        }

        public async Task<GetAllTimelogsResponse> GetAllTimelogsAsync(GetAllTimelogsRequest request)
        {
            var timelogs = await _context.Timelogs.Where(x => x.DeveloperId == request.DeveloperId).Include(x => x.Developer).Include(x => x.Project).ToListAsync();
            var response = new GetAllTimelogsResponse
            {
                Timelogs = new List<GetTimelogResponse>()
            };

            foreach (var timelog in timelogs)
            {
                var singleTimelog = new GetTimelogResponse
                {
                    Id = timelog.Id,
                    ProjectName = timelog.Project.Name,
                    DeveloperName = timelog.Developer.FirstName + " " + timelog.Developer.LastName,
                    TimeInMinutes = timelog.TimeInMinutes,
                    ProjectId = timelog.ProjectId,
                    DeveloperId = timelog.Developer.Id
                };
                response.Timelogs.Add(singleTimelog);
            }

            return response;
        }
        
        public async Task<CreateTimelogResponse> CreateTimelogAsync(CreateTimelogRequest request)
        {
            var timelog = new Timelog
            {
                ProjectId = request.ProjectId,
                DeveloperId = request.DeveloperId,
                TimeInMinutes = request.TimeInMinutes
            };
            await _context.Timelogs.AddAsync(timelog);
            await _context.SaveChangesAsync();

            return new CreateTimelogResponse
            {
                Response = $"Timelog with Id {timelog.Id} has been created."
            };
        }        

        public async Task<UpdateTimelogResponse> UpdateTimelogAsync(UpdateTimelogRequest request)
        {
            var timelog = await _context.Timelogs.FirstAsync(x => x.Id == request.Id);
            timelog.ProjectId = request.ProjectId;
            timelog.DeveloperId = request.DeveloperId;
            timelog.TimeInMinutes = request.TimeInMinutes;

            await _context.SaveChangesAsync();

            return new UpdateTimelogResponse
            {
                Response = $"Timelog with Id {timelog.Id} has been updated."
            };
        }

        public async Task<DeleteTimelogResponse> DeleteTimelogAsync(DeleteTimelogRequest request)
        {
            var timelogs = new List<Timelog>();
            string timelogListString = string.Empty;
            foreach (var id in request.TimelogIds)
            {
                var timelog = await _context.Timelogs.FirstOrDefaultAsync(x => x.Id == id);
                timelogs.Add(timelog);
                timelogListString += $" {timelog.Id},";
            }

            _context.Timelogs.RemoveRange(timelogs);
            await _context.SaveChangesAsync();

            timelogListString = timelogListString.Substring(1, timelogListString.Length - 2);
            var response = new DeleteTimelogResponse();
            if (request.TimelogIds.Count == 1)
            {
                response.Response = $"Timelog with Id {timelogListString} has been deleted!";
            }
            else
            {
                response.Response = $"Timelog with Ids {timelogListString} have been deleted!";
            }

            return response;
        }
    }
}
