using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Requests.Project
{
    public class UpdateProjectRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id must be larger than zero!")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Id must be larger than zero!")]
        public int CustomerId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Id must be larger than zero!")]
        public int DeveloperId { get; set; }

        public DateTime Deadline { get; set; }

        public bool IsFinished { get; set; }
    }
}
