using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.Models
{
    public class Product
    {
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public string StoreName { get; set; }
        public string ProductOldPrice { get; set; }
        public string ProductNewPrice { get; set; }
        public DateTime ProductAbatementDateBegin { get; set; }
        public DateTime ProductAbatementDateEnd { get; set; }
        public string ProductNote { get; set; }
        public bool Expired { get; set; }
        public bool Approved { get; set; }
    }
}
