using DiscountCatalog.WebAPI.Models.ManyToManyModels.Manager;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.REST.Manager;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models.ManyToManyModels.Manager
{
    public class ManagerStores
    {
        public ManagerREST Manager { get; set; }
        public IPagingList<ManagerStore> Stores { get; set; }
    }
}