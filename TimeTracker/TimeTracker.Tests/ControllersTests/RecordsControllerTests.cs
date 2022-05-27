using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
    public class RecordsControllerTests
    {
        private readonly RecordsController _controller;

        private readonly IRecordsRepository _recordsRepository;

        private readonly IGenericRepository<Project> _projectsRepository;

        private readonly ILogger<ProjectsController> _logger;

        public RecordsControllerTests()
        {
            this._recordsRepository = A.Fake<IRecordsRepository>();
            this._projectsRepository = A.Fake<IGenericRepository<Project>>();
            this._logger = A.Fake<ILogger<ProjectsController>>();
            this._controller = new RecordsController(this._recordsRepository, this._logger,
                                                     this._projectsRepository);
        }

        [Fact]
        public async Task GetTimeTrackedForDay_ReturnsInt()
        {
            var projectsDummy = A.CollectionOfDummy<Project>(5).AsEnumerable();
            A.CallTo(() => this._recordsRepository.GetTrackedTimeAsync(1, new DateOnly(2022, 5, 27)))
             .Returns(Task.FromResult(6));

            var actionResult = await this._controller.GetTimeTrackedForDay(1, 2022, 5, 27);

            Assert.Equal(6, actionResult.Value);
        }

        [Fact]
        public async Task GetTimeTrackedForWeek_ReturnsInt()
        {
            var projectsDummy = A.CollectionOfDummy<Project>(5).AsEnumerable();
            A.CallTo(() => this._recordsRepository.GetTrackedTimeAsync(1, 2022, 15))
             .Returns(Task.FromResult(20));

            var actionResult = await this._controller.GetTimeTrackedForWeek(1, 2022, 15);

            Assert.Equal(20, actionResult.Value);
        }
    }
}
