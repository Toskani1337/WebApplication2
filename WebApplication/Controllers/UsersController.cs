using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class UsersController : Controller
    {

        Context db;
        public UsersController(Context context)
        {
            db = context;
        }

        public IActionResult Users()
        {
            ViewBag.IdUser = HttpContext.Session.GetInt32("UserId");
            ViewBag.Login = HttpContext.Session.GetString("Login");

            return View();
        }

        public JsonResult GetData()
        {



            var data = db.Users.Where(w=>w.Login != "" && w.IdUser != 1&& w.Password!="").ToList();

            List<UserMnemo> UserMnemos = new List<UserMnemo>();
            foreach (var User in data)
            {
                UserMnemo UserMnemo = new UserMnemo();
                UserMnemo.id_user = User.IdUser;

                UserMnemo.login = User.Login;
                UserMnemo.password = User.Password;

                UserMnemos.Add(UserMnemo);
            }

            return Json(UserMnemos);
        }


        public JsonResult Get(int id)
        {
            User Users = db.Users.Where(w => w.IdUser == id).FirstOrDefault();

            UserMnemo UserMnemo = new UserMnemo();
            UserMnemo.id_user = Users.IdUser;

            UserMnemo.login = Users.Login;
            UserMnemo.password = Users.Password;

            return Json(UserMnemo);
        }

        [HttpPost]
        public JsonResult Create(UserMnemo UserMnemo)
        {

            User Users = new User();

            Users.Login = UserMnemo.login;
            Users.Password = UserMnemo.password;

            db.Users.Add(Users);
            db.SaveChanges();

            List<int> jsondata = new List<int>() { };
            return Json(jsondata);
        }

        [HttpPost]
        public JsonResult Update(UserMnemo UserMnemo)
        {
            
          
            User User_new = db.Users.Where(w => w.IdUser == UserMnemo.id_user).FirstOrDefault();

            User_new.Login = UserMnemo.login;
            User_new.Password = UserMnemo.password;


            db.Entry(User_new).State = EntityState.Modified;
            db.SaveChanges();
            
            return Json(User_new);
        }

        public JsonResult Delete(int id)
        {
            User Users = db.Users.Where(w => w.IdUser  == id).FirstOrDefault();
            

            db.Users.Remove(Users);
            db.SaveChanges();

            List<int> jsondata = new List<int>() { id };

            return Json(jsondata);
        }

        public JsonResult DeleteMany(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return Json(new List<SubsystemMnemo>());

            List<int> jsondata = new List<int>();
            foreach (var id in ids)
            {
                User Users = db.Users.Where(w => w.IdUser == id).FirstOrDefault();

                db.Users.Remove(Users);
                db.SaveChanges();

                jsondata.Add(id);
            };

            return Json(jsondata);
        }

    }
}
