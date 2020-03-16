using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.Models
{
    public class ManagerStore
    {
        public Manager Manager { get; set; }
        public Store Store { get; set; }
        public bool Assigned { get; set; }
    }
}
