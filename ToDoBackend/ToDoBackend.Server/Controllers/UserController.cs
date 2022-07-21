using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Auth.Interfaces;
using ToDoBackend.Auth.Services;
using ToDoBackend.BLL.Interfaces;

namespace ToDoBackend.Server.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

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

        [HttpGet]
        [Route("project-users/{projectId}")]
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
        
        [HttpGet]
        [Route("project-admins/{projectId}")]
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
        
        [HttpPost]
        [Route("make-admin/{userId}")]
        public async Task<ActionResult<bool>> MakeUserAdminAsync(string userId)
        {
            IdentityUser toBecomeAdmin = await _userService.GetUserByIdAsync(userId);
            if (toBecomeAdmin != null)
            {
                return await _userService.MakeUserAdminAsync(toBecomeAdmin);
            }

            return false;
        }
    }
}