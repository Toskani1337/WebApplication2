using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Subsystem
    {
        [Key]
        public int IdSubsystem { get; set; }
        [Display(Name = "Модуль")]
        public string name { get; set; }
    }

    public class SubsystemMnemo
    {    
        public int id_subsystem { get; set; }
        [Display(Name = "Модуль")]
        public string name { get; set; }
    }

    
}