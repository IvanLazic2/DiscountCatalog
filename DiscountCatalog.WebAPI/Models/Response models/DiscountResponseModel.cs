using DiscountCatalog.Common.WebApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models
{
    public class DiscountResponseModel
    {
        public DiscountModel Discount { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

        public DiscountResponseModel()
        {
            Discount = new DiscountModel();
        }
    }
}