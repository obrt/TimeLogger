using System;
using System.Collections.Generic;
using System.Text;

namespace Timelogger.DTO.Responses.Customer
{
    public class GetCustomerResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DeveloperId { get; set; }
    }
}
