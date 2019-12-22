using AbatementHelper.CommonModels.WebApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.MVC.Models
{
    public class dbproduct
    {
        public string Id { get; set; }
        public WebApiStore Store { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public string ProductOldPrice { get; set; }
        public string ProductNewPrice { get; set; }
        public string DiscountPercentage { get; set; }
        public DateTime DiscountDateBegin { get; set; }
        public DateTime DiscountDateEnd { get; set; }
        public string Quantity { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool Expired { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
