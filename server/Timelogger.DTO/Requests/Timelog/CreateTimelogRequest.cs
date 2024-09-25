using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Requests.Timelog
{
    public class CreateTimelogRequest
    {

        [Range(0, int.MaxValue, ErrorMessage = "DeveloperId must be larger than zero!")]
        public int DeveloperId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "ProjectId must be larger than zero!")]
        public int ProjectId { get; set; }

        [Range(30, int.MaxValue, ErrorMessage = "Can't log less than 30 minutes!")]
        public int TimeInMinutes { get; set; }
    }
}
