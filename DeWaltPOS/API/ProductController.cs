using DeWaltPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace DeWaltPOS
{
    [Serializable]
    public class DResponse
    {
        public int rcnt;
        public long tmstref;
        public IEnumerable<object> rcrds; // record data
    }
    public class ProductController : ApiController
    {

        // GET api/<controller>/5
        public object Get()
        {
            
            string serialised = ""; object resp = null;
            IList<Product> products = new List<Product>();
            using (DeWaltPOSEntities dc = new DeWaltPOSEntities())
            {
                products = dc.Products.ToList();
                serialised = new JavaScriptSerializer().Serialize(resp = new DResponse()
                {
                    rcnt = dc.API_KEY_TABLE.Count(),
                    tmstref = 123,
                    rcrds = products.Select(e => new
                    {
                        id = e.id,
                        sku = e.sku,
                        name = e.Description,
                        description = e.Text,
                        IsBestPractice = e.IsBestPractice,
                        producttype = e.productType,
                        paneltype = e.PanelType,
                        panelarray = e.RelatedPanels.Select(r => r.PanelId),
                        bestpracticetype = e.BestPracticeId,
                        price = e.price,
                        thumb_imageuri = e.productType == 14 || e.productType == 1 ? "" : (e.ThumbImages.Where(t => t.prodid == e.id).Count() > 0 ? "http://dewalt.ommdevelopment.co.uk" + e.ThumbImages.SingleOrDefault(t => t.prodid == e.id).image : ""),
                        catlg_imageuri = (e.CatalogueImages.Where(t => t.prodid == e.id).Count() > 0 ? e.CatalogueImages.Where(c => c.prodid == e.id).Select(ct => "http://dewalt.ommdevelopment.co.uk" + ct.image) : null),
                        width = e.length,
                        height = e.height,
                        depth = e.width,
                        relations = e.RelatedProducts.Select(r => r.RelatedProductId),
                        Quantity = e.RelatedProducts.Select(r => r.Quantity),
                        brand = e.Brand

                    })
                });
            }

            return resp;
        }


        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}