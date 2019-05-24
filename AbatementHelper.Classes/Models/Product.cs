using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.Classes.Models
{
    public class Product
    {
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public string ProductOldPrice { get; set; }
        public string ProductNewPrice { get; set; }
        public string ProductAbatementDateBegin { get; set; }
        public string ProductAbatementDateEnd { get; set; }
        public string ProductNote { get; set; }
    }
}
