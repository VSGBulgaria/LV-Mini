using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using LVMiniAdminApi.Models;
using LVMiniAdminApi.Models.TeamModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LVMiniAdminApi.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/teams")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminTeamsController : BaseController
    {
        private readonly IMapper _mapper;

        public AdminTeamsController(ITeamRepository teamRepository, IUserRepository userRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _teamRepository.GetAll();
            var mappedTeams = _mapper.Map<IEnumerable<Team>, IEnumerable<TeamDto>>(teams);
            return Ok(mappedTeams);
        }

        [HttpGet]
        [Route("{currentTeamName}")]
        public async Task<IActionResult> GetCurrent(string currentTeamName)
        {
            var currentTeam = await _teamRepository.GetByTeamName(currentTeamName);
            var mappedTeam = _mapper.Map<TeamDto>(currentTeam);
            if (currentTeam == null)
            {
                return BadRequest("Invalid team name.");
            }
            return Ok(mappedTeam);
        }


        [HttpPost]
        public async Task<IActionResult> PostTeam([FromBody]TeamDto teamDto)
        {
            if (ModelState.IsValid && teamDto.TeamName != null)
            {
                Team current = new Team() { IsActive = teamDto.IsActive, TeamName = teamDto.TeamName };
                var suchTeamAlreadyExist = await _teamRepository.GetByTeamName(current.TeamName);
                if (suchTeamAlreadyExist == null)
                {
                    if (teamDto.Users != null && teamDto.Users.Count > 0)
                    {
                        var uniqueUsers = teamDto.Users.Distinct();
                        foreach (var teamDtoUser in uniqueUsers)
                        {
                            var currentUser = await _userRepository.GetByUsername(teamDtoUser.Username);
                            if (currentUser == null)
                            {
                                return BadRequest($"Can not insert unexistable user: {teamDtoUser.Username} in team: {teamDto.TeamName}.");
                            }

                            current.UsersTeams.Add(new UserTeam() { UserId = currentUser.SubjectId });
                        }
                    }
                    await _teamRepository.Insert(current);
                    var commitsCount = await _teamRepository.SaveChangesAsync();
                    if (commitsCount > 0)
                    {
                        return Ok("Team created.");
                    }
                    return BadRequest("Something went wrong. Team was not created.");
                }
                return BadRequest($"Team with name {current.TeamName} is already exists.");
            }
            return BadRequest("Invalid model state");
        }

        [HttpPut]
        [Route("inactive")]
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

        [HttpPut]
        [Route("active")]
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

        [HttpPut]
        [Route("add/user")]
        public async Task<IActionResult> PostUserToTeam([FromBody]TeamUserDto teamUserDto)
        {
            var user = await _userRepository.GetByUsername(teamUserDto.UserName);
            if (user == null)
            {
                return BadRequest($"Unknown user: {teamUserDto.UserName}.");
            }
            var team = await _teamRepository.GetByTeamName(teamUserDto.TeamName);
            if (team == null)
            {
                return BadRequest($"Unknown team: {teamUserDto.TeamName}.");
            }
            foreach (var userUsersTeam in user.UsersTeams)
            {
                if (userUsersTeam.TeamId == team.TeamId)
                {
                    return BadRequest($"User: {teamUserDto.UserName} already exist in team: {teamUserDto.TeamName}.");
                }
            }
            team.UsersTeams.Add(new UserTeam() { UserId = user.SubjectId });
            var countOfCommits = await _teamRepository.SaveChangesAsync();
            if (countOfCommits > 0)
            {
                return Ok($"User:{teamUserDto.UserName} was added to the team:{teamUserDto.TeamName} successfully!");
            }
            return BadRequest(
                $"Something went wrong. User: {teamUserDto.UserName} wasn't added to the team successfully.");
        }

        [HttpPut]
        [Route("remove/user")]
        public async Task<IActionResult> RemoveUserFromTeam([FromBody]TeamUserDto teamUserDto)
        {
            var team = await _teamRepository.GetByTeamName(teamUserDto.TeamName);
            if (team == null)
            {
                return BadRequest($"Unknown team: {teamUserDto.TeamName}.");
            }
            var user = await _userRepository.GetByUsername(teamUserDto.UserName);
            if (user == null)
            {
                return BadRequest($"Unknown user: {teamUserDto.UserName}.");
            }
            var removed = false;
            foreach (var middleRecord in team.UsersTeams)
            {
                if (middleRecord.User.Username == user.Username)
                {
                    team.UsersTeams.Remove(middleRecord);
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