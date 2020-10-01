using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        Context db;
        public HomeController(Context context)
        {
            db = context;
        }

       

        public IActionResult Index()
        {
            ViewBag.IdUser = HttpContext.Session.GetInt32("UserId");
            ViewBag.Login = HttpContext.Session.GetString("Login");

            return View();
        }


        public IActionResult Login()
        {
           
            User user1 = new User
            {
               
                Login = "Донской",
                Password = "1"
            };
            User user2 = new User
            {
              
                Login = "Стерхова",
                Password = "1"
            };
            User user3 = new User
            {
              
                Login = "Антонов",
                Password = "1"
            };

           

            ViewBag.isError = false;
            return View();
        }

        public ActionResult IncorrectLogin()
        {
            ViewBag.isError = true;


            return View("Login");
          
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
           
            User user = db.Users.Where(w => w.Login == login && w.Password == password).FirstOrDefault();

            if(user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.IdUser);

                HttpContext.Session.SetString("Login", user.Login);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("IncorrectLogin", "Home");
            }
       
        }

        public ActionResult Exit()
        {
            
            return RedirectToAction("Login", "Home");
        }


        public IActionResult Privacy()
        {
            return View();
        }
             

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
