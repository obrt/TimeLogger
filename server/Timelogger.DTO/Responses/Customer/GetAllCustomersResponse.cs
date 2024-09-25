using System;
using System.Collections.Generic;
using System.Text;

namespace Timelogger.DTO.Responses.Customer
{
    public class GetAllCustomersResponse
    {
        public List<GetCustomerResponse> Customers { get; set; }
    }
}
