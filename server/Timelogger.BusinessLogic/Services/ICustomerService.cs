using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Timelogger.DTO.Requests.Customer;
using Timelogger.DTO.Requests.Project;
using Timelogger.DTO.Responses.Customer;
using Timelogger.Entities;

namespace Timelogger.BusinessLogic.Services
{
    public interface ICustomerService
    {
        Task<GetCustomerResponse> GetCustomerAsync(int id);

        Task<GetAllCustomersResponse> GetAllCustomersAsync(GetAllCustomersRequest request);

        Task<CreateCustomerResponse> CreateCustomerAsync(CreateCustomerRequest request);

        Task<UpdateCustomerResponse> UpdateCustomerAsync(UpdateCustomerRequest request);

        Task<DeleteCustomerResponse> DeleteCustomerAsync(DeleteCustomerRequest request);
    }
}
