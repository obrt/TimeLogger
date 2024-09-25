using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Requests.Customer
{
    public class DeleteCustomerRequest
    {
        [Required]
        public List<int> CustomerIds { get; set; }
    }
}
