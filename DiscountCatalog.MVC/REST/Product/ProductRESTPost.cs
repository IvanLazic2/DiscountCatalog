using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.REST.Product
{
    public class ProductRESTPost
    {
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public string Currency { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string DiscountDateBegin { get; set; }
        public string DiscountDateEnd { get; set; }
        public string Quantity { get; set; }
        public string MeasuringUnit { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool Expired { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
        public byte[] ProductImage { get; set; }
        public string StoreId { get; set; }
    }
}