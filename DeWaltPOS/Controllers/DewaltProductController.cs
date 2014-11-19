using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeWaltPOS.Models;
using System.IO;
using DeWalt;
using System.Web.Helpers;
using System.Drawing;
using System.Web.Hosting;
using DeWaltPOS.Code;

namespace DeWaltPOS.Controllers
{
    public class DewaltProductController : Controller
    {
        //private DeWaltPOSEntities db = new DeWaltPOSEntities();

        //
        // GET: /DewaltProduct/

        [Authorize] 
        public ActionResult Index()
        {
            ViewBag.bread = "dispprod";
            return View();
        }

        
        public ActionResult ProductDisplay(int? type, int? cat, int? paneltype, int? besttype, string sku="")
        {
            List<Product> prods = new List<Product>();
            string vwname = "";
            using (DeWaltPOSEntities db = new DeWaltPOSEntities())
            {
                prods = db.Products.OrderBy(p => p.productType).Include("Thumbsmalls").Include("ProductType1").Include("PanelType1").Include("BestPracticeType").ToList();
                if(type == 1)
                {
                    prods = prods.Where(p => p.productType == type).ToList();
                    vwname = "SuperSkuDisplay";
                    ViewBag.bread = "recco";

                }

                if (type == 2)
                {
                    prods = prods.Where(p => p.productType == type).ToList();
                    ViewBag.bread = "globfix";
                    vwname = "FixtureDisplay";

                }
                
                if(type == 3)
                {
                    ViewBag.bread = "dispprod";
                    vwname = "ProductDisplay";
                    prods = prods.Where(p => p.productType > 2 && p.Brand == null).ToList();
                }

                if (type == 4)
                {
                    ViewBag.bread = "dwprod";
                    vwname = "DeWaltDisplay";
                    prods = prods.Where(p => p.Brand == 1).ToList();
                }


                if (sku != "")
                {
                    prods = prods.Where(p => p.sku.ToUpper().Contains(sku.ToUpper())).ToList();
                }

                if (cat != null && cat != 0)
                {
                    prods = prods.Where(p => p.productType==cat).ToList();
                }

                if (paneltype != null && paneltype != 0)
                {
                    prods = prods.Where(p => p.PanelType == paneltype).ToList();
                }

                if (besttype != null && besttype != 0)
                {
                    prods = prods.Where(p => p.BestPracticeId == besttype).ToList();
                }
            }

            return PartialView(vwname, prods);
        }

        [Authorize]
        public ActionResult DProduct()
        {
            ViewBag.bread = "dewaltprod";
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            return View("DewaltProduct", db.Products.Where(p =>p.Brand == 1).ToList());
        }
        [Authorize]
        public ActionResult SuperSkus()
        {
            ViewBag.bread = "recco";
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            return View(db.Products.Where(t => t.productType == 1).OrderBy(p => p.BestPracticeId).ToList());
        }
        [Authorize]
        public ActionResult GlobalFixtures()
        {
            ViewBag.bread = "globfix";
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            return View(db.Products.Where(t => t.productType == 2).OrderBy(p => p.PanelType).ToList());
        }

        [Authorize]
        public ActionResult DeWaltProducts()
        {
            ViewBag.bread = "dwprod";
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            return View(db.Products.Where(t => t.Brand == 1).OrderByDescending(p => p.id).ToList());
        }


        //
        // GET: /DewaltProduct/Details/5

        public ActionResult Details(int id = 0)
        {
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /DewaltProduct/Create

        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            if (User.Identity.Name != "admin")
            {
                return RedirectToAction("Index");
            }
            ViewBag.bread = "dispprod";
            ViewBag.type = 3;
            return View();
        }

        public ActionResult CreateFixture()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            //if (User.Identity.Name != "admin")
            //{
            //    return RedirectToAction("Index");
            //}
            ViewBag.bread = "globfix";
            ViewBag.type = 2;
            return View("Create");
        }

