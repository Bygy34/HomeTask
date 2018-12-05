using Microsoft.AspNet.Identity.EntityFramework;
using MotorDeportByDima.Meneger.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Meneger.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString) { }


         public ApplicationContext() : base("DefaultConnection") { }
        public DbSet<OrderUser> orderUsers { get; set; }
        public DbSet<OrderDriver> orderDrivers { get; set; }
        public DbSet<Order> order { get; set; }

    }
}
