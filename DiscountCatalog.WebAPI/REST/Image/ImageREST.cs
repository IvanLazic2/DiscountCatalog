using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.REST.Image
{
    public class ImageREST
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Base64StringContent { get; set; }
    }
}