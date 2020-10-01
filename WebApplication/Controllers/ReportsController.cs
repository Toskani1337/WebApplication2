using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ReportsController : Controller
    {

        Context db;
        public ReportsController(Context context)
        {
            db = context;
        }

        public IActionResult Reports()
        {
            ViewBag.IdUser = HttpContext.Session.GetInt32("UserId");
            ViewBag.Login = HttpContext.Session.GetString("Login");

            var Subsystems = db.Subsystems.ToList();
            Subsystem Subsystem = new Subsystem
            {
                IdSubsystem =-1,
                name = "Все"
            };

            Subsystems.Reverse();
            Subsystems.Add(Subsystem);
            Subsystems.Reverse();

            SelectList SubsystemSelectList = new SelectList(Subsystems, "IdSubsystem", "name");

            ViewBag.Subsystem = SubsystemSelectList;

            return View();
        }

        public HtmlString GetRemarksData(int subsystem)
        {
            if(subsystem == 0)
                return new HtmlString(null);
            List<Remark> Remarks;
            if (subsystem == -1)
            {
                Remarks = db.Remarks.Where(w=> w.IdStatus == 0).ToList();
            }
            else
            {
                Remarks = db.Remarks.Where(w => w.IdSubsystem == subsystem && w.IdStatus == 0).ToList();
            }
            
            List<RemarkMnemo> RemarkMnemos = new List<RemarkMnemo>();
            foreach (var Remark in Remarks)
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
                RemarkMnemo.user_create = db.Users.Where(w => w.IdUser == Remark.IdUserCreate).FirstOrDefault().Login;

                RemarkMnemos.Add(RemarkMnemo);
            }


            string result = "";
            int num = 1;

            // заполняем шапку на основании периода
            result += "<thead>";

            result += "<tr>";

            result += "<th>№</th>";
            result += "<th>Дата создания</th>";
            result += "<th>Модуль</th>";
            result += "<th>Создатель замечания</th>";
            result += "<th>Ошибка</th>";
            result += "<th>Статус</th>";
            result += "<th>Текст замечания</th>";

            result += "</tr>";

            result += "</thead>";

            // заполняем тело таблицы


            result += "<tbody>";


            foreach (var RemarkMnemo in RemarkMnemos)
            {

                result += "<tr>";
                result += "<td>" + num++ + "</td>";
                result += "<td>" + RemarkMnemo.doc_date_string + "</td>";
                result += "<td>" + RemarkMnemo.subsystem + "</td>";
                result += "<td>" + RemarkMnemo.user_create + "</td>";
                result += "<td>" + RemarkMnemo.error + "</td>";
                result += "<td>" + RemarkMnemo.status + "</td>";
                result += "<td>" + RemarkMnemo.text_remark + "</td>";

                result += "</tr>";
            }

            result += "</tbody>";

            return new HtmlString(result);
        }




    }
}
