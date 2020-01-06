using AbatementHelper.CommonModels.WebApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.MVC.ViewModels
{
    public class ProductViewModel
    {
        public WebApiProduct Product { get; set; }
        public DiscountModel Discount { get; set; }
    }
}