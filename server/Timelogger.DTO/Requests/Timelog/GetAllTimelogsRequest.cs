using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Requests.Timelog
{
    public class GetAllTimelogsRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "DeveloperId must be larger than zero!")]
        public int DeveloperId { get; set; }
    }
}
