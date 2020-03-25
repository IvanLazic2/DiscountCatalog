using DiscountCatalog.WebAPI.REST.Account;
using DiscountCatalog.WebAPI.REST.Manager;
using DiscountCatalog.WebAPI.REST.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.REST.StoreAdmin
{
    public class StoreAdminREST
    {
        public AccountREST Identity { get; set; }
        public IEnumerable<ManagerREST> Managers { get; set; }
        public IEnumerable<StoreREST> Stores { get; set; }
    }
}