using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Requests.Project
{
    public class FinishProjectsRequest
    {
        [Required]
        public List<int> ProjectIds { get; set; }
    }
}
