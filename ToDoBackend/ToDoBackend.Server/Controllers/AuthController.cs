using System;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Auth.JWT;
using ToDoBackend.Auth.Models;
using ToDoBackend.BLL.Interfaces;

namespace ToDoBackend.Server.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMailService _mailService;

        public AuthController
        (UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] Register request)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser()
                {
                    Email = request.EMail,
                    UserName = request.UserName
                };
                
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    try
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    
                        var confirmationLink = Url.Action(
                            "ConfirmEMail", 
                            "Auth",
                            new { user.Email, token },
                            Request.Scheme);

                        var message = $"Confirmation email link\n{confirmationLink}";

                        if (_mailService.SendAsync(message))
                        {
                            return Ok();
                        }
                    }
                    finally
                    {
                        await _userManager.DeleteAsync(user);
                    }
                }
            }
            
            return BadRequest();
        }

        [HttpGet]
        [Route("confirm-email")]
        public async Task<ActionResult> ConfirmEMail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                string jwt = Token.GetToken(user);
                return new JsonResult(jwt);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] Login request)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(request.EMail);

                if (user != null)
                {
                    var result = _signInManager
                        .PasswordSignInAsync(user, request.Password, false, false);

                    if (result.IsCompletedSuccessfully)
                    {
                        string token = Token.GetToken(user);
                        return new JsonResult(token);
                    }
                }
            }
            ModelState.TryAddModelError("", "Invalid credentials");

            return new UnauthorizedResult();
        }

        [HttpDelete]
        [Route("logout")]
        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}