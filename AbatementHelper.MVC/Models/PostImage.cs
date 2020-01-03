using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.MVC.Models
{
    public class PostImage
    {
        public string Id { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}