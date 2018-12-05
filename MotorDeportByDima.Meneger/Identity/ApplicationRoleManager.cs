using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MotorDeportByDima.Meneger.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Meneger.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(RoleStore<ApplicationRole> store)
                    : base(store)
        { }
    }
}
