using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Requests.Timelog
{
    public class DeleteTimelogRequest
    {
        [Required]
        public List<int> TimelogIds { get; set; }
    }
}
