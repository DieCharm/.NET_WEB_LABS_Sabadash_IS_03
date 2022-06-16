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
        private IProjectService _projectService;
        private readonly UserManager<IdentityUser> _userManager;

        public ProjectController(
            IProjectService projectService,
            UserManager<IdentityUser> userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }
        
        //admin
        [HttpGet]
        [Route("{projectId}")]
        public async Task<ActionResult<ProjectModel>> GetByIdAsync(int projectId)
        {
            return await _projectService.GetByIdAsync(projectId);
        }

        //admin
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] ProjectModel model)
        {
            await _projectService.AddAsync(model, _userManager.GetUserId(User));
            return Ok();
        }

        //admin
        [HttpPost]
        [Route("rename")]
        public async Task<ActionResult> RenameProjectAsync([FromBody] ProjectModel modelToRename)
        {
            if (await _projectService.GetByIdAsync(modelToRename.Id) != null &&
                (await _projectService.GetByIdAsync(modelToRename.Id)).Name != modelToRename.Name &&
                !(String.IsNullOrEmpty(modelToRename.Name) || String.IsNullOrWhiteSpace(modelToRename.Name)))
            {
                await _projectService.UpdateAsync(modelToRename);
            }

            return BadRequest();
        }
        
        //admin
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _projectService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetProjectsByUserAsync([FromBody] string userId)
        {
            return Ok(await _projectService.GetProjectsByUserAsync(userId));
        }
        
        //?? GetUserIdsByProjectAsync ??
        //?? GetAdminIdsByProjectAsync ??

        [HttpGet]
        [Route("isadmin/{projectId}")]
        public async Task<ActionResult<bool>> IsUserAdminAsync(int projectId, [FromBody] string userId)
        {
            return Ok(await _projectService.IsUserAdminAsync(projectId, userId));
        }

        //admin
        [HttpPost]
        [Route("setadmin{projectId}")]
        public async Task<ActionResult> SetUserAsAdminAsync(int projectId, [FromBody] string userId)
        {
            await _projectService.SetUserAsAdminAsync(projectId, userId);
            return Ok();
        }

        //admin
        [HttpDelete]
        [Route("removeuser/{projectId}")]
        public async Task<ActionResult> RemoveUserFromProjectAsync(int projectId, [FromBody] string userId)
        {
            await _projectService.RemoveUserFromProjectAsync(projectId, userId);
            return Ok();
        }
    }
}