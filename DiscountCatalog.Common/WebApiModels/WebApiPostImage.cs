using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.WebApiModels
{
    public class WebApiPostImage
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }
    }
}
