using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class HomeTestController : Controller
    {
        public IActionResult Index()
        {

            ViewData["Message"] = "Hello world!";
            ViewData["Message2"] = "Test!";
            ViewData["Message3"] = "SyperTest!";
            ViewData["Message4"] = "MegaTest!";
            ViewData["Message5"] = "TopTest!";

            return View("Index");

        }
    }
}
