using System;
using System.Collections.Generic;
using System.Text;

namespace Timelogger.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DeveloperId { get; set; }

        // Navigation properties
        public Developer Developer { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
