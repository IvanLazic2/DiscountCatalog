//using AbatementHelper.CommonModels.Attributes;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ExpressiveAnnotations.Attributes;
using AbatementHelper.CommonModels.Attributes;
using System.Globalization;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiProduct
    {
        public string Id { get; set; }
        public WebApiStore Store { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public string ProductOldPrice { get; set; }
        public string ProductNewPrice { get; set; }
        public string DiscountPercentage { get; set; }
        public string Currency { get; set; }
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.mm.gggg.}", ApplyFormatInEditMode = true)]
        public string DiscountDateBegin { get; set; }
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.mm.gggg.}", ApplyFormatInEditMode = true)]
        public string DiscountDateEnd { get; set; }
        public string Quantity { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool Expired { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateCreated { get; set; }

        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? Discount { get; set; }

        public WebApiProduct()
        {
            
        }
    }
}
