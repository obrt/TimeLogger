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
            var timelog = await _context.Timelogs.FirstAsync(t => t.Id == request);
            var response = new GetTimelogResponse
            {
                Id = timelog.Id,
                ProjectId = timelog.ProjectId,
                DeveloperId = timelog.DeveloperId,
                TimeInMinutes = timelog.TimeInMinutes
            };

            return response;
        }

        public async Task<GetAllTimelogsResponse> GetAllTimelogsAsync(GetAllTimelogsRequest request)
        {
            var timelogs = await _context.Timelogs.Where(t => t.DeveloperId == request.DeveloperId).ToListAsync();
            var response = new GetAllTimelogsResponse
            {
                Timelogs = new List<GetTimelogResponse>()
            };

            foreach (var timelog in timelogs)
            {
                var singleTimelog = new GetTimelogResponse
                {
                    Id = timelog.Id,
                    ProjectId = timelog.ProjectId,
                    DeveloperId = timelog.DeveloperId,
                    TimeInMinutes = timelog.TimeInMinutes
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
            var timelog = await _context.Timelogs.FirstAsync(t => t.Id == request.Id);
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
                var timelog = await _context.Timelogs.FirstOrDefaultAsync(t => t.Id == id);
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
