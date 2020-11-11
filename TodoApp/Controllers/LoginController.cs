using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TodoApp.Data.Collections;
using TodoApp.Data.Model;
using TodoApp.Security;

namespace TodoApp.Controllers
{
    public class LoginData
    {
        [JsonPropertyName("username")]
        public string Name { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class LoginResult
    {
        [JsonPropertyName("username")]
        public string Name { get; set; }

        [JsonPropertyName("fullname")]
        public string FullName { get; set; }
    }

    public class RegisterData
    {
        [JsonPropertyName("username")]
        public string Name { get; set; }

        [JsonPropertyName("fullname")]
        public string FullName { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    [ApiController]
    public class LoginController : ControllerBase
    {
        private SignInManager<User> signInManager;
        private UserManager<User> userMgr;
        private RoleManager<AppRole> roleMgr;

        public LoginController(SignInManager<User> signin, UserManager<User> usermanager, RoleManager<AppRole> r)
        {
            signInManager = signin;
            userMgr = usermanager;
            roleMgr = r;
        }

        [AllowAnonymous]
        [HttpPost("/user/login")]
        public async Task<IActionResult> Login([FromBody] LoginData data)
        {
            User dbUser = UserCollection.FindByUsername(data.Name);
            if (dbUser == null)
            {
                return NotFound();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(dbUser, data.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(new LoginResult()
                {
                    Name = dbUser.Username,
                    FullName = dbUser.FullName
                });
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("/user/register")]
        public async Task<IActionResult> Register([FromBody] RegisterData data)
        {
            User dbUser = UserCollection.FindByUsername(data.Name);
            if (dbUser != null)
            {
                return Forbid();
            }

            User newUser = new User()
            {
                Username = data.Name,
                FullName = data.FullName,
                Roles = new string[] { "USER" },
                PasswordHash = null
            };

            IdentityResult result = await userMgr.CreateAsync(newUser, data.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("/user/logout")]
        public async Task<StatusCodeResult> Logout()
        {
            User user = await userMgr.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }

            await signInManager.SignOutAsync();

            return Ok();
        }
    }
}
