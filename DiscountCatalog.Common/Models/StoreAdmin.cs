using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.Models
{
    public class StoreAdmin
    {
        public string Id { get; set; }
        public User Identity { get; set; }
        public List<Store> Stores { get; set; }
        public List<Manager> Managers { get; set; }
    }
}
