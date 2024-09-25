using System;
using System.Collections.Generic;
using System.Text;

namespace Timelogger.Entities
{
    public class Developer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        // Navigation properties.
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Timelog> Timelogs { get; set; }
    }
}
