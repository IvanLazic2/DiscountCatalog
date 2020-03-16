using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.Common.Models
{
    public class AuthenticationModel
    {
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
    }
}