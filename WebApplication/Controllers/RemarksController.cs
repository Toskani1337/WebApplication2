using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class RemarksController : Controller
    {

        Context db;
        public RemarksController(Context context)
        {
            db = context;
        }

        public IActionResult Remarks()
        {
            ViewBag.IdUser = HttpContext.Session.GetInt32("UserId");
            ViewBag.Login = HttpContext.Session.GetString("Login");

            SelectList Subsystem = new SelectList(db.Subsystems.ToList(), "IdSubsystem", "name");

            ViewBag.Subsystem = Subsystem;
            return View();
        }

        public JsonResult GetData()
        {

            var data = db.Remarks.ToList();

            List<RemarkMnemo> RemarkMnemos = new List<RemarkMnemo>();
            foreach (var Remark in data)
            {
                RemarkMnemo RemarkMnemo = new RemarkMnemo();

                RemarkMnemo.doc_date = Remark.DocDate.Date;
                RemarkMnemo.doc_date_string = Remark.DocDate.Date.ToShortDateString();
                RemarkMnemo.error = Remark.IsError == true ? "Да" : "Нет";
                RemarkMnemo.id_remark = Remark.IdRemark;
                RemarkMnemo.id_status = Remark.IdStatus;
                RemarkMnemo.id_subsystem = Remark.IdSubsystem;
                RemarkMnemo.is_error = Remark.IsError;
                
                switch (Remark.IdStatus)
                {
                    case 0:
                        RemarkMnemo.status = "Открыт";
                        break;
                    case 1:
                        RemarkMnemo.status = "Закрыт";
                        break;                  
                    default:
                        RemarkMnemo.status = "Открыт";
                        break;
                }

                RemarkMnemo.subsystem = db.Subsystems.Where(w => w.IdSubsystem == Remark.IdSubsystem).FirstOrDefault().name;
                RemarkMnemo.text_remark = Remark.TextRemark;
                RemarkMnemo.user_create = db.Users.Where(w=>w.IdUser == Remark.IdUserCreate).FirstOrDefault().Login;

                RemarkMnemos.Add(RemarkMnemo);
            }

            return Json(RemarkMnemos);
        }


        public JsonResult Get(int id)
        {
            Remark Remark = db.Remarks.Where(w => w.IdRemark == id).FirstOrDefault();

            RemarkMnemo RemarkMnemo = new RemarkMnemo();

            RemarkMnemo.doc_date = Remark.DocDate.Date;
            RemarkMnemo.doc_date_string = Remark.DocDate.Date.ToShortDateString();
            RemarkMnemo.error = Remark.IsError == true ? "Да" : "Нет";
            RemarkMnemo.id_remark = Remark.IdRemark;
            RemarkMnemo.id_status = Remark.IdStatus;
            RemarkMnemo.id_subsystem = Remark.IdSubsystem;
            RemarkMnemo.is_error = Remark.IsError;

            switch (Remark.IdStatus)
            {
                case 0:
                    RemarkMnemo.status = "Открыт";
                    break;
                case 1:
                    RemarkMnemo.status = "Закрыт";
                    break;
                default:
                    RemarkMnemo.status = "Открыт";
                    break;
            }

            RemarkMnemo.subsystem = db.Subsystems.Where(w => w.IdSubsystem == Remark.IdSubsystem).FirstOrDefault().name;
            RemarkMnemo.text_remark = Remark.TextRemark;
            RemarkMnemo.user_create = db.Users.Where(w => w.IdUser == Remark.IdUserCreate).FirstOrDefault().Login;

            return Json(RemarkMnemo);
        }

        [HttpPost]
        public JsonResult Create(RemarkMnemo RemarkMnemo)
        {
            int? IdUser = HttpContext.Session.GetInt32("UserId");

            Remark Remark = new Remark();

            Remark.DocDate = RemarkMnemo.doc_date;
            Remark.DocDateCreate = DateTime.Now;
            Remark.DocDateLast = DateTime.Now;
            Remark.IdStatus = 0;
            Remark.IdSubsystem = RemarkMnemo.id_subsystem;
            Remark.IdUserCreate = IdUser.Value;
            Remark.IdUserLast = IdUser.Value;
            Remark.IsError = RemarkMnemo.is_error;
            Remark.TextRemark = RemarkMnemo.text_remark;

            db.Remarks.Add(Remark);
            db.SaveChanges();

            List<int> jsondata = new List<int>() { };
            return Json(jsondata);
        }

        [HttpPost]
        public JsonResult Update(RemarkMnemo RemarkMnemo)
        {
            int? IdUser = HttpContext.Session.GetInt32("UserId");

            Remark Remark = db.Remarks.Where(w => w.IdRemark == RemarkMnemo.id_remark).FirstOrDefault();

            Remark.DocDate = RemarkMnemo.doc_date;
            Remark.DocDateLast = DateTime.Now;
            Remark.IdSubsystem = RemarkMnemo.id_subsystem;
            Remark.IdUserLast = IdUser.Value;
            Remark.IsError = RemarkMnemo.is_error;
            Remark.TextRemark = RemarkMnemo.text_remark;
           
            db.Entry(Remark).State = EntityState.Modified;
            db.SaveChanges();

            return Json(Remark);
        }

        public JsonResult Delete(int id)
        {
            Remark Remark = db.Remarks.Where(w => w.IdRemark == id).FirstOrDefault();


            db.Remarks.Remove(Remark);
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
                Remark Remark = db.Remarks.Where(w => w.IdRemark == id).FirstOrDefault();

                db.Remarks.Remove(Remark);
                db.SaveChanges();

                jsondata.Add(id);
            };

            return Json(jsondata);
        }

    }
}
