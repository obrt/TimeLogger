using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Timelogger.Entities;
using Timelogger.BusinessLogic.Services;
using Timelogger.BusinessLogic.Services.Implementation;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Timelogger.Api
{
	public class Startup
	{
		private readonly IWebHostEnvironment _environment;
		public IConfigurationRoot Configuration { get; }

		public Startup(IWebHostEnvironment env)
		{
			_environment = env;

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("e-conomic interview"));
			services.AddLogging(builder =>
			{
				builder.AddConsole();
				builder.AddDebug();
			});

			services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITimelogService, TimelogService>();
            services.AddScoped<IDeveloperService, DeveloperService>();

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(origin => true);
                });
            });
        }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            app.UseCors("DefaultPolicy");

            app.UseMvc();


			var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
			using (var scope = serviceScopeFactory.CreateScope())
			{
				SeedDatabase(scope);
			}
		}

		private static void SeedDatabase(IServiceScope scope)
		{
            {
                var context = scope.ServiceProvider.GetService<ApiContext>();
                var developer = new Developer
                {
                    FirstName = "Ben",
                    LastName = "Quick",
                    UserName = "benQuick",
                    Email = "ben.quick@malazan.com",
                    Password = "unsecure"
                };
                context.Developers.Add(developer);                
                context.SaveChanges(); // Save changes to generate IDs

                var customer1 = new Customer
                {
                    Name = "The Empire",
                    DeveloperId = 1,
                    Projects = new List<Project>(),
                    Developer = developer
                };
                var customer2 = new Customer
                {
                    Name = "Seven cities",
                    DeveloperId = 1,
                    Projects = new List<Project>(),
                    Developer = developer
                };
                context.Customers.AddRange(customer1, customer2);                
                context.SaveChanges();
                context.Developers.FirstOrDefault().Customers = context.Customers.ToList();

                var projects = new List<Project>();
                for (int i = 1; i <= 5; i++)
                {
                    var project = new Project
                    {
                        Name = $"Project {i}",
                        CustomerId = (i % 2 == 0) ? customer2.Id : customer1.Id,
                        DeveloperId = developer.Id,
                        Deadline = DateTime.Now.AddDays(30 * i),
                        IsFinished = (i % 2 == 0),
                        Timelogs = new List<Timelog>(),
                        Customer = customer1,
                        Developer = developer
                    };
                    projects.Add(project);                    
                }
                context.Projects.AddRange(projects);
                context.SaveChanges();
                foreach (var project in context.Projects)
                {
                    context.Customers.FirstOrDefault(x => x.Id == project.CustomerId ).Projects.Add(project);
                }
                context.SaveChanges();


                foreach (var project in projects)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        var timelog = new Timelog
                        {
                            DeveloperId = developer.Id,
                            ProjectId = project.Id,
                            TimeInMinutes = 60 * j,
                            Project = project,
                            Developer = developer
                        };
                        context.Timelogs.Add(timelog);
                        
                    }
                    context.SaveChanges();

                    foreach (var timelog in context.Timelogs)
                    {
                        context.Projects.FirstOrDefault(x => x.Id == timelog.ProjectId).Timelogs.Add(timelog);
                    }
                    
                }
                context.SaveChanges();
            }
        }

    }
}