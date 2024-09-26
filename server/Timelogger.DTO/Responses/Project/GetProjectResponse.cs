using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Responses.Project
{
    public class GetProjectResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CustomerId { get; set; }

        public int DeveloperId { get; set; }

        public string CustomerName { get; set; }

        public string DeveloperName { get; set; }

        public string Deadline { get; set; }

        public bool IsFinished { get; set; }

        public int TotalTimeLogged { get; set; }
    }
}
