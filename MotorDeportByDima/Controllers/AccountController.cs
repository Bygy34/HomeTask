using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MotorDeportByDima.Meneger.Entities;
using MotorDeportByDima.Models;
using MotorDeportByDima.Service.DTO;
using MotorDeportByDima.Service.Infrastructure;
using MotorDeportByDima.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MotorDeportByDima.Controllers
{
    public class AccountController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public ActionResult Login()
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action Login";
            logger.Info(messege);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            await SetInitialDataAsync();

            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                    string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action Login, Succses";
                    logger.Info(messege);
                }
                else
                {
                    string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action Login Уже залогинен";
                    logger.Info(messege);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");

                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action Logout Sucsees";
            logger.Info(messege);
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action Register getView";
            logger.Info(messege);
            SelectList selectListItems = new SelectList("user", "driver");
            ViewBag.Roles = selectListItems;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();

            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Role = model.Role
                };
                OperationDetails operationDetails = await UserService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action Register Succses";
                    logger.Info(messege);
                    return View("Login");
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                    string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action Register Error";
                    logger.Info(messege);
                }

            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult AllUser(string sortOrder)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            switch (sortOrder)
            {
                case "Name desc":
                    return View(UserService.GetAllUser().OrderByDescending(s => s.Email).ToList());
                default:
                    return View(UserService.GetAllUser().OrderBy(s => s.Email).ToList());
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult AllOrders(string sortOrder)
        {
            ViewBag.CarBrandSortParm = String.IsNullOrEmpty(sortOrder) ? "CarBrand desc" : "";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "StartDate desc" : "StartDate";
            switch (sortOrder)
            {
                case "StartDate":
                    return View(UserService.GetOrders().OrderBy(s => s.OrderUser.StartDate).ToList());
                case "StartDate desc":
                    return View(UserService.GetOrders().OrderByDescending(s => s.OrderUser.StartDate).ToList());
                case "CarBrand desc":
                    return View(UserService.GetOrders().OrderByDescending(s => s.OrderDriver.CarBrand).ToList());
                default:
                    return View(UserService.GetOrders().OrderBy(s => s.OrderDriver.CarBrand).ToList());
            }
        }

        [Authorize]
        public ActionResult AllOrdersByUser()
        {
            var result = UserService.GetOrdersByUser(User.Identity.GetUserId().ToString());
            if (result == null)
            {
                int[] mas = new int[2];
                mas[6] = 4;
            }
            return View(result.ToList());
        }

        [Authorize]
        public ActionResult GetOrderDetails(string id)
        {
            var result = UserService.GetOrderDetails(id);
            return View("AllOrders", result);
        }

        [Authorize(Roles = "driver")]
        public ActionResult UpdateOrder(string id)
        {
            UserService.UpdateOrder(id);
            return RedirectToAction("AllOrdersByUser");
        }

        [Authorize(Roles = "user")]
        public ActionResult CreateUserOrder(string id)
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action CreateUserOrder getView";
            logger.Info(messege);
            TempData["ID"] = id;
            return View();
        }

        [Authorize(Roles = "user")]
        public ActionResult UpdateUserOrder(string id)
        {
            OrderUser orderUser = UserService.FindOrderUser(id);
            CreateUserOrder createUserOrder = new CreateUserOrder();

            createUserOrder.StartAddressCity = orderUser.StartAddressCity;
            createUserOrder.StartAddressRoad = orderUser.StartAddressRoad;
            createUserOrder.StartAddressHouse = orderUser.StartAddressHouse;
            createUserOrder.EndAddressCity = orderUser.EndAddressCity;
            createUserOrder.EndAddressRoad = orderUser.EndAddressHouse;
            createUserOrder.EndAddressHouse = orderUser.EndAddressRoad;
            createUserOrder.StartDate = orderUser.StartDate;
            createUserOrder.DueDate = orderUser.DueDate;
            createUserOrder.ProductName = orderUser.ProductName;
            createUserOrder.Weight = orderUser.Weight;
            createUserOrder.Id = orderUser.Id;
            return View(createUserOrder);
        }

        [Authorize(Roles = "driver")]
        public ActionResult UpdateDriverOrder(string id)
        {
            OrderDriver orderDriver = UserService.FindOrderDriver(id);
            CreateDriverOrder createDriverOrder = new CreateDriverOrder();
            createDriverOrder.StartAddressCity = orderDriver.StartAddressCity;
            createDriverOrder.StartAddressRoad = orderDriver.StartAddressRoad;
            createDriverOrder.StartAddressHouse = orderDriver.StartAddressHouse;
            createDriverOrder.CarBrand = orderDriver.CarBrand;
            createDriverOrder.MaxWeight = orderDriver.MaxWeight;
            createDriverOrder.Id = orderDriver.Id;
            return View(createDriverOrder);

        }

        [HttpPost]
        public ActionResult UpdateUserOrder(CreateUserOrder createUserOrder)
        {
            OrderUser orderUser = new OrderUser
            {
                EndAddressCity = createUserOrder.EndAddressCity,
                EndAddressRoad = createUserOrder.EndAddressRoad,
                EndAddressHouse = createUserOrder.EndAddressHouse,
                ProductName = createUserOrder.ProductName,
                StartAddressCity =  createUserOrder.StartAddressCity,
                StartAddressRoad=  createUserOrder.StartAddressRoad,
                StartAddressHouse = createUserOrder.StartAddressHouse,
                Weight = createUserOrder.Weight,
                ApplicationUserId = User.Identity.GetUserId().ToString(),
                StartDate = createUserOrder.StartDate,
                DueDate = createUserOrder.DueDate,
                Status = true,
                Id = createUserOrder.Id
            };

            UserService.UpdateOrderUser(orderUser);
            return RedirectToAction("index", "Home");
        }

        [HttpPost]
        public ActionResult UpdateDriverOrder(CreateDriverOrder createDriverOrder)
        {
            OrderDriver orderDriver = new OrderDriver
            {
                CarBrand = createDriverOrder.CarBrand,
                MaxWeight = createDriverOrder.MaxWeight,
                StartAddressCity = createDriverOrder.StartAddressCity,
                StartAddressRoad = createDriverOrder.StartAddressRoad,
                StartAddressHouse= createDriverOrder.StartAddressHouse,
                ApplicationUserId = User.Identity.GetUserId().ToString(),
                Status = true,
                Id = createDriverOrder.Id
            };

            UserService.UpdateOrderDriver(orderDriver);
            return RedirectToAction("index", "Home");

        }

        [Authorize(Roles = "driver")]
        public ActionResult CreateDriverOrder(string id)
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action CreateDriverOrder getView";
            TempData["ID"] = id;
            logger.Info(messege);

            return View();
        }

        [Authorize(Roles = "user")]
        public ActionResult DeleteUserOrderById(string id)
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action DeleteUserOrderById Succses";
            logger.Info(messege);

            UserService.DeleteUserOrderById(id);

            return RedirectToAction("DetailsUserOrderById");
        }

        [Authorize(Roles = "driver")]
        public ActionResult DeleteDriverOrderById(string id)
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action DeleteDriverOrderById Succses";
            logger.Info(messege);
            UserService.DeleteDriverOrderById(id);

            return RedirectToAction("DetailsDriverOrderById");
        }

        [HttpPost]
        public ActionResult CreateUserOrder(CreateUserOrder createUserOrder)
        {
            if (ModelState.IsValid)
            {
                string idDriver;
                if (TempData["ID"] != null)
                {
                    idDriver = TempData["ID"].ToString();
                    TempData["ID"] = null;
                }
                else idDriver = null;
                OrderUser orderUser = new OrderUser
                {
                    EndAddressCity = createUserOrder.EndAddressCity,
                    EndAddressRoad = createUserOrder.EndAddressRoad,
                    EndAddressHouse = createUserOrder.EndAddressHouse,
                    ProductName = createUserOrder.ProductName,
                    StartAddressCity = createUserOrder.StartAddressCity,
                    StartAddressRoad = createUserOrder.StartAddressRoad,
                    StartAddressHouse = createUserOrder.StartAddressHouse,
                    Weight = createUserOrder.Weight,
                    ApplicationUserId = User.Identity.GetUserId().ToString(),
                    StartDate = createUserOrder.StartDate,
                    DueDate = createUserOrder.DueDate,
                    Status = true
                };
                UserService.CreateUserOrder(orderUser, idDriver);

                string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                    "controller: AccountController,  Action CreateUserOrder Succses";
                logger.Info(messege);
                return RedirectToAction("index", "Home");
            }
            return RedirectToAction("CreateUserOrder");
        }

        [HttpPost]
        public ActionResult CreateDriverOrder(CreateDriverOrder createDriverOrder)
        {
            string idUser;
            if (TempData["ID"] != null)
            {
                idUser = TempData["ID"].ToString();
                TempData["ID"] = null;
            }
            else idUser = null;
            OrderDriver orderDriver = new OrderDriver
            {
                CarBrand = createDriverOrder.CarBrand,
                MaxWeight = createDriverOrder.MaxWeight,
                StartAddressCity =  createDriverOrder.StartAddressCity,
                StartAddressRoad =  createDriverOrder.StartAddressRoad,
                StartAddressHouse =  createDriverOrder.StartAddressHouse,
                ApplicationUserId = User.Identity.GetUserId().ToString(),
                Status = true
            };
            UserService.CreateDriverOrder(orderDriver, idUser);
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action CreateUserOrder Succses";
            logger.Info(messege);
            return RedirectToAction("index", "Home");
        }

        [Authorize]
        public ActionResult DetailsUserOrderById(string sortOrder)
        {


            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action DetailsUserOrderById Succses";
            logger.Info(messege);

            ViewBag.EndAddressSortParm = String.IsNullOrEmpty(sortOrder) ? "EndAddress desc" : "";
            ViewBag.WeightSortParm = sortOrder == "Weight" ? "Weight desc" : "Weight";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "StartDate desc" : "StartDate";
            ViewBag.DueDateSortParm = sortOrder == "DueDate" ? "DueDate desc" : "DueDate";
            ViewBag.ProductNameSortParm = sortOrder == "ProductName" ? "ProductName desc" : "ProductName";
            ViewBag.StartAddressSortParm = sortOrder == "StartAddress" ? "StartAddress desc" : "StartAddress";
            switch (sortOrder)
            {
                case "Weight":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderBy(s => s.Weight).ToList());
                case "Weight desc":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderByDescending(s => s.Weight).ToList());
                case "ProductName":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderBy(s => s.ProductName).ToList());
                case "ProductName desc":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderByDescending(s => s.ProductName).ToList());
                case "StartDate":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderBy(s => s.StartDate).ToList());
                case "StartDate desc":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderByDescending(s => s.StartDate).ToList());
                case "DueDate":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderBy(s => s.DueDate).ToList());
                case "DueDate desc":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderByDescending(s => s.DueDate).ToList());
                case "StartAddress":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderBy(s => s.StartAddressCity).ToList());
                case "StartAddress desc":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderByDescending(s => s.StartAddressCity).ToList());
                case "EndAddress desc":
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderByDescending(s => s.EndAddressCity).ToList());
                default:
                    return View("FindOrderForDriver", UserService.GetUserDetailsById(User.Identity.GetUserId()).OrderBy(s => s.EndAddressCity).ToList());
            }

        }

        [Authorize]
        public ActionResult DetailsDriverOrderById(string sortOrder)
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action DetailsDriverOrderById Succses";
            logger.Info(messege);

            ViewBag.CarBrandSortParm = String.IsNullOrEmpty(sortOrder) ? "CarBrand desc" : "";
            ViewBag.MaxWeightSortParm = sortOrder == "MaxWeight" ? "MaxWeight desc" : "MaxWeight";
            ViewBag.StartAddressSortParm = sortOrder == "StartAddress" ? "StartAddress desc" : "StartAddress";
            switch (sortOrder)
            {
                case "MaxWeight":
                    return View("FindOrderForUser", UserService.GetDriverDetailsById(User.Identity.GetUserId()).OrderBy(s => s.MaxWeight).ToList());
                case "MaxWeight desc":
                    return View("FindOrderForUser", UserService.GetDriverDetailsById(User.Identity.GetUserId()).OrderByDescending(s => s.MaxWeight).ToList());
                case "StartAddress":
                    return View("FindOrderForUser", UserService.GetDriverDetailsById(User.Identity.GetUserId()).OrderBy(s => s.StartAddressCity).ToList());
                case "StartAddress desc":
                    return View("FindOrderForUser", UserService.GetDriverDetailsById(User.Identity.GetUserId()).OrderByDescending(s => s.StartAddressCity).ToList());
                case "CarBrand desc":
                    return View("FindOrderForUser", UserService.GetDriverDetailsById(User.Identity.GetUserId()).OrderByDescending(s => s.CarBrand).ToList());
                default:
                    return View("FindOrderForUser", UserService.GetDriverDetailsById(User.Identity.GetUserId().ToString())
                        .OrderBy(s => s.CarBrand).ToList());
            }

        }

        [Authorize(Roles = "admin")]
        public ActionResult BlockUser(string email)
        {
            UserService.BlockUser(email);

            return RedirectToAction("AllUser");
        }

        [Authorize]
        public ActionResult FindOrderForUser(string sortOrder)
        {

            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action FindOrderForUser Succses";
            logger.Info(messege);

            ViewBag.CarBrandSortParm = String.IsNullOrEmpty(sortOrder) ? "CarBrand desc" : "";
            ViewBag.MaxWeightSortParm = sortOrder == "MaxWeight" ? "MaxWeight desc" : "MaxWeight";
            ViewBag.StartAddressSortParm = sortOrder == "StartAddress" ? "StartAddress desc" : "StartAddress";
            switch (sortOrder)
            {
                case "MaxWeight":
                    return View(UserService.GetDriverDetails().OrderBy(s => s.MaxWeight).ToList());
                case "MaxWeight desc":
                    return View(UserService.GetDriverDetails().OrderByDescending(s => s.MaxWeight).ToList());
                case "StartAddress":
                    return View(UserService.GetDriverDetails().OrderBy(s => s.StartAddressCity).ToList());
                case "StartAddress desc":
                    return View(UserService.GetDriverDetails().OrderByDescending(s => s.StartAddressCity).ToList());
                case "CarBrand desc":
                    return View(UserService.GetDriverDetails().OrderByDescending(s => s.CarBrand).ToList());
                default:
                    return View(UserService.GetDriverDetails().OrderBy(s => s.CarBrand).ToList());
            }


        }

        [Authorize]
        public ActionResult FindOrderForDriver(string sortOrder)
        {

            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action FindOrderForDriver Succses";
            logger.Info(messege);

            ViewBag.EndAddressSortParm = String.IsNullOrEmpty(sortOrder) ? "EndAddress desc" : "";
            ViewBag.WeightSortParm = sortOrder == "Weight" ? "Weight desc" : "Weight";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "StartDate desc" : "StartDate";
            ViewBag.DueDateSortParm = sortOrder == "DueDate" ? "DueDate desc" : "DueDate";
            ViewBag.ProductNameSortParm = sortOrder == "ProductName" ? "ProductName desc" : "ProductName";
            ViewBag.StartAddressSortParm = sortOrder == "StartAddress" ? "StartAddress desc" : "StartAddress";
            switch (sortOrder)
            {
                case "Weight":
                    return View(UserService.GetUserDetails().OrderBy(s => s.Weight).ToList());
                case "Weight desc":
                    return View(UserService.GetUserDetails().OrderByDescending(s => s.Weight).ToList());
                case "ProductName":
                    return View(UserService.GetUserDetails().OrderBy(s => s.ProductName).ToList());
                case "ProductName desc":
                    return View(UserService.GetUserDetails().OrderByDescending(s => s.ProductName).ToList());
                case "StartDate":
                    return View(UserService.GetUserDetails().OrderBy(s => s.StartDate).ToList());
                case "StartDate desc":
                    return View(UserService.GetUserDetails().OrderByDescending(s => s.StartDate).ToList());
                case "DueDate":
                    return View(UserService.GetUserDetails().OrderBy(s => s.DueDate).ToList());
                case "DueDate desc":
                    return View(UserService.GetUserDetails().OrderByDescending(s => s.DueDate).ToList());
                case "StartAddress":
                    return View(UserService.GetUserDetails().OrderBy(s => s.StartAddressCity).ToList());
                case "StartAddress desc":
                    return View(UserService.GetUserDetails().OrderByDescending(s => s.StartAddressCity).ToList());
                case "EndAddress desc":
                    return View(UserService.GetUserDetails().OrderByDescending(s => s.EndAddressCity).ToList());
                default:
                    return View(UserService.GetUserDetails().OrderBy(s => s.EndAddressCity).ToList());
            }

        }

        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO
            {
                Email = "somemail@mail.ru",
                UserName = "somemail@mail.ru",
                Password = "ad46D_ewr3",
                Name = "Семен Семенович Горбунков",
                Address = "ул. Спортивная, д.30, кв.75",
                Role = "admin",
            }, new List<string> { "user", "driver", "admin" });

            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: AccountController,  Action SetInitialDataAsync Succses";
            logger.Info(messege);
        }
    }
}