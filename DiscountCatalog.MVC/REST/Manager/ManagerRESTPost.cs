using DiscountCatalog.MVC.REST.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.REST.Manager
{
    public class ManagerRESTPost
    {
        public AccountRESTPost Identity { get; set; }
        public string StoreAdminId { get; set; }
    }
}