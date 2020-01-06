using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class DiscountModel
    {
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public double Discount { get; set; }
    }
}
