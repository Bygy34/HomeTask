using MotorDeportByDima.Meneger.EF;
using MotorDeportByDima.Meneger.Entities;
using MotorDeportByDima.Meneger.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Meneger.Repositories
{
    public class OrdertManager : IOrderManager
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public ApplicationContext Database { get; set; }
        public OrdertManager(ApplicationContext db)
        {
            Database = db;
        }



        public void CreateUserOrder(OrderUser item, string st)
        {
            item.Id = Database.orderUsers.Count() + 2.ToString();
            if (st != null)
            {
                var itemOther = Database.orderDrivers.Find(st);
                if (item.Weight <= itemOther.MaxWeight)
                {
                    Database.order.Add(new Order { OrderDriverId = st, OrderUserId = itemOther.Id, Status = "Ready" });
                    item.Status = false;
                    Database.orderDrivers.Find(st).Status = false;

                }

            }

            Database.orderUsers.Add(item);
            try
            {
                Database.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, ";  The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void CreateOrderDriver(OrderDriver item, string st)
        {
            item.Id = Database.orderDrivers.Count() + 2.ToString();
            if (st != null)
            {
                var itemOther = Database.orderUsers.Find(st);
                if (item.MaxWeight >= itemOther.Weight)
                {
                    Database.order.Add(new Order { OrderDriverId = item.Id, OrderUserId = itemOther.Id, Status = "Ready" });
                    item.Status = false;
                    Database.orderUsers.Find(st).Status = false;

                }

            }
            Database.orderDrivers.Add(item);

            try
            {
                Database.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, ";  The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public OrderUser FindOrderUser(string id)
        {
            OrderUser orderUser = Database.orderUsers.Find(id);
            return orderUser;
        }

        public OrderDriver FindOrderDriver(string id)
        {
            OrderDriver orderDriver = Database.orderDrivers.Find(id);
            return orderDriver;
        }

        public void UpdateOrderUser(OrderUser item)
        {
            var r = Database.orderUsers.Find(item.Id);
            r.ProductName = item.ProductName;
            r.EndAddressCity = item.EndAddressCity;
            r.EndAddressRoad = item.EndAddressRoad;
            r.EndAddressHouse = item.EndAddressHouse;
            r.EndAddressCity = item.EndAddressCity;
            r.EndAddressRoad = item.EndAddressRoad;
            r.EndAddressHouse = item.EndAddressHouse;
            r.StartDate = item.StartDate;
            r.DueDate = item.DueDate;
            r.Weight = item.Weight;
            //Database.Entry(item).State = EntityState.Modified;
            Database.SaveChanges();
        }

        public void UpdateOrderDriver(OrderDriver item)
        {
            var r = Database.orderDrivers.Find(item.Id);
            r.StartAddressCity = item.StartAddressCity;
            r.StartAddressRoad = item.StartAddressRoad;
            r.StartAddressHouse = item.StartAddressHouse;
            r.MaxWeight = item.MaxWeight;
            r.CarBrand = item.CarBrand;
            //Database.Entry(item).State = EntityState.Modified;
            Database.SaveChanges();
        }

        public void DeleteUserOrderById(string idUserOrder)
        {
            //if(Database.orderUsers.Find(idUserOrder) != null && 
            //   Database.orderUsers.Find(idUserOrder).ApplicationUserId == idUserIdentyti)
            //{
            OrderUser r = Database.orderUsers.Find(idUserOrder);
            Database.orderUsers.Remove(r);
            Database.SaveChanges();

            //else result = "Что то пошло не так";

        }

        public void DeleteDriverOrderById(string idDriverOrder)
        {
            //if (Database.orderDrivers.Find(idDriverOrder) != null &&
            //    Database.orderUsers.Find(idDriverOrder).ApplicationUserId == idUserIdentyti)
            //{
            OrderDriver r = Database.orderDrivers.Find(idDriverOrder);
            Database.orderDrivers.Remove(r);
            Database.SaveChanges();

            //result = "Заявка удалена";
            //_logger.Error("[Error in ClientController.Edit - id: " + idDriverOrder + "24234" + idDriverOrder + " - Error: "  "]");
            //result = "ЧТо то пошло не так удалена";

        }

        public List<Order> GetAllOrder()
        {
            var result = Database.order.ToList();
            return result.ToList();
        }

        public List<Order> GetOrderByUser(string id)
        {
            List<Order> result = Database.order.Where(s => s.OrderDriver.ApplicationUser.Id == id).ToList();
            result.AddRange(Database.order.Where(s => s.OrderUser.ApplicationUser.Id == id).ToList());
            return result;
        }

        public List<Order> GetOrderDetails(string id)
        {
            List<Order> result = Database.order.Where(s => s.OrderUser.Id == id).ToList();
            result.AddRange(Database.order.Where(s => s.OrderDriver.Id == id).ToList());
            return result;
        }

        public void UpdateOrder(string id)
        {
            int idd = Convert.ToInt32(id);
            Order result = Database.order.Find(idd);
            switch (result.Status)
            {
                case "Ready":
                    result.Status = "In progress";
                    break;
                case "In progress":
                    result.Status = "Done";
                    break;
                default:
                    result.Status = "Ready";
                    break;
            }
            Database.SaveChanges();
        }

        public List<OrderUser> GetAllUserOrder()
        {
            var result = Database.orderUsers.ToList().Where(x => x.Status == true);
            return result.ToList();
        }

        public List<OrderUser> GetUserDetailsById(ApplicationUser applicationUser)
        {
            return Database.orderUsers.Where(x => x.ApplicationUser.Id == applicationUser.Id).ToList(); ;
        }

        public List<OrderDriver> GetAllOrderDriver()
        {
            var result = Database.orderDrivers.ToList().Where(x => x.Status == true);
            return result.ToList();
        }

        public List<OrderDriver> GetDriverDetailsById(ApplicationUser applicationUser)
        {
            List<OrderDriver> result;
            if (applicationUser.Id != null)
            {
                result = Database.orderDrivers.Where(x => x.ApplicationUser.Id == applicationUser.Id).ToList();
            }
            else
            {
                result = null;
            }

            return result;

        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
