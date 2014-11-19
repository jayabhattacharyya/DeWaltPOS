using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeWalt;
using DeWaltPOS.Models;

namespace DeWaltPOS.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            ViewBag.Path = Url.Content("~/Images/dewalt-old.jpg");
            return View();
        }

        [HttpPost]
        public ActionResult Save(Product frm)
        {

            HttpPostedFileBase tfile = Request.Files["thumb"];
            HttpPostedFileBase cfile = Request.Files["catalg"];

            int pid = frm.id;

            ViewBag.pid = pid;

            string path = "";
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            ThumbImage th = new ThumbImage();
            CatalogueImage ci = new CatalogueImage(); 
            string sku = dc.Products.FirstOrDefault(p => p.id == pid).sku;

            if (ModelState.IsValid)
            {
                frm.created = DateTime.Now;
                frm.modified = DateTime.Now;
                
                
                if (frm.id == 0)
                {
                    dc.Products.Add(frm);
                }
                else
                {
                    dc.Entry(frm).State = EntityState.Modified;
                }
                
            }
            if (tfile.ContentLength > 0)
            {
                var ext = Path.GetExtension(tfile.FileName);
                path = Path.Combine(Server.MapPath("~" + Globals.ImageThumbSavePath), sku + ext);
                tfile.SaveAs(path);
                path = Path.Combine(Server.MapPath("~" + Globals.ImageTempSavePath), Path.GetFileName(tfile.FileName));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                th = dc.ThumbImages.SingleOrDefault(p => p.prodid == pid && p.language == Globals.lang);
                if (th == null)
                {
                    th = new ThumbImage();
                    th.prodid = pid;
                    th.language = Globals.lang;
                    th.image = Path.Combine(Globals.ImageThumbSavePath, sku + ext);
                    dc.ThumbImages.Add(th);
                }
                else
                {

                    th.image = Path.Combine(Globals.ImageThumbSavePath, sku + ext);
                }
            }

            if (cfile.ContentLength > 0)
            {
                var ext = Path.GetExtension(cfile.FileName);
                ci = dc.CatalogueImages.SingleOrDefault(p => p.prodid == pid && p.language == Globals.lang);
                path = Path.Combine(Server.MapPath("~" + Globals.ImageCatalogueSavePath), sku + ext);
                cfile.SaveAs(path);
                path = Path.Combine(Server.MapPath("~" + Globals.ImageTempSavePath), Path.GetFileName(tfile.FileName));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                if (ci == null)
                {
                    ci = new CatalogueImage();
                    ci.prodid = pid;
                    ci.language = Globals.lang;
                    ci.image = Path.Combine(Globals.ImageCatalogueSavePath, sku + ext);
                    dc.CatalogueImages.Add(ci);
                }
                else
                {
                    ci.image = Path.Combine(Globals.ImageCatalogueSavePath, sku + ext);
                }
            }

            dc.SaveChanges();

            return View("Index");
        }




    }
}
