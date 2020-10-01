using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Remark
    {
        [Key]
        public int IdRemark { get; set; }
        public DateTime DocDate { get; set; }
        public int IdSubsystem { get; set; }
        public int IdUserCreate { get; set; }
        public DateTime DocDateCreate { get; set; }
        public int IdUserLast { get; set; }
        public DateTime DocDateLast { get; set; }
        public bool IsError { get; set; }
        public int IdStatus { get; set; }
        public string TextRemark { get; set; }
        public Subsystem Subsystem { get; set; }

    }


    public class RemarkMnemo
    {
        public int id_remark { get; set; }
       
        public string doc_date_string { get; set; }
        [Display(Name = "Дата создания")]
        public DateTime doc_date { get; set; }
        [Display(Name = "Модуль")]
        public int id_subsystem { get; set; }
        public string subsystem { get; set; }
        public string user_create { get; set; }
        [Display(Name = "Ошибка")]
        public bool is_error { get; set; }
        public string error { get; set; }
        public int id_status { get; set; }
        public string status { get; set; }
        [Display(Name = "Текст замечания")]
        public string text_remark { get; set; }
       
    }
}