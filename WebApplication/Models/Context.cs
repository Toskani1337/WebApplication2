using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApplication.Models
{
    public class Context : DbContext
    {
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<Subsystem> Subsystems { get; set; }
        public DbSet<WebApp.Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
    
  
}
