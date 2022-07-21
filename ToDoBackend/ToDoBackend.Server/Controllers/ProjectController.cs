using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.BLL.Interfaces;
using ToDoBackend.BLL.Models;

namespace ToDoBackend.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly UserManager<IdentityUser> _userManager;

        public ProjectController(
            IProjectService projectService,
            UserManager<IdentityUser> userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }
        
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("{projectId}")]
        public async Task<ActionResult<ProjectModel>> GetByIdAsync(int projectId)
        {
            return await _projectService.GetByIdAsync(projectId);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] ProjectModel model)
        {
            await _projectService.AddAsync(model, _userManager.GetUserId(User));
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("rename")]
        public async Task<ActionResult> RenameProjectAsync([FromBody] ProjectModel modelToRename)
        {
            if (!(String.IsNullOrEmpty(modelToRename.Name) || String.IsNullOrWhiteSpace(modelToRename.Name)))
            {
                await _projectService.UpdateAsync(modelToRename);
                return Ok();
            }

            return BadRequest();
        }
        
        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _projectService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("byuser")]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetProjectsByUserAsync([FromBody] UserIdModel userId)
        {
            return Ok(await _projectService.GetProjectsByUserAsync(userId.UserId));
        }

        [HttpGet]
        [Route("isadmin/{projectId}")]
        public async Task<ActionResult<bool>> IsUserAdminAsync(int projectId, [FromBody] UserIdModel userId)
        {
            return Ok(await _projectService.IsUserAdminAsync(projectId, userId.UserId));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("setadmin/{projectId}")]
        public async Task<ActionResult> SetUserAsAdminAsync(int projectId, [FromBody] UserIdModel userId)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId.UserId);
            if (user != null && await _userManager.IsInRoleAsync(user, "admin"))
            {
                await _projectService.SetUserAsAdminAsync(projectId, userId.UserId);
                return Ok();
            }

            return BadRequest();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("adduser/{projectId}")]
        public async Task<ActionResult> AddUserToProjectAsync(int projectId, [FromBody] UserIdModel userId)
        {
            await _projectService.AddUserToProjectAsync(projectId, userId.UserId);
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("removeuser/{projectId}")]
        public async Task<ActionResult> RemoveUserFromProjectAsync(int projectId, [FromBody] UserIdModel userId)
        {
            await _projectService.RemoveUserFromProjectAsync(projectId, userId.UserId);
            return Ok();
        }
    }
}