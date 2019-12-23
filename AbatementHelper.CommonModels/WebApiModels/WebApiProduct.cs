using AbatementHelper.CommonModels.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiProduct
    {
        public string Id { get; set; }
        public WebApiStore Store { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        [GreaterThan("ProductNewPrice"), Required, Range(0, double.MaxValue, ErrorMessage = "Please enter valid price.")]
        public string ProductOldPrice { get; set; }
        [Required, Range(0, double.MaxValue, ErrorMessage = "Please enter valid price.")] //oof
        public string ProductNewPrice { get; set; }
        public string DiscountPercentage
        {
            get
            {
                string toReturn = "";

                if (ProductOldPrice != null && ProductNewPrice != null)
                {
                    double productOldPrice = double.Parse(ProductOldPrice, System.Globalization.CultureInfo.InvariantCulture);
                    double productNewPrice = double.Parse(ProductNewPrice, System.Globalization.CultureInfo.InvariantCulture);

                    double percentage = Math.Round(100 - (productNewPrice / productOldPrice) * 100, 1);

                    toReturn = percentage.ToString() + "%";
                }



                return toReturn;
            }
            private set
            {
            }
        }
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
    }
}
