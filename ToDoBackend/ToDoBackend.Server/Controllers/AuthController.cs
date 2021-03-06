using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Auth.Interfaces;
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
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IMailService mailService,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _tokenService = tokenService;
        }
        
        /// <summary>
        /// Is called to create a new user and send confirmation email
        /// </summary>
        /// <param name="request">Register model with user info</param>
        /// <returns></returns>
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
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    
                    var confirmationLink = Url.Action(
                        "ConfirmEMail", 
                        "Auth",
                        new { user.Email, token },
                        Request.Scheme);

                    var message = $"Confirmation email link\n{confirmationLink}";

                    if (_mailService.SendAsync(request.EMail, request.UserName, message))
                    {
                        return Ok();
                    }
                    
                    await _userManager.DeleteAsync(user);
                }
            }
            
            return BadRequest();
        }

        /// <summary>
        /// Confirms an email
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="token">Confirmation token generated in register method</param>
        /// <returns>JWT token</returns>
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
                string jwt = _tokenService.GetToken(user);
                return new JsonResult(jwt);
            }

            return BadRequest();
        }

        /// <summary>
        /// Is called to sign in registered user
        /// </summary>
        /// <param name="request">Email and password</param>
        /// <returns>JWT token</returns>
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
                        string token = _tokenService.GetToken(user);
                        return new JsonResult(token);
                    }
                }
            }

            return new UnauthorizedResult();
        }

        /// <summary>
        /// Is called to log out and deactivate JWT token
        /// </summary>
        [Authorize]
        [HttpDelete]
        [Route("logout")]
        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
            //cancel token mb
        }
    }
}