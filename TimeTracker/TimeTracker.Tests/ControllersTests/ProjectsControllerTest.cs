using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.API.Controllers;
using TimeTracker.Application.DTO;
using TimeTracker.Application.IRepositories;
using TimeTracker.Core.Entities;
using Xunit;

namespace TimeTracker.Tests.ControllersTests
{
    public class ProjectsControllerTest
    {
        private readonly ProjectsController _controller;

        private readonly IGenericRepository<Project> _projectsRepository;

        private readonly ILogger<ProjectsController> _logger;

        public ProjectsControllerTest()
        {
            this._projectsRepository = A.Fake<IGenericRepository<Project>>();
            this._logger = A.Fake<ILogger<ProjectsController>>();
            this._controller = new ProjectsController(this._projectsRepository, this._logger);
        }

        [Fact]
        public async Task GetAllProjects_ReturnsIEnumerable()
        {
            var projectsDummy = A.CollectionOfDummy<Project>(5).AsEnumerable();
            A.CallTo(() => this._projectsRepository.GetAllAsync())
             .Returns(Task.FromResult(projectsDummy));

            var actionResult = await this._controller.GetAllProjects();
            var result = actionResult.Result as OkObjectResult;
            var projects = result.Value as IEnumerable<ProjectDTO>;
            
            Assert.Equal(5, projects.Count());
        }

        [Fact]
        public async Task GetProject_ReturnsNotFound()
        {
            A.CallTo(() => this._projectsRepository.GetAsync(1, null))
             .WithAnyArguments()
             .Returns(Task.FromResult(null as Project));

            var actionResult = await this._controller.GetProject(1);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Theory]
        [InlineData(2)]
        public async Task GetProject_ReturnsProject(int id)
        {
            var project = A.Dummy<Project>();
            A.CallTo(() => this._projectsRepository.GetAsync(id, p => p.Records))
             .Returns(Task.FromResult(project));

            var actionResult = await this._controller.GetProject(id);

            Assert.IsAssignableFrom<Project>(actionResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var projectDTO = A.Dummy<ProjectDTO>();

            var actionResult = await this._controller.Create(projectDTO);

            Assert.IsType<CreatedAtActionResult>(actionResult);
        }

        [Fact]
        public async Task Update_ReturnsNotFound()
        {
            var projectDTO = A.Dummy<ProjectDTO>();
            A.CallTo(() => this._projectsRepository.GetAsync(1))
             .Returns(Task.FromResult(null as Project));

            var actionResult = await this._controller.Update(1, projectDTO);

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            var project = A.Dummy<Project>();
            var projectDTO = A.Dummy<ProjectDTO>();
            A.CallTo(() => this._projectsRepository.GetAsync(1))
             .Returns(Task.FromResult(project));

            var actionResult = await this._controller.Update(1, projectDTO);

            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound()
        {
            A.CallTo(() => this._projectsRepository.GetAsync(1))
             .Returns(Task.FromResult(null as Project));

            var actionResult = await this._controller.Delete(1);

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            var project = A.Dummy<Project>();
            A.CallTo(() => this._projectsRepository.GetAsync(2))
             .Returns(Task.FromResult(project));

            var actionResult = await this._controller.Delete(2);

            Assert.IsType<NoContentResult>(actionResult);
        }
    }
}
