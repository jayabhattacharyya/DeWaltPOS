//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeWaltPOS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.CatalogueImages = new HashSet<CatalogueImage>();
            this.RelatedFixtures = new HashSet<RelatedFixture>();
            this.RelatedFixtures1 = new HashSet<RelatedFixture>();
            this.RelatedPanels = new HashSet<RelatedPanel>();
            this.RelatedProducts = new HashSet<RelatedProduct>();
            this.RelatedProducts1 = new HashSet<RelatedProduct>();
            this.ThumbImages = new HashSet<ThumbImage>();
            this.Thumbsmalls = new HashSet<Thumbsmall>();
        }
    
        public int id { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<System.DateTime> modified { get; set; }
        public Nullable<int> productType { get; set; }
        public Nullable<bool> IsBestPractice { get; set; }
        public string sku { get; set; }
        public Nullable<int> Brand { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public Nullable<decimal> length { get; set; }
        public Nullable<decimal> height { get; set; }
        public Nullable<decimal> width { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> cartonWeight { get; set; }
        public Nullable<int> cartonLength { get; set; }
        public Nullable<int> cartonWidth { get; set; }
        public Nullable<int> cartonHeight { get; set; }
        public Nullable<int> cartonUnits { get; set; }
        public Nullable<int> cartonReqd { get; set; }
        public Nullable<int> cbmReqd { get; set; }
        public Nullable<int> itemsPerLoad { get; set; }
        public Nullable<int> MOQ { get; set; }
        public Nullable<bool> imageSupplied { get; set; }
        public Nullable<int> productPlaceholder { get; set; }
        public Nullable<bool> deleted { get; set; }
        public Nullable<int> Category { get; set; }
        public Nullable<int> PanelType { get; set; }
        public Nullable<int> BestPracticeId { get; set; }
    
        public virtual BestPracticeType BestPracticeType { get; set; }
        public virtual Brand Brand1 { get; set; }
        public virtual ICollection<CatalogueImage> CatalogueImages { get; set; }
        public virtual PanelType PanelType1 { get; set; }
        public virtual Product Product1 { get; set; }
        public virtual Product Product2 { get; set; }
        public virtual ProductType ProductType1 { get; set; }
        public virtual ICollection<RelatedFixture> RelatedFixtures { get; set; }
        public virtual ICollection<RelatedFixture> RelatedFixtures1 { get; set; }
        public virtual ICollection<RelatedPanel> RelatedPanels { get; set; }
        public virtual ICollection<RelatedProduct> RelatedProducts { get; set; }
        public virtual ICollection<RelatedProduct> RelatedProducts1 { get; set; }
        public virtual ICollection<ThumbImage> ThumbImages { get; set; }
        public virtual ICollection<Thumbsmall> Thumbsmalls { get; set; }
    }
}
