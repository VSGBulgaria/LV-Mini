using System;
using System.Collections.Generic;
using System.Linq;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using LVMiniAdminApi.Controllers;
using LVMiniAdminApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace AdminApiTests.ControllersTests
{
    [TestFixture]
    class TeamsControllerTest
    {
        private AdminTeamsController _teamsController;
        private Mock<ITeamRepository> _teamRepository;
        private Mock<IUserRepository> _userRepository;


        [SetUp]
        public void Init()
        {
            _teamRepository = new Mock<ITeamRepository>();
            _userRepository = new Mock<IUserRepository>();

            _teamsController = new AdminTeamsController(_teamRepository.Object, _userRepository.Object);
        }

        #region GetAllMethod

        [Test]
        public void GetAllMustReturnsCollectionOfTeams()
        {
            var testGetAllFirstTeam = new Team() { IsActive = true, TeamId = 1, TeamName = "TEST_TEAM_NAME_1", UsersTeams = new List<UserTeam>() };
            var testGetAllSecondTeam = new Team() { IsActive = true, TeamId = 2, TeamName = "TEST_TEAM_NAME_2", UsersTeams = new List<UserTeam>() };
            var testGetAllTeams = new List<Team>() { testGetAllFirstTeam, testGetAllSecondTeam };
            _teamRepository.Setup(rep => rep.GetAll()).ReturnsAsync(testGetAllTeams);

            var getAllResult = _teamsController.GetAll().Result as OkObjectResult;
            var asModelResource = getAllResult?.Value as ICollection<Team>;

            Assert.NotNull(getAllResult);
            Assert.NotNull(asModelResource);
            CollectionAssert.AreEqual(testGetAllTeams, asModelResource);
        }

        #endregion

        #region GetCurrentTeamMethod

        [Test]
        public void GetCurrentTeamMustReturnsTeamIfExistsNormalScenario()
        {
            var teamName = "TEST_TEAM_NAME_1";
            var testGetCurrentTeam = new Team() { IsActive = true, TeamId = 1, TeamName = teamName, UsersTeams = new List<UserTeam>() };
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(testGetCurrentTeam);

            var getByUsername = _teamsController.GetCurrent(teamName).Result as OkObjectResult;
            var asModelResource = getByUsername?.Value as Team;

            Assert.NotNull(getByUsername);
            Assert.NotNull(asModelResource);
            Assert.AreEqual(testGetCurrentTeam, asModelResource);
        }

        [Test]
        public void GetCurrentTeamMustReturnsBadRequestIfTheTeamDoesNotExist()
        {
            var teamName = "TEST_TEAM_NAME";
            var testGetCurrentTeam = new Team() { IsActive = true, TeamId = 1, TeamName = teamName, UsersTeams = new List<UserTeam>() };
            var unExistingTeamName = "UN _EXISTING_TEAM_NAME";
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(testGetCurrentTeam);
            var expectedResultValue = "Invalid team name.";

            var actualResult = _teamsController.GetCurrent(unExistingTeamName).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedResultValue, actualRequestValue);
        }

        #endregion

        #region PostTeamMethod

        [Test]
        public void PostTeamMustReturnsBadRequestObjectResultInvalidStateScenario()
        {
            var invalidStateTeam = new LVMiniAdminApi.Models.TeamModels.TeamDto();
            var expectedMessage = "Invalid model state";

            var actualResult = _teamsController.PostTeam(invalidStateTeam).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        [Test]
        public void PostTeamMustReturnsBadRequestIfTeamWithGivenTeamNameAlreadyExist()
        {
            var teamName = "VALID_TEAM_NAME";
            var team = new Team() { TeamName = teamName, IsActive = true };
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(team);
            var expectedMessage = $"Team with name {team.TeamName} is already exists.";

            var actualResult = _teamsController
                .PostTeam(new LVMiniAdminApi.Models.TeamModels.TeamDto()
                {
                    TeamName = team.TeamName,
                    IsActive = team.IsActive
                }).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        [Test]
        public void PostTeamMustReturnsBadRequestIfTeamWasNotPersistInDb()
        {
            var teamName = "VALID_TEAM_NAME";
            var team = new Team() { TeamName = teamName, IsActive = true };
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(0);
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(team);
            var expectedMessage = "Something went wrong. Team was not created.";

            var actualResult = _teamsController
                .PostTeam(new LVMiniAdminApi.Models.TeamModels.TeamDto()
                {
                    TeamName = "UNEXISTABLE_TEAM_NAME",
                    IsActive = team.IsActive
                }).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        [Test]
        public void PostTeamMustReturnsOkIfTeamIsPersistedSuccessfully()
        {
            var teamName = "VALID_TEAM_NAME";
            var storedTeam = new Team() { TeamName = teamName, IsActive = true };
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(1);
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(storedTeam);
            var expectedMessage = "Team created.";
            var newTeam = new Team() { TeamName = "UN_STORED_TEAM_NAME", IsActive = true };

            var actualResult = _teamsController.PostTeam(
                new LVMiniAdminApi.Models.TeamModels.TeamDto() { TeamName = newTeam.TeamName, IsActive = newTeam.IsActive }).Result;

            Assert.IsInstanceOf<OkObjectResult>(actualResult);
            var actualRequestValue = (actualResult as OkObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        #endregion

        #region MakeTeamInActiveMethod

        [Test]
        public void MakeTeamInActiveMustReturnsBadRequestIfThereIsNotTeamWithGivenNameInTheDb()
        {
            var storedTeamName = "STORED_TEAM_NAME";
            _teamRepository.Setup(rep => rep.GetByTeamName(storedTeamName)).ReturnsAsync(null as Team);
            var expectedMessage = $"{storedTeamName} does not exist.";

            var actualResult = _teamsController.MakeTeamInActive(storedTeamName).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        [Test]
        public void MakeTeamInActiveMustReturnsBadRequestIfTheTeamIsAlreadyInActive()
        {
            var teamName = "STORED_IN_ACTIVE_TEAM_NAME";
            var storedInActiveTeam = new Team() { TeamName = teamName, IsActive = false };
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(storedInActiveTeam);
            var expectedMessage = $"{teamName} is already inactive";

            var actualResult = _teamsController.MakeTeamInActive(storedInActiveTeam.TeamName).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        [Test]
        public void MakeTeamInActiveMustReturnsOkNormalScenario()
        {
            var teamName = "STORED_IN_ACTIVE_TEAM_NAME";
            var storedInActiveTeam = new Team() { TeamName = teamName, IsActive = true };
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(storedInActiveTeam);
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(1);
            var expectedMessage = $"{teamName} is now inactive.";

            var actualResult = _teamsController.MakeTeamInActive(storedInActiveTeam.TeamName).Result;

            Assert.IsInstanceOf<OkObjectResult>(actualResult);
            var actualRequestValue = (actualResult as OkObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        [Test]
        public void MakeTeamInActiveMustReturnsBadRequestIfChangesDoNotPersistInDb()
        {
            var teamName = "STORED_IN_ACTIVE_TEAM_NAME";
            var storedInActiveTeam = new Team() { TeamName = teamName, IsActive = true };
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(storedInActiveTeam);
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(0);
            var expectedMessage = $"Something went wrong. {teamName} is not inactive.";

            var actualResult = _teamsController.MakeTeamInActive(storedInActiveTeam.TeamName).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        #endregion

        #region MakeTeamActiveMethod

        [Test]
        public void MakeTeamActiveMustReturnsBadRequestIfThereIsNotTeamWithGivenNameInTheDb()
        {
            var teamName = "TEAM_NAME";
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(null as Team);
            var expectedMessage = $"{teamName} does not exist.";

            var actualResult = _teamsController.MakeTeamActive(teamName).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        [Test]
        public void MakeTeamActiveMustReturnsBadRequestIfTheTeamIsAlreadyActive()
        {
            var teamName = "TEAM_NAME";
            var storedTeam = new Team() { TeamName = teamName, IsActive = true };
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(storedTeam);
            var expectedMessage = $"{teamName} is already active";

            var actualResult = _teamsController.MakeTeamActive(teamName).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        [Test]
        public void MakeTeamActiveMustReturnsOkNormalScenario()
        {
            var teamName = "TEAM_NAME";
            var storedTeam = new Team() { TeamName = teamName, IsActive = false };
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(storedTeam);
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(1);
            var expectedMessage = $"{teamName} is now active.";

            var actualResult = _teamsController.MakeTeamActive(teamName).Result;

            Assert.IsInstanceOf<OkObjectResult>(actualResult);
            var actualRequestValue = (actualResult as OkObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        [Test]
        public void MakeTeamActiveMustReturnsBadRequestIfChangesIsNotPersisted()
        {
            var teamName = "TEAM_NAME";
            var storedTeam = new Team() { TeamName = teamName, IsActive = false };
            _teamRepository.Setup(rep => rep.GetByTeamName(teamName)).ReturnsAsync(storedTeam);
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(0);
            var expectedMessage = $"Something went wrong. {teamName} is not active.";

            var actualResult = _teamsController.MakeTeamActive(teamName).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        #endregion

        #region PostUserToTeamMethod

        [Test]
        public void PostUserToTeamMustReturnsBadRequestsIfThereIsNotSuchUser()
        {
            var invalidUserName = "INVALID_USER_NAME";
            _userRepository.Setup(rep => rep.GetByUsername(invalidUserName)).ReturnsAsync(null as User);
            var expectedAnswer = $"Unknown user: {invalidUserName}.";
            var userTeamDto = new TeamUserDto() { UserName = invalidUserName };

            var actualResult = _teamsController.PostUserToTeam(userTeamDto).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedAnswer, actualRequestValue);
        }

        [Test]
        public void PostUserToTeamMustReturnsBadRequestsIfThereIsNotSuchTeam()
        {
            var validUserName = "VALID_USER_NAME";
            var validUser = new User() { Username = validUserName };
            _userRepository.Setup(rep => rep.GetByUsername(validUserName)).ReturnsAsync(validUser);
            var invalidTeamName = "INVALID_TEAM_NAME";
            _teamRepository.Setup(rep => rep.GetByTeamName(invalidTeamName)).ReturnsAsync(null as Team);
            var expectedAnswer = $"Unknown team: {invalidTeamName}.";
            var userTeamDto = new TeamUserDto() { UserName = validUserName, TeamName = invalidTeamName };

            var actualResult = _teamsController.PostUserToTeam(userTeamDto).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedAnswer, actualRequestValue);
        }

        [Test]
        public void PostUserToTeamMustReturnsBadRequestIfTheUserIsAlreadyInTheTeam()
        {
            var validUsername = "VALID_USERNAME";
            var validTeamName = "VALID_TEAM_NAME";
            var testTeamId = 1;
            var validTeam = new Team() { TeamName = validTeamName, TeamId = testTeamId };
            var validUser = new User()
            {
                Username = validUsername,
                UsersTeams = new List<UserTeam>()
                {
                    new UserTeam(){TeamId = testTeamId }
                }
            };
            _teamRepository.Setup(rep => rep.GetByTeamName(validTeamName)).ReturnsAsync(validTeam);
            _userRepository.Setup(rep => rep.GetByUsername(validUsername)).ReturnsAsync(validUser);
            var expectedAnswer = $"User: {validUsername} already exist in team: {validTeamName}.";

            var answer = _teamsController.PostUserToTeam(
                new TeamUserDto() { TeamName = validTeamName, UserName = validUsername }).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(answer);
            var actualRequestValue = (answer as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedAnswer, actualRequestValue);
        }

        [Test]
        public void PostUserToTeamMustReturnsOkNormalScenario()
        {
            var validUsername = "VALID_USERNAME";
            var validTeamName = "VALID_TEAM_NAME";
            var testTeamId = 1;
            var testUserId = "TEST_SUBJECT_ID";
            var validTeam = new Team()
            {
                IsActive = true,
                TeamId = testTeamId,
                TeamName = validTeamName,
                UsersTeams = new List<UserTeam>()
            };
            var validUser = new User(){SubjectId = testUserId };
            _teamRepository.Setup(rep => rep.GetByTeamName(validTeamName)).ReturnsAsync(validTeam);
            _userRepository.Setup(rep => rep.GetByUsername(validUsername)).ReturnsAsync(validUser);
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(1);
            var expectedResult =
                $"User:{validUsername} was added to the team:{validTeamName} successfully!";

            var answer = _teamsController.PostUserToTeam(
                new TeamUserDto() { TeamName = validTeamName, UserName = validUsername }).Result;

            Assert.IsInstanceOf<OkObjectResult>(answer);
            var actualRequestValue = (answer as OkObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedResult, actualRequestValue);
            CollectionAssert.IsNotEmpty(validTeam.UsersTeams);
        }

        [Test]
        public void PostUserToTeamMustReturnsBadRequestIfChangesDoesNotPersistInTheDb()
        {
            var validUsername = "VALID_USERNAME";
            var validTeamName = "VALID_TEAM_NAME";
            var testTeamId = 1;
            var testUserId = "TEST_SUBJECT_ID";
            var validTeam = new Team()
            {
                IsActive = true,
                TeamId = testTeamId,
                TeamName = validTeamName,
                UsersTeams = new List<UserTeam>()
            };
            var validUser = new User() { SubjectId = testUserId };
            _teamRepository.Setup(rep => rep.GetByTeamName(validTeamName)).ReturnsAsync(validTeam);
            _userRepository.Setup(rep => rep.GetByUsername(validUsername)).ReturnsAsync(validUser);
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(0);
            var expectedMessage = $"Something went wrong. User: {validUsername} wasn't added to the team successfully.";

            var answer = _teamsController.PostUserToTeam(
                new TeamUserDto() { TeamName = validTeamName, UserName = validUsername }).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(answer);
            var actualRequestValue = (answer as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedMessage, actualRequestValue);
        }

        #endregion

        #region RemoveUserFromTeamMethod

        [Test]
        public void RemoveUserFromTeamMustReturnsBadRequestIfThereIsNotSuchTeamInDb()
        {
            var invalidTeamName = "INVALID_TEAM_NAME";
            _teamRepository.Setup(rep => rep.GetByTeamName(invalidTeamName)).ReturnsAsync(null as Team);
            var expectedAnswer = $"Unknown team: {invalidTeamName}.";

            var actualResult = _teamsController
                .RemoveUserFromTeam(new TeamUserDto() {TeamName = invalidTeamName}).Result;

            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedAnswer, actualRequestValue);
        }

        [Test]
        public void RemoveUserFromTeamMustReturnsBadRequestIfThereIsNotSuchUserInDb()
        {
            var validTeamName = "VALID_TEAM_NAME";
            _teamRepository
                .Setup(rep => rep.GetByTeamName(validTeamName))
                .ReturnsAsync(new Team() {TeamName = validTeamName});
            var invalidUsername = "INVALID_USER_NAME";
            _userRepository
                .Setup(rep => rep.GetByUsername(invalidUsername))
                .ReturnsAsync(null as User);
            var expectedAnswer = $"Unknown user: {invalidUsername}.";

            var actualResult = _teamsController
                .RemoveUserFromTeam(new TeamUserDto(){UserName = invalidUsername,TeamName = validTeamName}).Result;

            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedAnswer, actualRequestValue);
        }

        [Test]
        public void RemoveUserFromTeamMustReturnsOkNormalScenario()
        {
            var validUsername = "VALID_USERNAME";
            var validTeamName = "VALID_TEAM_NAME";
            var validUser = new User(){Username = validUsername};
            var validTeam = new Team()
            {
                TeamName = validTeamName,
                UsersTeams = new List<UserTeam>(){new UserTeam(){User = validUser}}
            };
            _teamRepository
                .Setup(rep => rep.GetByTeamName(validTeamName))
                .ReturnsAsync(validTeam);
            _userRepository
                .Setup(rep => rep.GetByUsername(validUsername))
                .ReturnsAsync(validUser);
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(1);
            var expectedAnswer = $"The user: {validUsername} was removed from team: {validTeamName} successfully.";

            var actualResult = _teamsController
                .RemoveUserFromTeam(new TeamUserDto() { UserName = validUsername, TeamName = validTeamName }).Result;

            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<OkObjectResult>(actualResult);
            var actualRequestValue = (actualResult as OkObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedAnswer, actualRequestValue);
        }

        [Test]
        public void RemoveUserFromTeamMustReturnsBadRequestIfChangesWasNotPersistedInTheDb()
        {
            var validUsername = "VALID_USERNAME";
            var validTeamName = "VALID_TEAM_NAME";
            var validUser = new User() { Username = validUsername };
            var validTeam = new Team()
            {
                TeamName = validTeamName,
                UsersTeams = new List<UserTeam>() { new UserTeam() { User = validUser } }
            };
            _teamRepository
                .Setup(rep => rep.GetByTeamName(validTeamName))
                .ReturnsAsync(validTeam);
            _userRepository
                .Setup(rep => rep.GetByUsername(validUsername))
                .ReturnsAsync(validUser);
            _teamRepository.Setup(rep => rep.SaveChangesAsync()).ReturnsAsync(0);
            var expectedAnswer = $"The user: {validUsername} was not removed from team: {validTeamName} successfully.";

            var actualResult = _teamsController
                .RemoveUserFromTeam(new TeamUserDto() { UserName = validUsername, TeamName = validTeamName }).Result;

            Assert.IsNotNull(actualResult);
            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
            var actualRequestValue = (actualResult as BadRequestObjectResult)?.Value;
            Assert.NotNull(actualRequestValue);
            Assert.AreEqual(expectedAnswer, actualRequestValue);
        }

        #endregion

    }
}
