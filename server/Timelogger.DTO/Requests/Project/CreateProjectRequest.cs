using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Requests.Project
{
    public class CreateProjectRequest
    {
        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Id must be larger than zero!")]
        public int CustomerId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Id must be larger than zero!")]
        public int DeveloperId { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public bool IsFinished { get; set; }
    }
}
