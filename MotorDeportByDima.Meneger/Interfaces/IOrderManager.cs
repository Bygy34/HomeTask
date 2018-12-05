using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDeportByDima.Meneger.Entities;

namespace MotorDeportByDima.Meneger.Interfaces
{
    public interface IOrderManager : IDisposable
    {
        void CreateUserOrder(OrderUser item, string st);

        void CreateOrderDriver(OrderDriver item, string st);

        List<Order> GetAllOrder();

        List<OrderUser> GetAllUserOrder();

        List<OrderUser> GetUserDetailsById(ApplicationUser applicationUser);

        List<OrderDriver> GetDriverDetailsById(ApplicationUser applicationUser);

        List<OrderDriver> GetAllOrderDriver();

        void DeleteUserOrderById(string idUserOrder);

        void DeleteDriverOrderById(string idDriverOrder);

        void UpdateOrderUser(OrderUser item);

        void UpdateOrderDriver(OrderDriver item);

        OrderUser FindOrderUser(string id);

        OrderDriver FindOrderDriver(string id);

        List<Order> GetOrderByUser(string id);

        void UpdateOrder(string id);

        List<Order> GetOrderDetails(string id);
        
    }
}
