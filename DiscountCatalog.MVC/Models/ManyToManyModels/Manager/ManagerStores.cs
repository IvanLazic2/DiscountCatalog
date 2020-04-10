using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Store;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Models.ManyToManyModels.Manager
{
    public class ManagerStores
    {
        public ManagerREST Manager { get; set; }
        public PagingEntity<ManagerStore> Stores { get; set; }
    }
}