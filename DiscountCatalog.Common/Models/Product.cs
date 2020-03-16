using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.Models
{
    public class Product
    {
        public string Id { get; set; }
        public Store Store { get; set; }
        public string ProductName { get; set; }
    }
}
