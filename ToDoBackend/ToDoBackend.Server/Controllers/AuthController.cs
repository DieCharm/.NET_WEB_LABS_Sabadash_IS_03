using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ToDoBackend.Auth.Identity;
using ToDoBackend.Server.JWT;
using ToDoBackend.Auth.Models;

namespace ToDoBackend.Server.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController
        (UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] Login request)
        {
            if (ModelState.IsValid)
            {
                IdentityUser newUser = new IdentityUser()
                {
                    UserName = request.UserName
                };
                var result = await _userManager.CreateAsync(newUser, request.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, false);
                    string token = Token.GetToken(newUser);
                    return new JsonResult(token);
                }
            }

            return new UnauthorizedResult();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] Login request)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync
                    (request.UserName, request.Password, false, false);

                if (result.Succeeded)
                {
                    var userToLogIn = await _userManager.FindByNameAsync(request.UserName);
                    string token = Token.GetToken(userToLogIn);
                    return new JsonResult(token);
                }
            }

            return new UnauthorizedResult();
        }
    }
}