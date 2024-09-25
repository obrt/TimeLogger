using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Requests.Customer
{
    public class GetAllCustomersRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "Id must be larger than zero!")]
        public int DeveloperId {  get; set; }
    }
}
