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
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userMgr;
        private RoleManager<AppRole> _roleMgr;

        public LoginController(SignInManager<User> signin, UserManager<User> usermanager, RoleManager<AppRole> r)
        {
            _signInManager = signin;
            _userMgr = usermanager;
            _roleMgr = r;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<StatusCodeResult> Login([FromBody] LoginData data)
        {
            User dbUser = UserCollection.FindByUsername(data.Name);
            if (dbUser == null)
            {
                return NotFound();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(dbUser, data.Password, false, true);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpGet("logout")]
        public async Task<StatusCodeResult> Logout()
        {
            User user = await _userMgr.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }

            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}
