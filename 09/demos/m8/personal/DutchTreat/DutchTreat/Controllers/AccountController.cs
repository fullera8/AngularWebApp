using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<StoreUser> signInManager;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signInManager)
        {
            this.logger = logger;
            this.signInManager = signInManager;
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
    }
}
