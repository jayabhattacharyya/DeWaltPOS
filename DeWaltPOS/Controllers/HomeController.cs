using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeWaltPOS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.bread = "Home";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Test1()
        {
            ViewBag.Message = "Your app description page.";

            return View("Test1");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error(string message)
        {
            string err = Response.StatusCode.ToString() + "- " +  Response.StatusDescription + message;
            ViewBag.error = err;
            return View();
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    if (!filterContext.ExceptionHandled)
        //    {
        //        string controller = filterContext.RouteData.Values["controller"].ToString();
        //        string action = filterContext.RouteData.Values["action"].ToString();
        //        Exception ex = filterContext.Exception;
        //        //do something with these details here
        //        RedirectToAction("Error", "Home", new {message = "ABC"});
        //    }
        //}
    }
}
