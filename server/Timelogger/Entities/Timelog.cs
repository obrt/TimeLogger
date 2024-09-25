using System;
using System.Collections.Generic;
using System.Text;

namespace Timelogger.Entities
{
    public class Timelog
    {
        public int Id { get; set; }

        public int DeveloperId { get; set; }

        public int ProjectId { get; set; }

        public int TimeInMinutes {  get; set; }

        // Navigation properties
        public Project Project { get; set; }
        //public Developer Developer { get; set; }
    }
}
