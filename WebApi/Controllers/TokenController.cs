using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Token;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        //Using in validation to user
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TokenController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost("/api/CreateToken")]
        [Produces("application/json")]
        public async Task <IActionResult> CreateToken([FromBodyAttribute] InputLoginRequest input)
        {
            if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
                return Unauthorized();


            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = new TokenJwtBuilder()
                    .AddSecurityKey(JWTSecurityKey.Create("Secret-Key-12345-wmandradedev"))
                    .AddSubject("WmandradeDev")
                    .AddIssuer("wmandradedev.Security.Bearer")
                    .AddAudience("wmandradedev.Security.Bearer")
                    .AddExpiry(1)
                    .Builder();

                return Ok(token.value);
            }              
            else return Unauthorized();
        }
    }
}
