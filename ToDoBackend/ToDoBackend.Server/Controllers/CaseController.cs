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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CaseController: ControllerBase
    {
        private ICaseService _caseService;
        private readonly UserManager<IdentityUser> _userManager;

        public CaseController(
            ICaseService caseService,
            UserManager<IdentityUser> userManager)
        {
            _caseService = caseService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("{caseId}")]
        public async Task<ActionResult<CaseModel>> GetByIdAsync(int caseId)
        {
            return Ok(await _caseService.GetByIdAsync(caseId));
        }

        //admin
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CaseModel model)
        {
            await _caseService.AddAsync(model);
            return Ok();
        }

        //admin
        [HttpPost]
        [Route("name/{id}/{name}")]
        public async Task<ActionResult> ChangeNameAsync(int id, string name)
        {
            await _caseService.ChangeNameAsync(id, name);
            return Ok();
        }
        
        //admin
        [HttpPost]
        [Route("description/{id}/{description}")]
        public async Task<ActionResult> ChangeDescriptionAsync(int id, string description)
        {
            await _caseService.ChangeDescriptionAsync(id, description);
            return Ok();
        }
        
        //admin
        [HttpPost]
        [Route("deadline/{id}")]
        public async Task<ActionResult> ChangeDeadlineAsync(int id, [FromBody] DateTime deadline)
        {
            await _caseService.ChangeDeadlineAsync(id, deadline);
            return Ok();
        }
        
        //admin
        [HttpPost]
        [Route("priority/{id}/{priority}")]
        public async Task<ActionResult> ChangePriorityAsync(int id, int priority)
        {
            await _caseService.ChangePriorityAsync(id, priority);
            return Ok();
        }
        
        [HttpPost]
        [Route("status/{id}/{status}")]
        public async Task<ActionResult> ChangeStatusAsync(int id, int status)
        {
            await _caseService.ChangeStatusAsync(id, status);
            return Ok();
        }
        
        //admin
        [HttpPost]
        [Route("setuser/{id}/{userId}")]
        public async Task<ActionResult> SetUserAsync(int id, string userId)
        {
            await _caseService.SetUserAsync(id, userId);
            return Ok();
        }
        
        //admin
        [HttpPost]
        [Route("removeuser/{id}")]
        public async Task<ActionResult> RemoveUserAsync(int id)
        {
            await _caseService.RemoveUserAsync(id);
            return Ok();
        }

        //admin
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _caseService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("alltasks")]
        public async Task<ActionResult<IEnumerable<CaseModel>>> GetTasksByUserAsync([FromBody] string userId)
        {
            return Ok(await _caseService.GetTasksByUserAsync(userId));
        }

        [HttpGet]
        [Route("{projectId}")]
        public async Task<ActionResult<IEnumerable<CaseModel>>> GetTasksByUserInProjectAsync
            ([FromBody] string userId, int projectId)
        {
            return Ok(await _caseService.GetTasksByUserInProjectAsync(userId, projectId));
        }
    }
}