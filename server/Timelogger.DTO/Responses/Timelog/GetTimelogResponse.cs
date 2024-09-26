using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timelogger.DTO.Responses.Timelog
{
    public class GetTimelogResponse
    {
        public int Id { get; set; }

        public string DeveloperName { get; set; }

        public string ProjectName { get; set; }

        public int TimeInMinutes { get; set; }

        public int DeveloperId { get; set; }

        public int ProjectId { get; set; }
    }
}
