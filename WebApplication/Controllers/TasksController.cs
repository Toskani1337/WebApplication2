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
    public class TasksController : Controller
    {

        Context db;
        public TasksController(Context context)
        {
            db = context;
        }

        
        public JsonResult GetData(int id_remark)
        {

            var data = db.Tasks.Where(w=>w.IdRemark == id_remark).ToList();

            List<TaskMnemo> TaskMnemos = new List<TaskMnemo>();
            foreach (var Task in data)
            {
                TaskMnemo TaskMnemo = new TaskMnemo();

                TaskMnemo.doc_date = Task.DocDate.Date;              
                TaskMnemo.doc_date_string = Task.DocDate.Date.ToShortDateString();              
                TaskMnemo.id_remark = Task.IdRemark;
                TaskMnemo.id_status = Task.IdStatus;
                TaskMnemo.id_task = Task.IdTask;


                switch (Task.IdStatus)
                {
                    case 0:
                        TaskMnemo.status = "Черновик";
                        break;
                    case 1:
                        TaskMnemo.status = "Выполнен";
                        break;
                    case 2:
                        TaskMnemo.status = "Не выполнен";
                        break;
                    default:
                        TaskMnemo.status = "Черновик";
                        break;
                }
              
                TaskMnemo.text_task = Task.TextTask;
                TaskMnemo.user_create = db.Users.Where(w=>w.IdUser == Task.IdUserCreate).FirstOrDefault().Login;

                TaskMnemos.Add(TaskMnemo);
            }

            return Json(TaskMnemos);
        }


        public JsonResult Get(int id)
        {
            WebApp.Models.Task Task = db.Tasks.Where(w => w.IdTask == id).FirstOrDefault();

            TaskMnemo TaskMnemo = new TaskMnemo();

            TaskMnemo.doc_date = Task.DocDate.Date;
            TaskMnemo.doc_date_string = Task.DocDate.Date.ToShortDateString();
            TaskMnemo.id_remark = Task.IdRemark;
            TaskMnemo.id_status = Task.IdStatus;
            TaskMnemo.id_task = Task.IdTask;


            switch (Task.IdStatus)
            {
                case 0:
                    TaskMnemo.status = "Черновик";
                    break;
                case 1:
                    TaskMnemo.status = "Выполнен";
                    break;
                case 2:
                    TaskMnemo.status = "Не выполнен";
                    break;
                default:
                    TaskMnemo.status = "Черновик";
                    break;
            }

            TaskMnemo.text_task = Task.TextTask;
            TaskMnemo.user_create = db.Users.Where(w => w.IdUser == Task.IdUserCreate).FirstOrDefault().Login;

            return Json(TaskMnemo);
        }

        [HttpPost]
        public JsonResult Create(TaskMnemo TaskMnemo)
        {
            int? IdUser = HttpContext.Session.GetInt32("UserId");

            WebApp.Models.Task Task = new WebApp.Models.Task();

            Task.DocDate = TaskMnemo.doc_date;
            Task.DocDateCreate = DateTime.Now;
            Task.DocDateLast = DateTime.Now;
            Task.IdRemark = TaskMnemo.id_remark;
            Task.IdStatus = 0;       
            Task.IdUserCreate = IdUser.Value;
            Task.IdUserLast = IdUser.Value;
           
            Task.TextTask = TaskMnemo.text_task;

            db.Tasks.Add(Task);
            db.SaveChanges();

            List<int> jsondata = new List<int>() { };
            return Json(jsondata);
        }

        [HttpPost]
        public JsonResult Update(TaskMnemo TaskMnemo)
        {
            int? IdUser = HttpContext.Session.GetInt32("UserId");

            WebApp.Models.Task Task = db.Tasks.Where(w => w.IdTask == TaskMnemo.id_task).FirstOrDefault();

            Task.DocDate = TaskMnemo.doc_date;
            Task.DocDateLast = DateTime.Now;            
            Task.IdUserLast = IdUser.Value;      
            Task.TextTask = TaskMnemo.text_task;
           
            db.Entry(Task).State = EntityState.Modified;
            db.SaveChanges();

            return Json(Task);
        }

        public JsonResult Delete(int id)
        {
            WebApp.Models.Task Task = db.Tasks.Where(w => w.IdTask == id).FirstOrDefault();


            db.Tasks.Remove(Task);
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
                WebApp.Models.Task Task = db.Tasks.Where(w => w.IdTask == id).FirstOrDefault();

                db.Tasks.Remove(Task);
                db.SaveChanges();

                jsondata.Add(id);
            };

            return Json(jsondata);
        }


        [HttpPost]
        public JsonResult UpdateMany(int status, int[] id_tasks)
        {
            int? IdUser = HttpContext.Session.GetInt32("UserId");

            if (id_tasks == null || id_tasks.Length == 0)
                return Json(new List<WebApp.Models.Task>());

            List<int> jsondata = new List<int>();
            foreach (var id in id_tasks)
            {
                WebApp.Models.Task Task = db.Tasks.Where(w => w.IdTask == id).FirstOrDefault();
                
                if (Task == null)
                    throw new NotImplementedException();

                Task.DocDateLast = DateTime.Now;
                Task.IdUserLast = IdUser.Value;

                Task.IdStatus = status;

                db.Entry(Task).State = EntityState.Modified;
                db.SaveChanges();

                Remark Remark = db.Remarks.Where(w => w.IdRemark == Task.IdRemark).FirstOrDefault();

                List<WebApp.Models.Task> TaskCloses = db.Tasks.Where(w => w.IdRemark == Task.IdRemark && w.IdStatus!=0).ToList();
                List<WebApp.Models.Task> TasksRemark = db.Tasks.Where(w => w.IdRemark == Task.IdRemark).ToList();
                if(TasksRemark.Count == TaskCloses.Count)
                {
                    Remark.IdStatus = 1;
                    db.Entry(Remark).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Remark.IdStatus = 0;
                    db.Entry(Remark).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                jsondata.Add(id);
            };

            return Json(jsondata);

        }

    }
}
