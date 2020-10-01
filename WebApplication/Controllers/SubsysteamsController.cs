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
    public class SubsysteamsController : Controller
    {

        Context db;
        public SubsysteamsController(Context context)
        {
            db = context;
        }

        public IActionResult Subsysteams()
        {
            ViewBag.IdUser = HttpContext.Session.GetInt32("UserId");
            ViewBag.Login = HttpContext.Session.GetString("Login");

            return View();
        }

        public JsonResult GetData()
        {



            var data = db.Subsystems.ToList();

            List<SubsystemMnemo> SubsystemMnemos = new List<SubsystemMnemo>();
            foreach (var Subsystem in data)
            {
                SubsystemMnemo SubsystemMnemo = new SubsystemMnemo();
                SubsystemMnemo.id_subsystem = Subsystem.IdSubsystem;
                SubsystemMnemo.name = Subsystem.name;

                SubsystemMnemos.Add(SubsystemMnemo);
            }

            return Json(SubsystemMnemos);
        }


        public JsonResult Get(int id)
        {
            Subsystem Subsystems = db.Subsystems.Where(w => w.IdSubsystem == id).FirstOrDefault();

            SubsystemMnemo SubsystemMnemo = new SubsystemMnemo();

            SubsystemMnemo.id_subsystem = Subsystems.IdSubsystem;
            SubsystemMnemo.name = Subsystems.name;

            return Json(SubsystemMnemo);
        }

        [HttpPost]
        public JsonResult Create(SubsystemMnemo SubsystemMnemo)
        {

            Subsystem Subsystems = new Subsystem();         
            Subsystems.name = SubsystemMnemo.name;

            db.Subsystems.Add(Subsystems);
            db.SaveChanges();

            List<int> jsondata = new List<int>() { };
            return Json(jsondata);
        }

        [HttpPost]
        public JsonResult Update(SubsystemMnemo SubsystemMnemo)
        {
            
          
            Subsystem Subsystems_new = db.Subsystems.Where(w => w.IdSubsystem == SubsystemMnemo.id_subsystem).FirstOrDefault();
            Subsystems_new.name = SubsystemMnemo.name;


            db.Entry(Subsystems_new).State = EntityState.Modified;
            db.SaveChanges();
            
            return Json(Subsystems_new);
        }

        public JsonResult Delete(int id)
        {
            Subsystem Subsystems = db.Subsystems.Where(w => w.IdSubsystem == id).FirstOrDefault();
            

            db.Subsystems.Remove(Subsystems);
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
                Subsystem Subsystems = db.Subsystems.Where(w => w.IdSubsystem == id).FirstOrDefault();

                db.Subsystems.Remove(Subsystems);
                db.SaveChanges();

                jsondata.Add(id);
            };

            return Json(jsondata);
        }

    }
}
