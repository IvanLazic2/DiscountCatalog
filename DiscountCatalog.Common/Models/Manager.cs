using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.Models
{
    public class Manager
    {
        public string Id { get; set; }
        public User Identity { get; set; }
        public StoreAdmin Administrator { get; set; }
        public List<Store> Stores { get; set; }
    }
}
