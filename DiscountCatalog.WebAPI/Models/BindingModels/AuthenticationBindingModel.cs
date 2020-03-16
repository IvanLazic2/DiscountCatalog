using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models.BindingModels
{
    public class AuthenticationBindingModel
    {
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
    }
}