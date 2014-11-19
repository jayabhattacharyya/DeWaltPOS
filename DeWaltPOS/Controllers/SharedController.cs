using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeWaltPOS.Models;

namespace DeWaltPOS.Controllers
{
    public class SharedController : Controller
    {
        //
        // GET: /Shared/

        public ActionResult Index()
        {
            return View();
        }

        public string GetImage(int id)
        {
            string imageurl = "";
            using(DeWaltPOSEntities dc = new DeWaltPOSEntities()){

                imageurl = "~" + dc.ThumbImages.FirstOrDefault(i => i.id == id).image;
            }
            return imageurl;

        }

    }
}
