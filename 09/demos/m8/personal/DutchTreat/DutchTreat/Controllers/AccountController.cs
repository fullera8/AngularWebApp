//using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<StoreUser> signInManager;
        private readonly UserManager<StoreUser> userManager;
        private readonly IConfiguration _config;

        public AccountController(
            ILogger<AccountController> logger, 
            SignInManager<StoreUser> signInManager,
            UserManager<StoreUser> userManager,
            IConfiguration config)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            _config = config;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }

            return View();
        }

        //Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Confirm Login
                var result = await signInManager.PasswordSignInAsync
                    (
                        model.Username, 
                        model.Password, 
                        model.RememberMe, 
                        false
                    );
                //If Login sucessful, either redirect to the url requested or default to shop page
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                        return Redirect(Request.Query["ReturnUrl"].First());
                    else
                       return RedirectToAction("Shop", "App");                    
                }
            }

            //Login failed generic
            ModelState.AddModelError("", "Failed to login");

            //postback to login page on failed login
            return View();
        }

        //Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        //Create Authentication Token
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            //Check to make sure all required fields are being included in the request
            if (ModelState.IsValid)
            {
                //Look for the username
                var user = await this.userManager.FindByNameAsync(model.Username);
                if(user != null)
                {
                    //confirm the password for the username is correct
                    var result = await this.signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {
                        //Create token
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email), //Get Email
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //ID for request
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName) //Username acts as primary key and is used for lookup
                        };

                        //Create an key, encrypt the key
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        //create the final token
                        var token = new JwtSecurityToken(
                            _config["Tokens:Issuer"],
                            _config["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: cred
                            );

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created("", token);
                    }
                }
                    
            }

            //Cannot authorize the token
            return BadRequest();
        }
    }
}
