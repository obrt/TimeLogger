using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelogger.DTO.Requests.Customer;
using Timelogger.DTO.Responses.Customer;
using Timelogger.DTO.Responses.Project;
using Timelogger.Entities;

namespace Timelogger.BusinessLogic.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ApiContext _context;

        public CustomerService(ApiContext context)
        {
            _context = context;
        }

        public async Task<GetCustomerResponse> GetCustomerAsync(int request)
        {
            var customer = await _context.Customers.FirstAsync(p => p.Id == request);
            var response = new GetCustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                DeveloperId = customer.DeveloperId
            };

            return response;
        }

        public async Task<GetAllCustomersResponse> GetAllCustomersAsync(GetAllCustomersRequest request)
        {
            var customers = await _context.Customers.Where(c => c.DeveloperId == request.DeveloperId).ToListAsync();
            var response = new GetAllCustomersResponse
            {
                Customers = new List<GetCustomerResponse>()
            };

            foreach (var customer in customers)
            {
                var singleCustomer = new GetCustomerResponse
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    DeveloperId = customer.DeveloperId,
                };

                response.Customers.Add(singleCustomer);
            }

            return response;
        }

        public async Task<CreateCustomerResponse> CreateCustomerAsync(CreateCustomerRequest request)
        {
            var customer = new Customer
            {
                Name = request.Name,
                DeveloperId = request.DeveloperId,
            };

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return new CreateCustomerResponse
            {
                Response = $"Customer named {customer.Name} with Id {customer.Id} has been created."
            };
        }

        public async Task<UpdateCustomerResponse> UpdateCustomerAsync(UpdateCustomerRequest request)
        {
            var customer = await _context.Customers.FirstAsync(c => c.Id == request.Id);
            customer.Name = request.Name;
            await _context.SaveChangesAsync();

            return new UpdateCustomerResponse
            {
                Response = $"Customer with Id {customer.Id} has been updated."
            };
        }
        public async Task<DeleteCustomerResponse> DeleteCustomerAsync(DeleteCustomerRequest request)
        {
            var customers = new List<Customer>();
            string customerListString = string.Empty;
            foreach (var id in request.CustomerIds)
            {
                var customer = await _context.Customers
                    .Include(c => c.Projects)
                    .ThenInclude(p => p.Timelogs)
                    .FirstOrDefaultAsync(c => c.Id == id);
                customers.Add(customer);
                customerListString += $" {customer.Id}";
            }

            _context.Customers.RemoveRange(customers);
            await _context.SaveChangesAsync();

            customerListString = customerListString.Substring(1, customerListString.Length - 2);
            var response = new DeleteCustomerResponse();
            if (request.CustomerIds.Count == 1)
            {
                response.Response = $"Customer with Id {customerListString} has been deleted!";
            }
            else
            {
                response.Response = $"Customer with Ids {customerListString} have been deleted!";
            }

            return response;
        }
    }
}
