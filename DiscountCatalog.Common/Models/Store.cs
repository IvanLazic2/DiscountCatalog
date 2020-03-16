using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.Models
{
    public class Store
    {
        public string Id { get; set; }
        public string StoreName { get; set; }
        public StoreAdmin Administrator { get; set; }
        public List<Product> Products { get; set; }
    }
}
