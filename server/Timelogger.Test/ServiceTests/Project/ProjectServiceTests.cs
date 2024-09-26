using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timelogger.Entities;
using System.ComponentModel.DataAnnotations;
using Timelogger;
using Timelogger.BusinessLogic.Services.Implementation;
using Microsoft.EntityFrameworkCore;
using Timelogger.DTO.Requests.Project;
using System.Threading;
using System.Linq.Expressions;

[TestClass]
public class ProjectServiceTests
{
    private ApiContext _context;
    private ProjectService _projectService;

    [TestInitialize]
    public void Setup()
    {
        // Use an in-memory database for testing
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApiContext(options);
        _projectService = new ProjectService(_context);

    }

    [TestMethod]
    public async Task GetProjectAsync_ReturnsCorrectProject()
    {
        // Arrange
        var projectId = 1;
        _context.Projects.Add(new Project
        {
            Id = 1,
            Name = "Test Project",
            Customer = new Customer { Id = 1, Name = "Test Customer" },
            Developer = new Developer { Id = 1, FirstName = "John", LastName = "Doe" },
            Deadline = DateTime.Now,
            IsFinished = false
        });

        _context.Timelogs.Add(new Timelog { ProjectId = 1, TimeInMinutes = 120 });
        _context.Timelogs.Add(new Timelog { ProjectId = 1, TimeInMinutes = 180 });
        var totalTime = 300;

        // Act
        var result = await _projectService.GetProjectAsync(projectId);

        // Assert
        var expectedProject = _context.Projects.First();
        Assert.AreEqual(expectedProject.Id, result.Id);
        Assert.AreEqual(expectedProject.Name, result.Name);
        Assert.AreEqual("John Doe", result.DeveloperName);
        Assert.AreEqual(result.TotalTimeLogged, totalTime);

        _context.Projects.ToList().Clear();
        _context.Timelogs.ToList().Clear();
    }

    

    [TestMethod]
    public async Task CreateProjectAsync_ReturnsCorrectResponse()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "New Project",
            CustomerId = 1,
            DeveloperId = 1,
            Deadline = DateTime.Now.AddDays(30),
            IsFinished = false
        };

        // Act
        var result = await _projectService.CreateProjectAsync(request);

        // Assert
        Assert.IsTrue(_context.Projects.Any());

        _context.Projects.ToList().Clear();
        _context.Timelogs.ToList().Clear();
    }

    [TestMethod]
    public async Task UpdateProjectAsync_ReturnsUpdatedResponse()
    {
        // Arrange
        var request = new UpdateProjectRequest
        {
            Id = 1,
            Name = "Updated Project",
            CustomerId = 1,
            Deadline = DateTime.Now.AddDays(30),
            DeveloperId = 1,
            IsFinished = true
        };

        _context.Projects.Add(new Project { Id = 1, Name = "Old Project", CustomerId = 1, IsFinished = false });
        _context.Developers.Add(new Developer { Id = 1, FirstName = "Quick", LastName = "Ben" });
        _context.Customers.Add(new Customer { Id = 1, Name = "Empire" });

        await _context.SaveChangesAsync();
        // Act
        var result = await _projectService.UpdateProjectAsync(request);

        // Assert
        Assert.IsTrue(_context.Projects.First(x => x.Id == request.Id).IsFinished);

        _context.Projects.ToList().Clear();
        _context.Timelogs.ToList().Clear();
    }

    [TestMethod]
    public async Task DeleteProjectAsync_ReturnsCorrectResponse()
    {
        // Arrange
        var request = new DeleteProjectRequest { ProjectIds = new List<int> { 1, 2 } };

        _context.Projects.Add(new Project { Id = 1, Name = "First Project", CustomerId = 1, IsFinished = false });
        _context.Projects.Add(new Project { Id = 2, Name = "Second Project", CustomerId = 1, IsFinished = true });
        _context.Developers.Add(new Developer { Id = 1, FirstName = "Quick", LastName = "Ben" });
        _context.Customers.Add(new Customer { Id = 1, Name = "Empire" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _projectService.DeleteProjectAsync(request);

        // Assert
        Assert.IsFalse(_context.Projects.Any());

        _context.Projects.ToList().Clear();
        _context.Timelogs.ToList().Clear();
    }

    // Failure tests

    [TestMethod]
    public async Task DeleteProjectAsync_InvalidProjectIds_ReturnsErrorResponse()
    {
        // Arrange
        var request = new DeleteProjectRequest { ProjectIds = new List<int> { 999 } }; // Non-existent project

        // Act

        // Assert
        await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
        {
            await _projectService.DeleteProjectAsync(request);
        });
        //Assert.IsTrue(result.Response.Contains("No projects found for deletion"));
    }
}
