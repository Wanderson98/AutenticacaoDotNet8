using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //Using in save  user
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CreateUser")]
        public async Task<IActionResult> CreateUser([FromBodyAttribute] AddUserRequest input)
        {
            if (string.IsNullOrWhiteSpace(input.email) || string.IsNullOrWhiteSpace(input.password))
                return BadRequest("Some information is missing.");


            var user = new ApplicationUser
            {               
                UserName = input.email,
                Email = input.email,
                RG = input.rg
            };
            var result = await _userManager.CreateAsync(user, input.password);

            if(result.Errors.Any())
            {
                return BadRequest(result.Errors);
            }

            //Generation of email confirmation code
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //return email
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result2 = await _userManager.ConfirmEmailAsync(user, code);

            if(result2.Succeeded)
            {
                return Ok("User created successfully.");
            }
            else
            {
                return BadRequest("Error creating user.");
            }
        }
    }
}
