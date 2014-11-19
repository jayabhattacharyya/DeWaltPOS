using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Threading;
using System.Web.Mvc;
using System.Threading.Tasks;
using DeWaltPOS.Models;


namespace DeWalt.Code.Caching
{
    public static class DeWaltCaching
    {
        static DeWaltCaching()
        {

        }

        public static void InitApplicationCaching(bool asynchronous = true)
        {
            if (asynchronous)
                new Thread(() =>
                    {
                        _InitApplicationCaching();
                    }).Start();
            else
                _InitApplicationCaching();
        }

        private static void _InitApplicationCaching()
        {
            ProductsCache.Initialise();
            ProductTypesCache.Initialise();
        }

        public static void ClearProductSearch()
        {

        }

        public static void ClearProductSearchWorker()
        {

        }

        /// <summary>
        ///  returns the number of milliseconds between now and midnight
        /// </summary>
        /// <returns></returns>
        public static int MillisToMidnight()
        {
#if DEBUG
            return 10000;
#else
            return (int)DateTime.Now.Date.AddDays(1).Subtract(DateTime.Now).TotalMilliseconds;
#endif
        }
    }

    public class ProductTypesCache
    {
        private const string CacheName = "DeWalt.ProductsTypeCache";
        private static object CacheLock = new object();

        private static ProductTypesCache _Cache
        {
            get
            {
                if (HttpRuntime.Cache[CacheName] is ProductTypesCache)
                    return (ProductTypesCache)HttpRuntime.Cache[CacheName];
                return null;
            }
        }

        private static int _RefreshInterval;
        public static void Initialise(int refreshIntervalDays = 7)
        {
            _RefreshInterval = refreshIntervalDays;
            new ProductTypesCache();
        }

        public static List<ProductType> ProductTypes
        {
            get
            {
                if (_Cache == null)
                    return null;
                return _Cache._ProductTypes;
            }
        }
        public static List<SelectListItem> ProductTypesSelectList(int selectedProductType = 0)
        {
            if (_Cache == null)
                return null;
            if (selectedProductType > 0)
            {
                var selectList = new List<SelectListItem>();
                foreach (SelectListItem item in _Cache._ProductTypesSelectList)
                    selectList.Add(item);
                var selectItem = selectList.SingleOrDefault(i => i.Value == selectedProductType.ToString());
                if (selectItem != null)
                    selectItem.Selected = true;
                return selectList;
            }
            else
                return ProductTypesCache._Cache._ProductTypesSelectList;
        }


        public static List<SelectListItem> AllProducts()
        {
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            List<SelectListItem> AllProdSelList =  new List<SelectListItem>();
           IEnumerable<Product> allprod = dc.Products;
           foreach (Product pd in allprod)
           {
               AllProdSelList.Add(new SelectListItem()
               {                  
                       Value = pd.id.ToString(),
                       Text = pd.id.ToString()
               });
           }
           return AllProdSelList;

        }

        public static List<SelectListItem> AllProductDetails()
        {
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            List<SelectListItem> AllProdSelList = new List<SelectListItem>();
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
            return AllProdSelList;

        }



        public static List<SelectListItem> AllProductTypes()
        {
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            List<SelectListItem> lstProdType = new List<SelectListItem>();
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
            return lstProdType;

        }

        public static List<SelectListItem> DispProductTypes()
        {
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            List<SelectListItem> lstProdType = new List<SelectListItem>();
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
            return lstProdType;

        }

        public static List<SelectListItem> DispPanelTypes()
        {
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            List<SelectListItem> lstProdType = new List<SelectListItem>();
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
            return lstProdType;

        }

        public static List<SelectListItem> DispBestPracticeTypes()
        {
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            List<SelectListItem> lstBPType = new List<SelectListItem>();
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
            return lstBPType;

        }


        public static List<SelectListItem> BestPracticeTypes()
        {
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            List<SelectListItem> lstBPType = new List<SelectListItem>();
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
            return lstBPType;

        }

        public static List<SelectListItem> AllPanelTypes()
        {
            DeWaltPOSEntities dc = new DeWaltPOSEntities();
            List<SelectListItem> lstProdType = new List<SelectListItem>();
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
            return lstProdType;

        }

