using Microsoft.AspNet.Identity;
using MotorDeportByDima.Meneger.Entities;
using MotorDeportByDima.Meneger.Interfaces;
using MotorDeportByDima.Service.DTO;
using MotorDeportByDima.Service.Infrastructure;
using MotorDeportByDima.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Service.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }


        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public void BlockUser(string email)
        {
            Database.UserManager.Delete(Database.UserManager.FindByEmail(email));
            Database.SaveAsync();
        }

        public void CreateDriverOrder(OrderDriver orderDriver, string st)
        {
            orderDriver.ApplicationUser = Database.UserManager.FindById(orderDriver.ApplicationUserId);
            Database.OrderManager.CreateOrderDriver(orderDriver, st);
        }

        public void CreateUserOrder(OrderUser orderUser, string st)
        {
            orderUser.ApplicationUser = Database.UserManager.FindById(orderUser.ApplicationUserId);
            Database.OrderManager.CreateUserOrder(orderUser, st);
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                await Database.UserManager.CreateAsync(user, userDto.Password);
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public void DeleteUserOrderById(string idUserOrder)
        {
            Database.OrderManager.DeleteUserOrderById(idUserOrder);

        }

        public void DeleteDriverOrderById(string idDriverOrder)
        {
            Database.OrderManager.DeleteDriverOrderById(idDriverOrder);

        }

        public OrderUser FindOrderUser(string idOrderUser)
        {
            return Database.OrderManager.FindOrderUser(idOrderUser);
        }

        public OrderDriver FindOrderDriver(string idOrderDriver)
        {
            return Database.OrderManager.FindOrderDriver(idOrderDriver);
        }

        public List<OrderDriver> GetDriverDetails()
        {
            var result = Database.OrderManager.GetAllOrderDriver();
            return result;
        }

        public List<OrderUser> GetUserDetails()
        {
            var result = Database.OrderManager.GetAllUserOrder();
            return result;
        }

        public List<OrderDriver> GetDriverDetailsById(string applicationDriverId)
        {
            var user = Database.UserManager.FindById(applicationDriverId);
            var result = Database.OrderManager.GetDriverDetailsById(user);
            return result;
        }

        public List<OrderUser> GetUserDetailsById(string applicationUserId)
        {
            var user = Database.UserManager.FindById(applicationUserId);
            var result = Database.OrderManager.GetUserDetailsById(user);
            return result;
        }

        public List<ApplicationUser> GetAllUser()
        {
            var result = Database.UserManager.Users.ToList();
            return result;
        }

        public List<Order> GetOrders()
        {
            var result = Database.OrderManager.GetAllOrder();
            return result;
        }

        public List<Order> GetOrderDetails(string id)
        {
            var result = Database.OrderManager.GetOrderDetails(id);
            return result;
        }

        public List<Order> GetOrdersByUser(string id)
        {
            var result = Database.OrderManager.GetOrderByUser(id);
            return result;
        }

        public void UpdateOrder(string id)
        {
            Database.OrderManager.UpdateOrder(id);
        }

        public void UpdateOrderDriver(OrderDriver orderDriver)
        {
            Database.OrderManager.UpdateOrderDriver(orderDriver);
        }

        public void UpdateOrderUser(OrderUser orderUser)
        {
            Database.OrderManager.UpdateOrderUser(orderUser);
        }

        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }

            await Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
