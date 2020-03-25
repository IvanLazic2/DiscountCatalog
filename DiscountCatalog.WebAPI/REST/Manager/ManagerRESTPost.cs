using DiscountCatalog.WebAPI.REST.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.REST.Manager
{
    public class ManagerRESTPost
    {
        public AccountRESTPost Identity { get; set; }
        public string StoreAdminId { get; set; }
        public string Password { get; set; }
    }
}