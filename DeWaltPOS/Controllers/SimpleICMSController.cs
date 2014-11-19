using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeWaltPOS.Models;
using System.Web.Helpers;
using DeWaltPOS.Code;
using System.Web.Hosting;


namespace DeWalt.Controllers
{
    public class SimpleICMSController : Controller
    {
        //
        // GET: /SimpleICMS/


        [HttpPost]
        public JsonResult AddImage()
        {
            int id = 18;
            HttpPostedFileBase file = Request.Files["addpic"];
            string path = "";
            string retpath = "";
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~" + Globals.ImageTempSavePath), fileName);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                try
                {
                    WebImage cimage = StandingMethods.ResizeImage(file, 1000);
                    cimage.Save(path);
                }
                catch
                {

                }

                retpath = Globals.ImageTempSavePath + fileName; ;
            }


            return Json(new { pth = retpath }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProdData(int pid)
        {
            //int pid = frm["ProdList"] == null ? 18 : int.Parse(frm["ProdList"].ToString());
            string tpth = "";
            string cpth = "";

            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                tpth = dc.ThumbImages.FirstOrDefault(p => p.prodid == pid) == null ? Globals.DefaultImage : dc.ThumbImages.FirstOrDefault(p => p.prodid == pid).image;
                cpth = dc.CatalogueImages.FirstOrDefault(p => p.prodid == pid) == null ? Globals.DefaultImage : dc.CatalogueImages.FirstOrDefault(p => p.prodid == pid).image;
            }
                
            return Json(new { thumbpth = tpth, catlgpath = cpth }, JsonRequestBehavior.AllowGet);
        }

        public void DeleteTempImages()
        {
            var tempfilesdir = HostingEnvironment.MapPath("~" + Globals.ImageTempSavePath);
            string[] picList = Directory.GetFiles(tempfilesdir);
            foreach (string fileName in picList)
                System.IO.File.Delete(fileName);

        }


        public JsonResult GetDta(int pid)
        {
            string tpth = "";
            string cpth = "";

            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                tpth = dc.ThumbImages.FirstOrDefault(p => p.prodid == pid) == null ? Globals.DefaultImage : dc.ThumbImages.FirstOrDefault(p => p.prodid == pid).image;
                cpth = dc.CatalogueImages.FirstOrDefault(p => p.prodid == pid) == null ? Globals.DefaultImage : dc.CatalogueImages.FirstOrDefault(p => p.prodid == pid).image;
            }
            return Json(new { thumbpth = tpth, catlgpath = cpth }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDeWaltProd(string sku)
        {
            string nid = "";
            string nsku = "";

            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                Product nprod = new Product();
                nprod.sku = sku;
                nprod.Brand = 1;
                dc.Products.Add(nprod);
                dc.SaveChanges();
                nid = nprod.id.ToString();
                nsku = nprod.sku;
            }
            return Json(new { id = nid, sku = nsku }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ResizeImages()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResizeImages(int id)
        {
            
            return View();
        }

    }
}
