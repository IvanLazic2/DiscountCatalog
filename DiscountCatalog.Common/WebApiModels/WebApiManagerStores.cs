using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.WebApiModels
{
    public class WebApiManagerStore
    {
        public WebApiStore Store { get; set; }
        public WebApiManager Manager { get; set; }
        public bool Assigned { get; set; }
    }
}
