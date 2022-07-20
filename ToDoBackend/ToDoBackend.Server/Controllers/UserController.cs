using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Auth.Services;
using ToDoBackend.BLL.Interfaces;

namespace ToDoBackend.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private IProjectService _projectService;

        public UserController(
            UserService userService,
            IProjectService projectService)
        {
            _userService = userService;
            _projectService = projectService;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<IdentityUser>> GetUserByIdAsync(string userId)
        {
            return Ok(await _userService.GetUserByIdAsync(userId));
        }

        //admin
        [HttpGet]
        [Route("projectusers/{projectId}")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetUsersInProjectAsync(int projectId)
        {
            IEnumerable<string> indexes = await _projectService.GetUserIdsByProjectAsync(projectId);
            List<IdentityUser> result = new List<IdentityUser>();
            foreach (var index in indexes)
            {
                result.Add(await _userService.GetUserByIdAsync(index));
            }

            return result;
        }
        
        //admin
        [HttpGet]
        [Route("projectadmins/{projectId}")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetAdminsInProjectAsync(int projectId)
        {
            IEnumerable<string> indexes = await _projectService.GetAdminIdsByProjectAsync(projectId);
            List<IdentityUser> result = new List<IdentityUser>();
            foreach (var index in indexes)
            {
                result.Add(await _userService.GetUserByIdAsync(index));
            }

            return result;
        }
        
        //give admin credentials to user
    }
}