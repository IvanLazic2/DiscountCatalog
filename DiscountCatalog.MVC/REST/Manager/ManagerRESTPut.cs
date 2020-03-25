using DiscountCatalog.MVC.REST.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.REST.Manager
{
    public class ManagerRESTPut
    {
        public string Id { get; set; }
        public AccountREST Identity { get; set; }
    }
}