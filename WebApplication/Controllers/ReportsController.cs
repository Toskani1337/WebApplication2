using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using WebApp.Models;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ReportsController : Controller
    {
       //ДЛЯ КОММИТА ИСПОЛЬЗУЕМ
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
                IdSubsystem = -1,
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
            if (subsystem == 0)
                return new HtmlString(null);
            List<Remark> Remarks;
            if (subsystem == -1)
            {
                Remarks = db.Remarks.Where(w => w.IdStatus == 0).ToList();
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



        public FileResult ExportRemarksData(int subsystem)
        {

            var newFile = new FileInfo(Path.Combine("Reports/", "RemarksData.xlsx"));

            byte[] stream;

            string subsystemName;

            if (subsystem == 0)
                return null;

            List<Remark> Remarks;
            if (subsystem == -1)
            {
                subsystemName = "Все";
                Remarks = db.Remarks.Where(w => w.IdStatus == 0).ToList();
            }
            else
            {
                subsystemName = db.Subsystems.Where(w => w.IdSubsystem == subsystem).FirstOrDefault().name;
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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage pck = new ExcelPackage(newFile))
            {

                pck.Workbook.Properties.Author = "Donskoi Igor";
                int a = pck.Workbook.Worksheets.Count;
                ExcelWorksheet ws = pck.Workbook.Worksheets[0];
                // белый фон для всех ячеек
                ws.Cells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);


                ws.Cells[2, 7].Value = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                ws.Cells[4, 1].Value = "Незакрытые замечания по отделу: " + subsystemName;

                // пошли по строкам   
                int count = 1;
                int write_row = 8; // номер начальной строки в файле



                foreach (var RemarkMnemo in RemarkMnemos)
                {

                    ws.InsertRow(write_row, 1, 7);

                    ws.Cells[write_row, 1].Value = count;
                    ws.Cells[write_row, 1].Style.Font.Bold = false;

                    ws.Cells[write_row, 2].Value = RemarkMnemo.doc_date_string;
                    ws.Cells[write_row, 2].Style.Font.Bold = false;

                    ws.Cells[write_row, 3].Value = RemarkMnemo.subsystem;
                    ws.Cells[write_row, 3].Style.Font.Bold = false;

                    ws.Cells[write_row, 4].Value = RemarkMnemo.user_create;
                    ws.Cells[write_row, 4].Style.Font.Bold = false;

                    ws.Cells[write_row, 5].Value = RemarkMnemo.error;
                    ws.Cells[write_row, 5].Style.Font.Bold = false;

                    ws.Cells[write_row, 6].Value = RemarkMnemo.status;
                    ws.Cells[write_row, 6].Style.Font.Bold = false;

                    ws.Cells[write_row, 7].Value = RemarkMnemo.text_remark;
                    ws.Cells[write_row, 7].Style.Font.Bold = false;
                                    
                    count++;
                    write_row++;

                }

                // сохряняем в поток
                stream = pck.GetAsByteArray();
            }

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet; charset=utf-8",
                "Незакрытые замечания по отделу.xlsx");
        }

    }
}
