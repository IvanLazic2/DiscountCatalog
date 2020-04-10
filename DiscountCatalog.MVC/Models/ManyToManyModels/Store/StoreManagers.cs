using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Store;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Models.ManyToManyModels.Store
{
    public class StoreManagers
    {
        public StoreREST Store { get; set; }
        public PagingEntity<StoreManager> Managers { get; set; }
    }
}