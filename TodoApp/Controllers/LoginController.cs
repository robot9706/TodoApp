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
        [JsonPropertyName("name")]
        public string Name { get; set; }

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
        public async Task<StatusCodeResult> Login([FromBody] LoginData data)
        {
            User dbUser = UserCollection.FindByUsername(data.Name);
            if (dbUser == null)
            {
                return NotFound();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(dbUser, data.Password, false, true);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
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
