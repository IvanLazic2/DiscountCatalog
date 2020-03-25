using DiscountCatalog.WebAPI.REST.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.REST.StoreAdmin
{
    public class StoreAdminRESTPut
    {
        public string Id { get; set; }
        public AccountRESTPut Identity { get; set; }
    }
}