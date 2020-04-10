using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.REST.Store;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models.ManyToManyModels.Store
{
    public class StoreManagers
    {
        public StoreREST Store { get; set; }
        public IPagingList<StoreManager> Managers { get; set; }
    }
}