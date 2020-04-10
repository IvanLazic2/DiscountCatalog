using DiscountCatalog.WebAPI.REST.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models.ManyToManyModels.Store
{
    public class StoreManager
    {
        public ManagerREST Manager { get; set; }
        public bool Assigned { get; set; }
    }
}