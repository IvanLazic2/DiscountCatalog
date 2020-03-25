using DiscountCatalog.WebAPI.REST.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.REST.StoreAdmin
{
    public class StoreAdminRESTPost
    {
        public AccountRESTPost Identity { get; set; }
    }
}