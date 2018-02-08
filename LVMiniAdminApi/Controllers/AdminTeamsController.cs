using System.Threading.Tasks;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using IdentityServer4.Events;
using LVMiniAdminApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team = LVMiniAdminApi.Models.TeamModels.Team;

namespace LVMiniAdminApi.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/teams")]
    //[Authorize(Policy = "AdminOnly")]
    public class AdminTeamsController : BaseController
    {
        public AdminTeamsController(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _teamRepository.GetAll();
            return Ok(teams);
        }

        [HttpGet]
        [Route("{currentTeamName}")]
        public async Task<IActionResult> GetCurrent(string currentTeamName)
        {
            var currentTeam = await _teamRepository.GetByTeamName(currentTeamName);
            if (currentTeam ==  null)
            {
                return BadRequest("Invalid team name.");
            }
            return Ok(currentTeam);
        }


        [HttpPost]
        public async Task<IActionResult> PostTeam([FromBody]Team team)
        {
            if (ModelState.IsValid)
            {
                Data.Service.Core.Entities.Team current = new Data.Service.Core.Entities.Team() { IsActive = team.IsActive, TeamName = team.TeamName };
                await _teamRepository.Insert(current);
                var commitsCount = await _teamRepository.SaveChangesAsync();
                if (commitsCount > 0)
                {
                    return Ok("Team created");
                }
                return BadRequest("Something went wrong. Team was not created.");
            }
            return BadRequest("Invalid model state");
        }

        [HttpDelete]
        public async Task<IActionResult> MakeTeamInActive([FromBody]string teamName)
        {
            var team = await _teamRepository.GetByTeamName(teamName);
            if (team == null)
            {
                return BadRequest($"{teamName} does not exist.");
            }
            if (!team.IsActive)
            {
                return BadRequest($"{teamName} is already inactive");
            }
            team.IsActive = false;
            var countOfCommits = await _teamRepository.SaveChangesAsync();
            if (countOfCommits > 0)
            {
                return Ok($"{teamName} is now inactive.");
            }
            return BadRequest($"Something went wrong. {teamName} is not inactive.");
        }

        [HttpPatch]
        public async Task<IActionResult> MakeTeamActive([FromBody] string teamName)
        {
            var team = await _teamRepository.GetByTeamName(teamName);
            if (team == null)
            {
                return BadRequest($"{teamName} does not exist.");
            }
            if (team.IsActive)
            {
                return BadRequest($"{teamName} is already active");
            }
            team.IsActive = true;
            var countOfCommits = await _teamRepository.SaveChangesAsync();
            if (countOfCommits > 0)
            {
                return Ok($"{teamName} is now active.");
            }
            return BadRequest($"Something went wrong. {teamName} is not active.");
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> PostUserToTeam([FromBody]TeamUser teamUser)
        {
            var user = await _userRepository.GetByUsername(teamUser.UserName);
            if (user == null)
            {
                return BadRequest("Unknown user.");
            }
            var team = await _teamRepository.GetByTeamName(teamUser.TeamName);
            if (team == null)
            {
                return BadRequest("Unknown team.");
            }
            foreach (var userUsersTeam in user.UsersTeams)
            {
                if (userUsersTeam.TeamId == team.TeamId)
                {
                    return BadRequest($"User already exist in \"{team.TeamName}\" team.");
                }
            }
            team.UsersTeams.Add(new UserTeam(){UserId = user.SubjectId});
            var coutOfCommits = await _teamRepository.SaveChangesAsync();
            if (coutOfCommits > 0)
            {
                return Ok($"User:{teamUser.UserName} was added to the team:{teamUser.TeamName} seccessfully!");
            }
            return BadRequest("Something went wrong. User wasn't added to the team seccessfully.");
        }

        [HttpDelete]
        [Route("user")]
        public async Task<IActionResult> RemoveUserFromTeam([FromBody]TeamUser teamUser)
        {
            var team = await _teamRepository.GetByTeamName(teamUser.TeamName);
            if (team == null)
            {
                return BadRequest("Unknown team.");
            }
            var user = await _userRepository.GetByUsername(teamUser.UserName);
            if (user == null)
            {
                return BadRequest("Unknown user.");
            }
            var removed = false;
            foreach (var middleReccord in team.UsersTeams)
            {
                if (middleReccord.User.Username == user.Username)
                {
                    team.UsersTeams.Remove(middleReccord);
                    removed = true;
                    break;
                }
            }
            var countOfCommits = await _teamRepository.SaveChangesAsync();
            if (countOfCommits > 0 && removed)
            {
                return Ok($"The user: {user.Username} was removed from team: {team.TeamName} successfully.");
            }
            return BadRequest($"The user: {user.Username} was not removed from team: {team.TeamName} successfully.");
        }
    }
}