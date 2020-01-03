using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiPostImage
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }
    }
}
