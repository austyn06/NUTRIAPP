using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriApp.Controllers.Models;
using NutriApp;

namespace NutriApp.Controllers;

[Route("api/Teams")]
[ApiController]
[Authorize]
public class TeamsApiController : ControllerBase
{
    private readonly App _app;
    
    public TeamsApiController(App app)
    {
        _app = app;
    }
    
    // POST api/Teams
    [HttpPost]
    public async Task<ActionResult<TeamModel>> CreateTeam(TeamModel teamModel)
    {
        return teamModel;
    }
    
    // POST api/Teams/invite/{username}
    [HttpPost("invite/{username}")]
    public async Task<ActionResult<TeamInviteModel>> InviteUser(string username)
    {
        return new TeamInviteModel { Code = "nutricode" };
    }
    
    // PUT api/Teams/invite/accept/{inviteCode}
    [HttpPut("invite/accept/{inviteCode}")]
    public async Task<ActionResult<TeamModel>> AcceptInvite(string inviteCode)
    {
        return new TeamModel { Name = "NutriTeam" };
    }
    
    // PUT api/Teams/leave
    [HttpPut("leave")]
    public async Task<IActionResult> LeaveTeam()
    {
        return Ok();
    }
    
    // GET api/Teams/workouts/{username}
    [HttpGet("workouts/{username}")]
    public async Task<ActionResult<IEnumerable<EntryModel<Models.WorkoutModel>>>> GetWorkouts(string username)
    {
        // Some dummy workout structs
        EntryModel<Models.WorkoutModel>[] workouts =
        {
            new()
                { Value = new() { Name = "Morning Jog", Minutes = 60, Intensity = 10 } },
            new()
                { Value = new() { Name = "Afternoon Jog", Minutes = 60, Intensity = 10 } },
            new()
                { Value = new() { Name = "Evening Jog", Minutes = 60, Intensity = 10 } }
        };
        
        return workouts;
    }
    
    // POST api/Teams/challenge
    [HttpPost("challenge")]
    public async Task<IActionResult> CreateChallenge()
    {
        return Ok();
    }
    
    // GET api/Teams/challenge
    [HttpGet("challenge")]
    public async Task<ActionResult<List<UserProfileModel>>> GetChallengeRanking()
    {
        // Some dummy user profile structs
        UserProfileModel[] users =
        {
            new() { Name = "Dan Donchuk" },
            new() { Name = "Raynard Miot" },
            new() { Name = "Danny Gramowski" },
            new() { Name = "Austyn Wright" },
            new() { Name = "Jack Lindsey-Noble" }
        };
        
        return users.ToList();
    }
}