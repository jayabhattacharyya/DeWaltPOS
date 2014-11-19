using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Threading;
using System.Web.Mvc;
using System.Threading.Tasks;
using DeWaltPOS.Models;

namespace DeWaltPOS.Code
{
    public class StandingData
    {

        public static List<SelectListItem> AllProducts()
        {
            List<SelectListItem> AllProdSelList = new List<SelectListItem>();
            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                IEnumerable<Product> allprod = dc.Products;
                foreach (Product pd in allprod)
                {
                    AllProdSelList.Add(new SelectListItem()
                    {
                        Value = pd.id.ToString(),
                        Text = pd.id.ToString()
                    });
                }
            }
            return AllProdSelList;

        }

        public static List<SelectListItem> AllProductDetails()
        {
             List<SelectListItem> AllProdSelList = new List<SelectListItem>();
             using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
             {
                 IEnumerable<Product> allprod = dc.Products;
                 AllProdSelList.Add(new SelectListItem()
                 {
                     Text = "NEW PRODUCT",
                     Value = "0"
                 });
                 foreach (Product pd in allprod)
                 {
                     AllProdSelList.Add(new SelectListItem()
                     {
                         Text = pd.sku + " /  " + pd.Description,
                         Value = pd.id.ToString()
                     });
                 }
             }
            return AllProdSelList;

        }



        public static List<SelectListItem> AllProductTypes()
        {
            List<SelectListItem> lstProdType = new List<SelectListItem>();

            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                IEnumerable<ProductType> allprodtypes = dc.ProductTypes;
                foreach (ProductType pd in allprodtypes)
                {
                    if (pd.id > 1)
                    {
                        lstProdType.Add(new SelectListItem()
                        {
                            Text = pd.name,
                            Value = pd.id.ToString()
                        });
                    }
                }
            }
            return lstProdType;

        }

        public static List<SelectListItem> DispProductTypes()
        {
            List<SelectListItem> lstProdType = new List<SelectListItem>();

            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                IEnumerable<ProductType> allprodtypes = dc.ProductTypes;
                lstProdType.Add(new SelectListItem()
                {
                    Text = "Show All Products ",
                    Value = "0"
                });
                foreach (ProductType pd in allprodtypes)
                {
                    if (pd.id > 2)
                    {
                        lstProdType.Add(new SelectListItem()
                        {
                            Text = pd.name,
                            Value = pd.id.ToString()
                        });
                    }
                }
            }
            return lstProdType;

        }

        public static List<SelectListItem> DispPanelTypes()
        {
            List<SelectListItem> lstProdType = new List<SelectListItem>();
            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                IEnumerable<PanelType> allpaneltypes = dc.PanelTypes.OrderBy(p => p.PanelId);
                lstProdType.Add(new SelectListItem()
                {
                    Text = "Show All Panels ",
                    Value = "0"
                });

                foreach (PanelType pn in allpaneltypes)
                {
                    if (pn.PanelId > 0)
                    {
                        lstProdType.Add(new SelectListItem()
                        {
                            Text = pn.PanelCode + "  /  " + pn.Description,
                            Value = pn.PanelId.ToString()
                        });
                    }
                }
            }
            return lstProdType;

        }

        public static List<SelectListItem> DispBestPracticeTypes()
        {
            List<SelectListItem> lstBPType = new List<SelectListItem>();

            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                IEnumerable<BestPracticeType> allBPtypes = dc.BestPracticeTypes.OrderBy(b => b.BestPracticeId);
                lstBPType.Add(new SelectListItem()
                {
                    Text = "Show All",
                    Value = "0"
                });
                foreach (BestPracticeType bp in allBPtypes)
                {
                    if (bp.BestPracticeId > 0)
                    {
                        lstBPType.Add(new SelectListItem()
                        {
                            Text = bp.Description,
                            Value = bp.BestPracticeId.ToString()
                        });
                    }
                }
            }
            return lstBPType;

        }


        public static List<SelectListItem> BestPracticeTypes()
        {
            List<SelectListItem> lstBPType = new List<SelectListItem>();
            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                IEnumerable<BestPracticeType> allBPtypes = dc.BestPracticeTypes.OrderBy(b => b.BestPracticeId);

                foreach (BestPracticeType bp in allBPtypes)
                {
                    if (bp.BestPracticeId > 0)
                    {
                        lstBPType.Add(new SelectListItem()
                        {
                            Text = bp.Description,
                            Value = bp.BestPracticeId.ToString()
                        });
                    }
                }
            }
            return lstBPType;

        }

        public static List<SelectListItem> AllPanelTypes()
        {
            List<SelectListItem> lstProdType = new List<SelectListItem>();
            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                IEnumerable<PanelType> allpaneltypes = dc.PanelTypes.OrderBy(p => p.PanelId);
                foreach (PanelType pn in allpaneltypes)
                {
                    if (pn.PanelId > 0)
                    {
                        lstProdType.Add(new SelectListItem()
                        {
                            Text = pn.PanelCode + "  /  " + pn.Description,
                            Value = pn.PanelId.ToString()
                        });
                    }
                }
            }
            return lstProdType;

        }
    }
}