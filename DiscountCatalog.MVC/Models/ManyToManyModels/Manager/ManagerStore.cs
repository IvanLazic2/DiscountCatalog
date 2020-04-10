using DiscountCatalog.MVC.REST.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Models.ManyToManyModels.Manager
{
    public class ManagerStore
    {
        public StoreREST Store { get; set; }
        public bool Assigned { get; set; }
    }
}