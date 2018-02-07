using System.Threading.Tasks;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LVMiniAdminApi.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/teams")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminTeamsController : BaseController
    {
        public AdminTeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _teamRepository.GetAll();
            return Ok(teams);
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Team team)
        {
            if (ModelState.IsValid)
            {
                await _teamRepository.Insert(team);
                var commitsCount = await _teamRepository.SaveChangesAsync();
                if (commitsCount > 0)
                {
                    return Ok("Team created");
                }
                return BadRequest("Something went wrong. Team was not created.");
            }
            return BadRequest("Invalid model state");
        }
    }
}