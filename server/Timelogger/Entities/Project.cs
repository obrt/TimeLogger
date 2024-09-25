using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Timelogger.Entities
{
	public class Project
	{
		public int Id { get; set; }

		public string Name { get; set; }
		
		public int CustomerId { get; set; }

		public int DeveloperId { get; set; }
				
		public DateTime Deadline { get; set; }
				
		public bool IsFinished { get; set; }

        // Navigation properties
        public Developer Developer { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Timelog> Timelogs { get; set; }
    }
}
