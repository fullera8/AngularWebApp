using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        //Homepage/Default Page
        public IActionResult Index()
        {
            //Dev error testing only
            //throw new InvalidOperationException();
            return View();
        }

        //Contact Us Page
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            //throw new InvalidOperationException("Error Page");
            return View();
        }

        //when page posts back, return itself
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Data is valid
            }
            else 
            { 
                //Data in error
            }
            return View();
        }

        //About Us Page
        [HttpGet("about")]
        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }
    }
}
