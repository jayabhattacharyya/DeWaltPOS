using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeWaltPOS.Models
{
    public class VMRelatedProducts
    {
        public int ProductId { get; set; }
        public int RelatedProductId { get; set; }
        public string sku { get; set; }
        public string Description { get; set; }
        public string ThumbImage { get; set; }
        public string ProductType { get; set; }
        public int Quantity { get; set; }
    }
}