using DiscountCatalog.Common.WebApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.ViewModels
{
    public class ProductViewModel
    {
        public WebApiProduct Product { get; set; }
        public DiscountModel Discount { get; set; }
    }
}