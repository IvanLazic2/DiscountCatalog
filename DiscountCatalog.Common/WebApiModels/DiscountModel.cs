using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.WebApiModels
{
    public class DiscountModel
    {
        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? Discount { get; set; }
    }
}
