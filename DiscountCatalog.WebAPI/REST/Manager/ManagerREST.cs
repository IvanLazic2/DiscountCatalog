using DiscountCatalog.WebAPI.REST.Account;
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.REST.StoreAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.REST.Manager
{
    public class ManagerREST
    {
        public string Id { get; set; }
        public AccountREST Identity { get; set; }
        public StoreAdminREST Administrator { get; set; }
        public IEnumerable<StoreREST> Stores { get; set; }
    }
}