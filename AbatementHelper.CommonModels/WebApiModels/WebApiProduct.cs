//using AbatementHelper.CommonModels.Attributes;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;


namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiProduct
    {
        public string Id { get; set; }
        public WebApiStore Store { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        [Required, Range(0, double.MaxValue, ErrorMessage = "Please enter valid price.")]
        public double ProductOldPrice { get; set; }
        [LessThan("ProductOldPrice", ErrorMessage = "New price has to be a discount"), Required, Range(0, double.MaxValue, ErrorMessage = "Please enter valid price.")]
        public double ProductNewPrice { get; set; }

        //[GreaterThan("ProductNewPrice"), Required, Range(0, double.MaxValue, ErrorMessage = "Please enter valid price.")]
        public string ProductOldPriceString { get; set; }
        public string ProductNewPriceString { get; set; }
        
        public double DiscountPercentage
        {
            get
            {
                return Math.Round(100 - (ProductNewPrice / ProductOldPrice) * 100, 1);
            }
            private set
            {
            }
        }

        public string DiscountPercentageString
        {
            get
            {
                string toReturn = DiscountPercentage.ToString() + "%";

                return toReturn;
            }
            private set
            {
            }
        }
        public string Currency { get; set; }
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DiscountDateBegin { get; set; }
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DiscountDateEnd { get; set; }
        public string Quantity { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool Expired
        {
            get
            {
                if (DateTime.Compare(DiscountDateEnd, DateTime.Now) >= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {

            }
        }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateCreated { get; set; }

        public WebApiProduct()
        {
            ProductOldPriceString = ProductOldPrice.ToString() + Currency;
            ProductNewPriceString = ProductNewPrice.ToString() + Currency;
        }
    }

    //public class ProductValidator : AbstractValidator<WebApiProduct>
    //{
    //    public ProductValidator()
    //    {
    //        RuleFor(p => p.ProductNewPrice).LessThan(p => p.ProductOldPrice).WithMessage("{0} has to be a discount!");
    //    }
    //}
}
