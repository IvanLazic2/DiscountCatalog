using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.MVC.Models
{
    public class ResponseMessage
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Method { get; set; }
        public string Result { get; set; }
    }
}