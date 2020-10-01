using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Task 
    {
        [Key]
        public int IdTask { get; set; }
        public int IdRemark { get; set; }
        public DateTime DocDate { get; set; }
        public int IdUserCreate { get; set; }
        public DateTime DocDateCreate { get; set; }
        public int IdUserLast { get; set; }
        public DateTime DocDateLast { get; set; }        
        public int IdStatus { get; set; }
        public string TextTask { get; set; }
        public Remark Remark { get; set; }
    }

    public class TaskMnemo
    {     
        public int id_task { get; set; }
        public int id_remark { get; set; }
        public string doc_date_string { get; set; }
        [Display(Name = "Дата создания")]
        public DateTime doc_date { get; set; }
        public string user_create { get; set; }
        public int id_status { get; set; }
        public string status { get; set; }
        [Display(Name = "Текст задачи")]
        public string text_task { get; set; }
        
    }
}