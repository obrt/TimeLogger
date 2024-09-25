using Microsoft.EntityFrameworkCore;
using Timelogger.Entities;

namespace Timelogger
{
	public class ApiContext : DbContext
	{
		public ApiContext(DbContextOptions<ApiContext> options)
			: base(options)
		{
		}

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Project> Projects { get; set; }		
		public DbSet<Timelog> Timelogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Developer>()
                .HasMany(d => d.Customers)
                .WithOne(c => c.Developer)
                .HasForeignKey(c => c.DeveloperId)
                .OnDelete(DeleteBehavior.Cascade);

            // Correct navigation properties if necessary
            //modelBuilder.Entity<Developer>()
            //    .HasMany(d => d.Projects)
            //    .WithOne(p => p.Developer)
            //    .HasForeignKey(p => p.DeveloperId)
            //    .OnDelete(DeleteBehavior.Cascade);                      

            //modelBuilder.Entity<Developer>()
            //    .HasMany(d => d.Timelogs)
            //    .WithOne(c => c.Developer)
            //    .HasForeignKey(c => c.DeveloperId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Timelogs)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Projects)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Timelog>()
            //    .HasOne(t => t.Developer)
            //    .WithMany(d => d.Timelogs)
            //    .HasForeignKey(t => t.DeveloperId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
