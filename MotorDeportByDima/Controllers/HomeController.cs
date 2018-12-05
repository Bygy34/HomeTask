using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MotorDeportByDima.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotorDeportByDima.Controllers
{
    public class HomeController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        //private readonly ILogger _logger;
        //public HomeController(ILogger logger)
        //{
        //    _logger = logger;
        //}
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        public ActionResult Index()
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: HomeController,  ActionResult Index";
            logger.Info(messege);
            return View(UserService.GetOrders().ToList());

        }


        public ActionResult Contact()
        {
            int[] mas = new int[2];
            mas[6] = 4;
            return View();
        }

        //[Authorize(Roles = "admin")]
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult UserProfile()
        //{
        //    return View();
        //}


        // // [Authorize(Roles = "user")]
        // public ActionResult AboutUser()
        // {
        //     return View(UserService.GetDriverDetails().ToList());
        // }

        //// [Authorize(Roles = "driver")]
        // public ActionResult AboutDriver()
        // {
        //     return View(UserService.GetUserDetails().ToList());
        // }

        public ActionResult CreateOrderByDriver(string id)
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: HomeController,  ActionResult CreateOrderByDriver";
            logger.Info(messege);
            TempData["ID"] = id;
            return RedirectToAction("CreateUserOrder", "Account");
        }

        public ActionResult CreateOrderByUser(string id)
        {
            string messege = "user Id:" + User.Identity.GetUserId() + "User Name: " + User.Identity.Name +
                "controller: HomeController,  ActionResult CreateOrderByDriver";
            logger.Info(messege);
            TempData["ID"] = id;
            return RedirectToAction("CreateDriverOrder", "Account");
        }





    }
}