        public ActionResult CreateSuper()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            //if (User.Identity.Name != "admin")
            //{
            //    return RedirectToAction("Index");
            //}
            ViewBag.bread = "recco";
            ViewBag.type = 1;
            return View();
        }

        [Authorize]
        public ActionResult CreateDeWalt()
        {

            ViewBag.bread = "dwprod";
            ViewBag.type = 4;
            ViewBag.brand = 1;
            return View();
        }

        //
        // POST: /DewaltProduct/Create

        [HttpPost]
        [Authorize]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, FormCollection frm, IEnumerable<HttpPostedFileBase> cfiles, IEnumerable<DeWaltPOS.Models.VMRelatedProducts> relprods)
        {
            int prodtype = 0;
            if (frm["ProductType"] != null)
            {
                prodtype = int.Parse(frm["ProductType"].ToString());
            }

            string returnview = "";

            if(product.Brand == 1)
            {
                returnview = "CreateDeWalt";
            }
            else
            {
                if(prodtype == 1)
                {
                    returnview = "CreateSuper";
                }
                else
                {
                    returnview = "Create";
                }
            }
            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {

                    dc.Products.Add(product);
                    if (prodtype == 1)
                    {
                        product.height = 0;
                        product.length = 0;
                        product.width = 0;
                        product.price = 0;
                        product.IsBestPractice = true;
                    }
                    
                    try
                    {
                        dc.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        ViewBag.error = ex.InnerException.GetBaseException().Message.ToString();
                        return View(returnview, product);
                    }

                    SaveRelatedProducts(dc, frm, product.id);
                    if (prodtype > 2) 
                    {
                        SaveRelatedPanels(dc, frm, product.id);
                    }

                   
                    
                    if (product.productType == 1)
                    {
                        if (cfiles != null)
                        {
                            SaveSuperCatlgImages(dc, product, frm, cfiles);
                        }
                    }
                    else
                    {
                        SaveImages(dc, product, Request);
                    }

                    dc.SaveChanges();

                    if(product.Brand == 1)
                    {
                        return RedirectToAction("DewaltProducts");
                    } 

                    if (prodtype == 1)
                    {
                        return RedirectToAction("SuperSkus");
                    }
                    else if (product.productType == 2)
                    {
                        return RedirectToAction("GlobalFixtures");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }

        }

        public ActionResult Edit(int id = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            //if (User.Identity.Name != "admin")
            //{
            //    return RedirectToAction("Index");
            //}

            Product product = new Product();
            using( DeWaltPOSEntities db = new DeWaltPOSEntities())
            {
                product = db.Products.Include("ThumbImages").Include("CatalogueImages").FirstOrDefault(p => p.id == id);

            }

            if (product == null)
            {
                return HttpNotFound();
            }
            if (product.productType == 2)
            {
                ViewBag.bread = "globfix";
            }
            else
            {
                ViewBag.bread = "dispprod";
            }
            return View(product);
        }

        [Authorize] 
        public ActionResult Home()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult PanelsChoice(int id =0)
        {
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            var rels = db.RelatedPanels.Where(p => p.ProductId == id).Join(db.PanelTypes, r => r.PanelId , p =>p.PanelId, (r,p) => new {r,p}).Select(j => j.p);
            var nrels = db.PanelTypes.AsEnumerable().Except(rels).OrderBy(p => p.PanelId).ToList();
            return PartialView(nrels);
        }

        [ChildActionOnly]
        public ActionResult RelatedPanels(int id = 0)
        {
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            var prods = db.RelatedPanels.Where(p => p.ProductId == id).Include("PanelType").ToList();
            if (prods != null)
            {
                return PartialView(prods);
            }
            else
            {
                return null;
            }
        }

        public ActionResult TempRelatedPanels(int panelid = 0)
        {
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            var prods = db.PanelTypes.FirstOrDefault(p => p.PanelId == panelid);
            if (prods != null)
            {
                return PartialView(prods);
            }
            else
            {
                return null;
            }
        }

        public ActionResult GlobalFixturesEdit(int id = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            //if (User.Identity.Name != "admin")
            //{
            //    return RedirectToAction("Index");
            //}

            Product product = new Product();
            using (DeWaltPOSEntities db = new DeWaltPOSEntities())
            {
                product = db.Products.Include("ThumbImages").Include("CatalogueImages").FirstOrDefault(p => p.id == id);

            } 
            
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.bread = "globfix";
            return View("GlobalFixturesEdit", product);
        }


        public ActionResult SuperSkuEdit(int id = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            //if (User.Identity.Name != "admin")
            //{
            //    return RedirectToAction("Index");
            //}
            Product product = new Product();
            using (DeWaltPOSEntities db = new DeWaltPOSEntities())
            {
                product = db.Products.Include("ThumbImages").Include("CatalogueImages").FirstOrDefault(p => p.id == id);

            }

            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.bread = "recco";
            return View("SuperSkuEdit", product);
        }

        [Authorize]
        public ActionResult DeWaltProductEdit(int id = 0)
        {
            Product product = new Product();
            using (DeWaltPOSEntities db = new DeWaltPOSEntities())
            {
                product = db.Products.Include("ThumbImages").Include("CatalogueImages").FirstOrDefault(p => p.id == id);

            }
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.bread = "dwprod";
            ViewBag.brand = 1;
            return View(product);
        }

        //
        // POST: /DewaltProduct/Edit/5

        [HttpPost]
        [Authorize]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, FormCollection frm, IEnumerable<HttpPostedFileBase> cfiles, IEnumerable<DeWaltPOS.Models.VMRelatedProducts> relprods)
        {
            using(DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {

                dc.Entry(product).State = EntityState.Modified;
                dc.SaveChanges();
                
                SaveRelatedProducts(dc, frm, product.id);

                if (product.productType > 2)
                {
                    SaveRelatedPanels(dc, frm, product.id);
                }

                // Save Images - Multiple Catalogues for Super Skus and one thumb, one catalog for other products.
                if (product.productType == 1)
                {
                    SaveSuperCatlgImages(dc, product, frm, cfiles);
                }
                else
                {
                    SaveImages(dc, product, Request);
                }
                dc.SaveChanges();


                // Send tags in Viewbag.bread to create highlights in breadcrumb to ascertain current location in the website
                if(product.Brand == 1)
                {
                    ViewBag.bread = "dwprod";
                    return RedirectToAction("DeWaltProducts");
                }

                if(product.productType ==1)
                {
                    ViewBag.bread = "recco";
                    return RedirectToAction("SuperSkus");
                }
                else if(product.productType == 2)
                {
                    ViewBag.bread = "globfix";
                    return RedirectToAction("GlobalFixtures");
                }
                else
                {
                    ViewBag.bread = "dewaltprod";
                    return RedirectToAction("Index");
                }                
                           

            }

            return View(product);
        }

        public void SaveRelatedPanels(DeWaltPOSEntities dc, FormCollection frm, int id)
        {
            DeleteRelatedPanels(dc, frm, id);
   
            var chk = frm["pchk"] == null ? new List<string>() : frm["pchk"].Split(',').ToList();
            foreach (var c in chk)
            {
                RelatedPanel rp = new RelatedPanel();
                int r = int.Parse(c);
                rp.ProductId = id;
                rp.PanelId = r;
                dc.RelatedPanels.Add(rp);
            }
        }

        public void DeleteRelatedPanels(DeWaltPOSEntities dc, FormCollection frm, int id)
        {
            var nchk = frm["npchk"] == null ? new List<string>() : frm["nchk"].Split(',').ToList();

            foreach (var c in nchk)
            {
                RelatedPanel rp = new RelatedPanel();
                int ri = int.Parse(c);
                rp = dc.RelatedPanels.FirstOrDefault(r => r.ProductId == id && r.PanelId == ri);
                dc.RelatedPanels.Remove(rp);
            }
        }

        public void DeleteRelatedProducts(DeWaltPOSEntities dc, FormCollection frm, int id)
        {
            var nchk = frm["nchk"] == null ? new List<string>() : frm["nchk"].Split(',').ToList();
            Product prod = dc.Products.SingleOrDefault(p => p.id == id);
            foreach (var c in nchk)
            {
                RelatedProduct rp = new RelatedProduct();
                int ri = int.Parse(c);

                if((prod.Brand != null)||(prod.productType == 1))
                {
                    rp = dc.RelatedProducts.FirstOrDefault(r => r.ProductId == id && r.RelatedProductId== ri);
                }
                else
                {
                    rp = dc.RelatedProducts.FirstOrDefault(r => r.ProductId == ri && r.RelatedProductId == id);
                }
                if (rp != null)
                {
                    dc.RelatedProducts.Remove(rp);
                }
            }
        }

        public void SaveRelatedProducts(DeWaltPOSEntities dc, FormCollection frm, int id)
        {
            DeleteRelatedProducts(dc, frm, id);

            var chk = frm["chk"] == null ? new List<string>() : frm["chk"].Split(',').ToList();
            foreach (var c in chk)
            {
                int rid = int.Parse(c);
                string qty = frm[c];

                int? relbrand = dc.Products.SingleOrDefault(p => p.id == rid).Brand;
                int? currprodtype = dc.Products.SingleOrDefault(p => p.id == id).productType;
                RelatedProduct rp = new RelatedProduct();
                if(relbrand != null)
                {
                    rp.ProductId = rid;
                    rp.RelatedProductId = id;
                }
                else
                {
                    rp.ProductId = id;
                    rp.RelatedProductId = rid;
                }
                rp.Quantity = int.Parse(qty);
                dc.RelatedProducts.Add(rp);
            }

        }

        public void SaveImages(DeWaltPOSEntities dc, Product product, HttpRequestBase Request)
        {
            HttpPostedFileBase tfile = Request.Files["thumb"];
            HttpPostedFileBase cfile = Request.Files["catalg"];

            int pid = product.id;

            ViewBag.pid = pid;

            string path = "";
            string spath = "";

            ThumbImage th = new ThumbImage();
            Thumbsmall ts = new Thumbsmall();
            CatalogueImage ci = new CatalogueImage();
            string sku = dc.Products.FirstOrDefault(p => p.id == pid).sku;
            sku = sku.Replace(" ", "_");
            sku = sku + DateTime.Now.Ticks.ToString();

            if (tfile.ContentLength > 0)
            {
                var ext = Path.GetExtension(tfile.FileName);
                path = Path.Combine(Server.MapPath("~" + Globals.ImageThumbSavePath), sku + ext);
                spath = Path.Combine(Server.MapPath("~" + Globals.ImageThumbSmallPath), sku + ext);
                WebImage timage = StandingMethods.ResizeImage(tfile, 1000);
                timage.Save(path);
                WebImage simg = StandingMethods.ResizeImage(timage, 100);
                simg.Save(spath);

                th = dc.ThumbImages.SingleOrDefault(p => p.prodid == pid && p.language == Globals.lang);
                ts = dc.Thumbsmalls.FirstOrDefault(p => p.prodid == pid && p.sequence == 1);
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

                if (ts == null)
                {
                    ts = new Thumbsmall();
                    ts.prodid = pid;
                    ts.sequence = 1;
                    ts.image = Path.Combine(Globals.ImageThumbSmallPath, sku + ext);
                    dc.Thumbsmalls.Add(ts);
                }
                else
                {

                    ts.image = Path.Combine(Globals.ImageThumbSmallPath, sku + ext);
                }
            }

            if (cfile.ContentLength > 0)
            {
                var ext = Path.GetExtension(cfile.FileName);
                ci = dc.CatalogueImages.SingleOrDefault(p => p.prodid == pid && p.language == Globals.lang);
                path = Path.Combine(Server.MapPath("~" + Globals.ImageCatalogueSavePath), sku + ext);
                WebImage cimage = StandingMethods.ResizeImage(cfile, 400);
                cimage.Save(path);

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

            StandingMethods.DeleteTempImages();
        }



        public void SaveSuperCatlgImages(DeWaltPOSEntities dc, Product product, FormCollection frm, IEnumerable<HttpPostedFileBase> cfiles)
        {

            int pid = product.id;

            ViewBag.pid = pid;

            string path = "";
            string spath = "";

            ThumbImage th = new ThumbImage();
            Thumbsmall ts = new Thumbsmall();
            CatalogueImage ci = new CatalogueImage();
            string sku = dc.Products.FirstOrDefault(p => p.id == pid).sku;

            var delctlgs = frm["delctlg"] == null ? new List<string>() : frm["delctlg"].Split(',').ToList();
            foreach (var dctlg in delctlgs)
            {
                int seq = int.Parse(dctlg);
                CatalogueImage ctlg;
                if(seq == 0)
                {
                    ctlg = dc.CatalogueImages.FirstOrDefault(c => c.prodid == pid && c.sequence == null);
                }
                else
                {
                    ctlg = dc.CatalogueImages.FirstOrDefault(c => c.prodid == pid && c.sequence == seq);
                }
                Thumbsmall thmbsml = dc.Thumbsmalls.FirstOrDefault(t => t.prodid == pid && t.sequence == seq);
                if (ctlg != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~" + ctlg.image)))
                        System.IO.File.Delete(Server.MapPath("~" + ctlg.image));
                    dc.CatalogueImages.Remove(ctlg);

                }

                if(thmbsml != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~" + thmbsml.image)))
                        System.IO.File.Delete(Server.MapPath("~" + thmbsml.image));
                    dc.Thumbsmalls.Remove(thmbsml);
                }
            }

            sku = sku.Replace(" ", "_");
            int lstcatlgsequence = 0;
            var ctlgs = dc.CatalogueImages.Where(c => c.prodid == pid);
            if( ctlgs.Count() != 0)
            {
              lstcatlgsequence =  dc.CatalogueImages.Where(c => c.prodid == pid).OrderByDescending(c => c.sequence).FirstOrDefault().sequence ?? 0;
            }
            if(cfiles != null)
            {
                foreach (HttpPostedFileBase cfile in cfiles)
                {
                    if (cfile != null)
                    {
                        lstcatlgsequence++;
                        string savedfilename = sku + "_" + lstcatlgsequence.ToString();
                        var ext = Path.GetExtension(cfile.FileName);
                        path = Path.Combine(Server.MapPath("~" + Globals.ImageCatalogueSavePath), savedfilename + ext);
                        spath = Path.Combine(Server.MapPath("~" + Globals.ImageThumbSmallPath), savedfilename + ext);

                        WebImage cimage = StandingMethods.ResizeImage(cfile, 1000);
                        cimage.Save(path);

                        path = Path.Combine(Server.MapPath("~" + Globals.ImageTempSavePath), Path.GetFileName(cfile.FileName));

                        ci = new CatalogueImage();
                        ci.prodid = pid;
                        ci.language = Globals.lang;
                        ci.sequence = lstcatlgsequence;
                        ci.image = Path.Combine(Globals.ImageCatalogueSavePath, savedfilename + ext);
                        dc.CatalogueImages.Add(ci);

                        ts = dc.Thumbsmalls.FirstOrDefault(p => p.prodid == pid);

                        ts = new Thumbsmall();
                        ts.prodid = pid;
                        ts.sequence = lstcatlgsequence;
                        ts.image = Path.Combine(Globals.ImageThumbSmallPath, savedfilename + ext);
                        dc.Thumbsmalls.Add(ts);
                        WebImage simg = StandingMethods.ResizeImage(cimage, 100);
                        simg.Save(spath);

                        simg.Save(spath);

                    }
                }
            }
            StandingMethods.DeleteTempImages();        
        }

        [ChildActionOnly]
        public ActionResult RelatedProducts(bool super, int id, string type = "")
        {
            IEnumerable<RelatedProduct> prelprods;
            List<VMRelatedProducts> vmrelprods = new List<VMRelatedProducts>();
            ViewBag.type = type;

            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                Product product = dc.Products.SingleOrDefault(p => p.id == id);

                if ((type=="DW") || (type == "VS"))
                {
                    prelprods = dc.RelatedProducts.Where(r => r.ProductId == id);
                }
                else
                {
                    prelprods = dc.RelatedProducts.Where(r => r.RelatedProductId == id);
                }

                foreach (var item in prelprods)
                {
                    VMRelatedProducts vm = new VMRelatedProducts();
                    Thumbsmall thmb = new Thumbsmall();
                    Product relprod = item.Product1;
                    vm.Quantity = item.Quantity;


                    if ((type == "DW") || (type == "VS"))
                    {
                        vm.ProductId = item.ProductId;
                        vm.Description = item.Product1.Description;
                        vm.RelatedProductId = item.RelatedProductId;
                        vm.sku = dc.Products.FirstOrDefault(p => p.id == item.RelatedProductId).sku;
                        vm.ProductType = dc.Products.SingleOrDefault(p => p.id == item.RelatedProductId).productType == null ? "" : dc.Products.SingleOrDefault(p => p.id == item.RelatedProductId).ProductType1.name;
                        thmb = dc.Thumbsmalls.SingleOrDefault(t => t.prodid == item.RelatedProductId);
                        vm.ThumbImage = thmb == null ? "" : thmb.image;
                        vmrelprods.Add(vm);
                    }
                    else
                    {
                        int? prodtype = dc.Products.SingleOrDefault(p => p.id == item.ProductId).productType;
                        if(prodtype != 1)
                        {
                            vm.ProductId = item.RelatedProductId;
                            vm.Description = item.Product.Description;
                            vm.RelatedProductId = item.ProductId;
                            vm.sku = dc.Products.FirstOrDefault(p => p.id == item.ProductId).sku;
                            vm.ProductType = dc.Products.SingleOrDefault(p => p.id == item.ProductId).productType == null ? "" : dc.Products.SingleOrDefault(p => p.id == item.ProductId).ProductType1.name;
                            thmb = dc.Thumbsmalls.FirstOrDefault(t => t.prodid == item.ProductId);
                            vm.ThumbImage = thmb == null ? "" : thmb.image;
                            vmrelprods.Add(vm);
                        }

                    }


                }

            }


            if (vmrelprods!= null)
            {
                var vmord = vmrelprods.OrderBy(r => r.ProductType).ToList();
                return PartialView(vmord);
            }
            else
            {
                return PartialView("");
            }
        }

        public ActionResult TempArray(List<string> arr, List<string> brr)
        {
            var abc = arr;
            return View();

        }

        public ActionResult TempRelatedProducts(List<int> rids, List<int> qtys, string type = "")
        {

            List<VMRelatedProducts> vmrelprods = new List<VMRelatedProducts>();
            ViewBag.type = type;
            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {

                for (int idx = 0; idx < rids.Count; idx++)
                {
                    int relid = rids[idx];
                    int qty = qtys[idx];
                    VMRelatedProducts vm = new VMRelatedProducts();
                    Thumbsmall thmb = new Thumbsmall();
                    vm.ProductId = 0;
                    vm.RelatedProductId = relid;
                    vm.Quantity = qty;

                    Product relprod = dc.Products.SingleOrDefault(p => p.id == relid);
                    vm.sku = relprod.sku;

                    vm.ProductType = relprod.productType == null ? "" : relprod.ProductType1.name;
                    vm.Description = relprod.Description;
                    thmb = dc.Thumbsmalls.SingleOrDefault(t => t.prodid == relid);
                    vm.ThumbImage = thmb == null ? "" : thmb.image;
                    vmrelprods.Add(vm);
                }

            }
            if (vmrelprods != null)
            {
                return PartialView("TempRelatedProducts", vmrelprods);
            }
            else
            {
                return PartialView("TempRelatedProducts", "");
            }

        }


        [ChildActionOnly]
        public ActionResult SuperProductDisplay(int id)
        {
            ViewBag.id = id;
            var img = "";
            using(DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                var prod = dc.Products.FirstOrDefault(p => p.id == id);
                img = prod.CatalogueImages.FirstOrDefault(t => t.language ==  Globals.lang).image;
            }
            ViewBag.img = img;
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult RelProdDisplayPanel(bool super, int id, int brand = 0)
        {
            IEnumerable<Product> relprods;
            IEnumerable<RelatedProduct> prelprods;
            List<VMRelatedProducts> vmrelprods = new List<VMRelatedProducts>();
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            prelprods = dc.RelatedProducts.Where(r => r.ProductId == id);
            foreach (var item in prelprods)
            {
                VMRelatedProducts vm = new VMRelatedProducts();
                ThumbImage thmb = new ThumbImage();
                Product relprod = item.Product1;
                vm.ProductId = item.ProductId;
                vm.RelatedProductId = item.RelatedProductId;
                vm.Quantity = item.Quantity;
                vm.sku = dc.Products.FirstOrDefault(p => p.id == item.RelatedProductId).sku;
                thmb = dc.ThumbImages.SingleOrDefault(t => t.prodid == item.RelatedProductId);
                //vm.ThumbImage = dc.ThumbImages.SingleOrDefault(t => (t.id == item.RelatedProductId) && t.language == 1) == null ? "" : dc.ThumbImages.SingleOrDefault(t => (t.id == item.RelatedProductId) && t.language == 1).image;
                vm.ThumbImage = thmb == null ? "" : thmb.image;
                vmrelprods.Add(vm);
            }

            if (vmrelprods != null)
            {
                return PartialView(vmrelprods);
            }
            else
            {
                return PartialView("");
            }
        }


        public ActionResult NonRelatedProducts(int id, string type = "")
        {
            IEnumerable<Product> relprods;
            IList<Product> nrelprods =  new List<Product> ();
            List<VMRelatedProducts> vmrelprods = new List<VMRelatedProducts>();
            ViewBag.type = type;
            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                Product prod = dc.Products.FirstOrDefault(p => p.id == id);

                if ((type == "DW") || (type == "VS"))
                {
                    relprods = dc.RelatedProducts.Where(r => r.ProductId == id).Join(dc.Products, r => r.RelatedProductId, p => p.id, (r, p) => new { r, p }).Select(j => j.p);
                }
                else
                {
                    relprods = dc.RelatedProducts.Where(r => r.RelatedProductId == id).Join(dc.Products, r => r.ProductId, p => p.id, (r, p) => new { r, p }).Select(j => j.p);
                }

                if ((type == "DW") || (type == "VS"))
                {
                    nrelprods = dc.Products.Where(p => p.productType > 1).AsEnumerable().Except(relprods).OrderBy(p => p.productType).ToList();
                }

                if (type == "VN")
                {
                    nrelprods = dc.Products.Where(p => p.Brand > 0).AsEnumerable().Except(relprods).OrderBy(p => p.sku).ToList();
                }

                //if(prod.Brand == null)
                //{
                //    if(prod.productType > 1)
                //    {
                //        ViewBag.Type = "VN";
                //    }
                //    else
                //    {
                //        ViewBag.Type = "VS";
                //    }
                //}

                //if(prod.Brand == 1)
                //{
                //    ViewBag.Type = "DW";
                //}


                ViewBag.Type = type;
         
            foreach (var item in nrelprods)
            {
                VMRelatedProducts vm = new VMRelatedProducts();
                Thumbsmall thmb = new Thumbsmall();
                Product relprod = item.Product1;
                vm.ProductId = id;
                vm.RelatedProductId = item.id;
                vm.Quantity = 1;
                vm.sku = item.sku;
                
                vm.ProductType = item.productType ==  null? "" : item.ProductType1.name;
                thmb = dc.Thumbsmalls.SingleOrDefault(t => t.prodid == item.id);
                //vm.ThumbImage = dc.ThumbImages.SingleOrDefault(t => (t.id == item.RelatedProductId) && t.language == 1) == null ? "" : dc.ThumbImages.SingleOrDefault(t => (t.id == item.RelatedProductId) && t.language == 1).image;
                vm.ThumbImage = thmb == null ? "" : thmb.image;
                vmrelprods.Add(vm);
            }

            }
            if (nrelprods != null)
            {
                return PartialView("NonRelatedProducts", vmrelprods);
            }
            else
            {
                return PartialView("NonRelatedProducts", "");
            }
        }


        public ActionResult ResizeImages()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResizeImages(FormCollection frm)
        {
            string path = "";
            using(DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                var thmbs = dc.ThumbImages.ToList();

                foreach(ThumbImage thmbitem in thmbs)
                {
                    string fpath = "~" + thmbitem.image;
                        WebImage img = new WebImage(fpath);
                        double imgwidth = img.Width;
                        double imgheight = img.Height;
                        double valratio = imgwidth > imgheight ? imgwidth : imgheight;
                        imgwidth = (imgwidth / valratio) * 100;
                        imgheight = (imgheight / valratio) * 100;
                        img.Resize((int)Math.Round(imgwidth), (int)Math.Round(imgheight), true, true);
                        string fext = img.FileName.Substring(img.FileName.Length - 4);
                        path = Path.Combine(Server.MapPath("~" + Globals.ImageThumbSmallPath), thmbitem.Product.sku + fext);

                        img.Save(path);

                        Thumbsmall ts = dc.Thumbsmalls.FirstOrDefault(p => p.prodid == thmbitem.prodid);

                        if (ts == null)
                        {
                            ts = new Thumbsmall();
                            ts.prodid = thmbitem.prodid ?? 0;
                            ts.image = Path.Combine(Globals.ImageThumbSmallPath, thmbitem.Product.sku + fext);
                            dc.Thumbsmalls.Add(ts);
                        }
                        else
                        {

                            ts.image = Path.Combine(Globals.ImageThumbSmallPath, thmbitem.Product.sku + fext);
                        }

                        dc.SaveChanges();
                    
                }
            }
            return View();
        }

        public ActionResult Delete(int id = 0)
        {
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            if(product.productType < 3)
            {
                ViewBag.type = product.productType;
            }
            else
            {
                ViewBag.type = 3;
            }

            return View(product);
        }

        //
        // POST: /DewaltProduct/Delete/5

        [HttpPost, ActionName("Delete")]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeWaltPOSEntities db = new DeWaltPOSEntities();
            Product product = db.Products.Find(id);
            int type = product.productType ?? 0;
            var relprods = db.RelatedProducts.Where(p => p.ProductId == id).ToList();
            foreach(RelatedProduct relitem in relprods)
            {
                db.RelatedProducts.Remove(relitem);
            }
            var thmbs = db.ThumbImages.Where(t => t.prodid == id).ToList();
            foreach (ThumbImage relthmb in thmbs)
            {
                db.ThumbImages.Remove(relthmb);
            }


            var thmbsmall = db.Thumbsmalls.Where(t => t.prodid == id).ToList();
            foreach (Thumbsmall relthmb in thmbsmall)
            {
                db.Thumbsmalls.Remove(relthmb);
            }
            var ctlgs = db.CatalogueImages.Where(c => c.prodid == id).ToList();
            foreach (CatalogueImage ctlg in ctlgs)
            {
                db.CatalogueImages.Remove(ctlg);
            }

            var relpanels = db.RelatedPanels.Where(p => p.ProductId == id);
            foreach (RelatedPanel relitem in relpanels)
            {
                db.RelatedPanels.Remove(relitem);
            }

            db.Products.Remove(product);
            db.SaveChanges();

            if (type == 1)
            {
                return RedirectToAction("SuperSkus");
            }

            else if (type == 2)
            {
                return RedirectToAction("GlobalFixtures");
            }
            else
            {
                return RedirectToAction("Index");
            }
        
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}