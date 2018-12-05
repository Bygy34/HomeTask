using MotorDeportByDima.Meneger.Entities;
using MotorDeportByDima.Service.DTO;
using MotorDeportByDima.Service.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Service.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);

        Task<ClaimsIdentity> Authenticate(UserDTO userDto);

        List<ApplicationUser> GetAllUser();

        List<Order> GetOrders();

        List<OrderUser> GetUserDetails();

        List<OrderDriver> GetDriverDetails();

        Task SetInitialData(UserDTO adminDto, List<string> roles);

        List<OrderUser> GetUserDetailsById(string applicationUserId);

        List<OrderDriver> GetDriverDetailsById(string applicationUserId);

        void CreateUserOrder(OrderUser orderUser, string st);

        void CreateDriverOrder(OrderDriver orderDriver, string st);

        void DeleteUserOrderById(string idUserOrder);

        void DeleteDriverOrderById(string idDriverOrder);

        void UpdateOrderDriver(OrderDriver orderDriver);

        void UpdateOrderUser(OrderUser orderUser);

        OrderUser FindOrderUser(string idOrderUser);

        OrderDriver FindOrderDriver(string idOrderDriver);

        List<Order> GetOrdersByUser(string id);

        void UpdateOrder(string id);

        List<Order> GetOrderDetails(string id);

        void BlockUser(string email);
    }
}
