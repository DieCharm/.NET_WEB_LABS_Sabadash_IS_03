using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToDoBackend.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController: ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            if (User.Identity != null)
            {
                return Ok(User.Identity.Name);
            }
            
            return BadRequest();
        }
    }
}