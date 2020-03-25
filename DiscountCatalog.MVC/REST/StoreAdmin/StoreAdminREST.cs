
using DiscountCatalog.MVC.REST.Account;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.REST.StoreAdmin
{
    public class StoreAdminREST
    {
        public AccountREST Identity { get; set; }
        public IEnumerable<ManagerREST> Managers { get; set; }
        public IEnumerable<StoreREST> Stores { get; set; }
    }
}