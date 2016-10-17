using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to SignalR Server";

            return View();
        }

        public ActionResult Chat()
        {
            return View("Chat");
        }
    }
}
