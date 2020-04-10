using DiscountCatalog.MVC.Models.ManyToManyModels;
using DiscountCatalog.MVC.Models.ManyToManyModels.Manager;
using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.REST.Manager;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.ViewModels.ManyToManyViewModels
{
    public class ManagerStoresViewModel
    {
        public ManagerREST Manager { get; set; }
        public IPagedList<ManagerStore> Stores { get; set; }
    }
}