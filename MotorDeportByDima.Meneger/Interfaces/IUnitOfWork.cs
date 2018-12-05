using MotorDeportByDima.Meneger.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Meneger.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }

        IOrderManager OrderManager { get; }

        ApplicationRoleManager RoleManager { get; }

        Task SaveAsync();
    }
}
