using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApplication.Models
{
    public class SampleData
    {
        public static void Initialize(Context context)
        {
            if (!context.Subsystems.Any())
            {
                context.Subsystems.AddRange(
                    new Subsystem
                    {
                        IdSubsystem = 1,
                        name = "Бухгалтерия"                       
                    },
                    new Subsystem
                    {
                        IdSubsystem = 2,
                        name = "Отдел кадров"
                    },
                    new Subsystem
                    {
                        IdSubsystem = 3,
                        name = "Отдел разработчиков"
                    },
                    new Subsystem
                    {
                        IdSubsystem = 4,
                        name = "Отдел консультантов"
                    },
                    new Subsystem
                    {
                        IdSubsystem = 4,
                        name = "Отдел тестирования"
                    }
                    );
                context.SaveChanges();

                context.Users.AddRange(
                    new User
                    {
                        IdUser = 1,
                        Login = "Донской",
                        Password = "1"
                    },
                    new User
                    {
                        IdUser = 2,
                        Login = "Стерхова",
                        Password = "1"
                    },
                    new User
                    {
                        IdUser = 3,
                        Login = "Антонов",
                        Password = "1"
                    }
                   
                );
                context.SaveChanges();
            }
        }
    }
}
