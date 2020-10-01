using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

    }

    public class UserMnemo
    {
        public int id_user { get; set; }
        [Display(Name = "Логин")]
        public string login { get; set; }
        [Display(Name = "Пароль")]
        public string password { get; set; }
    }
}
