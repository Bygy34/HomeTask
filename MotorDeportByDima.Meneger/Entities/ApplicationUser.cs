using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Meneger.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<OrderUser> OrderUser { get; set; }
        public virtual List<OrderDriver> OrderDriver { get; set; }
    }
}
