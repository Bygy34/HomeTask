using Microsoft.AspNet.Identity.EntityFramework;
using MotorDeportByDima.Meneger.EF;
using MotorDeportByDima.Meneger.Entities;
using MotorDeportByDima.Meneger.Identity;
using MotorDeportByDima.Meneger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Meneger.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IOrderManager orderManager;

        public IdentityUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            orderManager = new OrdertManager(db);
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public IOrderManager OrderManager
        {
            get { return orderManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    orderManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