        /* the constructor builds the cache */
        private ProductTypesCache()
        {
            lock (CacheLock)
            {
                using (var entities = new DeWaltPOSEntities())
                {
                    _ProductTypes = entities.ProductTypes.ToList();
                    _ProductTypesSelectList = new List<SelectListItem>();
                    foreach (ProductType pt in _ProductTypes)
                    {
                        _ProductTypesSelectList.Add(new SelectListItem()
                        {
                            Value = pt.id.ToString(),
                            Text = pt.name
                        });
                    }
                    HttpRuntime.Cache.Remove(CacheName);
                    HttpRuntime.Cache.Add(
                        CacheName,
                        this,
                        null,
                        DateTime.Now.Date.GetFutureDayDate(DayOfWeek.Sunday), /* DateTime.Now.Date.AddDays(_RefreshInterval), */
                        Cache.NoSlidingExpiration,
                        CacheItemPriority.Normal,
                        new CacheItemRemovedCallback(CacheRemoved));
                    if (_ExpireTimer != null)
                    {
                        _ExpireTimer.Dispose();
                        _ExpireTimer = null;
                    }
                }
            }
        }

        protected static void CacheRemoved(string key, object value, CacheItemRemovedReason reason)
        { /* rebuild the cache */
            new ProductTypesCache();
        }

        private static Timer _ExpireTimer;
        public static void Expire()
        {
            if (_ExpireTimer == null)
            {
                _ExpireTimer = new Timer((object o) =>
                {
                    HttpRuntime.Cache.Remove(CacheName);
                },
                null,
                DeWaltCaching.MillisToMidnight(),
                Timeout.Infinite);
            }
        }

        private List<ProductType> _ProductTypes { get; set; } // base ProductTypes
        private List<SelectListItem> _ProductTypesSelectList { get; set; } // default - none selected
    }

    

    public class ProductsCache
    {        
        public const string CacheName = "DeWalt.ProductsCache";
        private static object CacheLock = new object();

        private static ProductsCache _Cache
        {
            get
            {
                if (HttpRuntime.Cache[CacheName] is ProductsCache)
                    return (ProductsCache)HttpRuntime.Cache[CacheName];
                return null;
            }
        }

        private static int _RefreshInterval;
        public static void Initialise(int refreshIntervalDays = 7)
        {
            _RefreshInterval = refreshIntervalDays;
            new ProductsCache();
        }

        private List<Product> _Products;
        public static List<Product> Products
        {
            get
            {
                if (_Cache == null)
                    return null;
                return _Cache._Products;
            }
        }

        /// <summary>
        /// Accsessor to the search cache object
        /// </summary>

        private ProductsCache()
        {
            lock (CacheLock)
            {

                HttpRuntime.Cache.Remove(CacheName);
                HttpRuntime.Cache.Add(
                    CacheName,
                    this,
                    null,
                    DateTime.Now.Date.GetFutureDayDate(DayOfWeek.Sunday), /* DateTime.Now.Date.AddDays(_RefreshInterval), */
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal,
                    new CacheItemRemovedCallback(CacheRemoved));
                ProductSearchCache.Expire();
                if (_ExpireTimer != null)
                {
                    _ExpireTimer.Dispose();
                    _ExpireTimer = null;
                }
            }
        }

        protected static void CacheRemoved(string key, object value, CacheItemRemovedReason reason)
        { /* rebuild the cache */
            new ProductsCache();
        }

        private static Timer _ExpireTimer;
        public static void Expire()
        {
            if (_ExpireTimer == null)
            {
                _ExpireTimer = new Timer((object o) =>
                {
                    HttpRuntime.Cache.Remove(CacheName);
                },
                null,
                DeWaltCaching.MillisToMidnight(),
                Timeout.Infinite);
            }
        }







        /// <summary>
        /// ProductsSearchCache dependent on ProductsCache. Non public, and regulated by ProductsCache.
        /// </summary>
        class ProductSearchCache
        {
            public const string CacheName = "DeWalt.ProductSearchCache";
            private static object CacheLock = new object();



            private static int _RefreshInterval;
            public static void Initialise(int refreshIntervalDays = 7)
            {
                _RefreshInterval = refreshIntervalDays;
                new ProductSearchCache();
            }

            /// <summary>
            /// Accsessor to the cached object
            /// </summary>


            protected static void CacheRemoved(string key, object value, CacheItemRemovedReason reason)
            { /* rebuild the cache */
                new ProductSearchCache();
            }

            public static void Expire()
            {
                if (_Cache == null)
                    new ProductSearchCache();
                else
                    HttpRuntime.Cache.Remove(CacheName);
            }
        }
    }
}