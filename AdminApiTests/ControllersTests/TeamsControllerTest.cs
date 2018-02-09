using System.Collections.Generic;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Data.Service.Persistance.Repositories;
using LVMiniAdminApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace AdminApiTests.ControllersTests
{
    [TestFixture]
    class TeamsControllerTest
    {
        private AdminTeamsController teamsController;
        private Mock<ITeamRepository> _teamRepository;
        private Mock<IUserRepository> _userRepository;
        

        [SetUp]
        public void Init()
        {
            _teamRepository = new Mock<ITeamRepository>();
            _userRepository = new Mock<IUserRepository>();

            teamsController = new AdminTeamsController(_teamRepository.Object, _userRepository.Object);
        }

        [Test]
        public void GetAllMustReturnsCollectionOfTeams()
        {
            var _testGetAllFirstTeam = new Team() { IsActive = true, TeamId = 1, TeamName = "TEST_TEAM_NAME_1", UsersTeams = new List<UserTeam>() };
            var _testGetAllSecondTeam = new Team() { IsActive = true, TeamId = 2, TeamName = "TEST_TEAM_NAME_2", UsersTeams = new List<UserTeam>() };
            var _testGetAllTeams = new List<Team>() { _testGetAllFirstTeam, _testGetAllSecondTeam };
            _teamRepository.Setup(rep => rep.GetAll()).ReturnsAsync(_testGetAllTeams);

            var getAllResult = teamsController.GetAll().Result as OkObjectResult;
            var asModelResource = getAllResult.Value as ICollection<Team>;

            Assert.NotNull(getAllResult);
            Assert.NotNull(asModelResource);
            CollectionAssert.AreEqual(_testGetAllTeams, asModelResource);
        }

        [Test]
        public void GetCurrentMustReturnsUserPassedByUsernameThroughUri()
        {
            var teamName = "TEST_TEAM_NAME_1";
            var _testGetAllFirstTeam = new Team() { IsActive = true, TeamId = 1, TeamName = teamName, UsersTeams = new List<UserTeam>() };
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(_testGetAllFirstTeam);

            var getByUsername = teamsController.GetCurrent(teamName).Result as OkObjectResult;
            var asModelResource = getByUsername.Value as Team;

            Assert.NotNull(getByUsername);
            Assert.NotNull(asModelResource);
            Assert.AreEqual(_testGetAllFirstTeam, asModelResource);
        }
    }
}
