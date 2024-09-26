using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelogger.DTO.Responses.Customer;
using Timelogger.DTO.Responses.Developer;

namespace Timelogger.BusinessLogic.Services.Implementation
{
    public class DeveloperService : IDeveloperService
    {
        private readonly ApiContext _context;

        public DeveloperService(ApiContext context)
        {
            _context = context;
        }
        public async Task<GetAllDevelopersResponse> GetAllDevelopersAsync()
        {
            var developers = await _context.Developers.ToListAsync();
            var response = new GetAllDevelopersResponse
            {
                Developers = new List<GetDeveloperResponse>()
            };
            foreach (var developer in developers) 
            {
                var singleDeveloper = new GetDeveloperResponse
                {
                    Id = developer.Id,
                    FirstName = developer.FirstName,
                    LastName = developer.LastName,
                    UserName = developer.UserName,
                    Email = developer.Email,
                    Password = developer.Password,
                };

                response.Developers.Add(singleDeveloper);
            }

            return response;            
        }

        public async Task<GetDeveloperResponse> GetDeveloperAsync(int id)
        {
            var developer = await _context.Developers.FirstOrDefaultAsync(x => x.Id == id);
            var response = new GetDeveloperResponse
            {
                Id = developer.Id,
                FirstName = developer.FirstName,
                LastName = developer.LastName,
                UserName = developer.UserName,
                Email = developer.Email,
                Password = developer.Password,
            };

            return response;
        }
    }
}
