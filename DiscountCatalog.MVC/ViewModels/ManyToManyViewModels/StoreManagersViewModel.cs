using DiscountCatalog.MVC.Models.ManyToManyModels.Store;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Store;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.ViewModels.ManyToManyViewModels
{
    public class StoreManagersViewModel
    {
        public StoreREST Store { get; set; }
        public IPagedList<StoreManager> Managers { get; set; }
    }
}