using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Requests.Customer
{
    public class UpdateCustomerRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id must be larger than zero!")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
