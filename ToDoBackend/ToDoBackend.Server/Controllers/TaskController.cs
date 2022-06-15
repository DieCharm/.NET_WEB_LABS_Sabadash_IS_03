using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ToDoBackend.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController: ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public TaskController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            if (User.Identity != null)
            {
                return Ok(_userManager.GetUserId(User));
            }
            
            return BadRequest();
        }
    }
}