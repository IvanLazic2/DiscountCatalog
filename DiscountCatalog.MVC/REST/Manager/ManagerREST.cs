using DiscountCatalog.MVC.REST.Account;
using DiscountCatalog.MVC.REST.Store;
using DiscountCatalog.MVC.REST.StoreAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.REST.Manager
{
    public class ManagerREST
    {
        public string Id { get; set; }
        public AccountREST Identity { get; set; }
        public StoreAdminREST Administrator { get; set; }
        public IEnumerable<StoreREST> Stores { get; set; }
    }
}