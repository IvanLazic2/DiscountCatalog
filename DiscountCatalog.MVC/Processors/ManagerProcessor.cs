using DiscountCatalog.MVC.Extensions;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Processors
{
    public static class ManagerProcessor
    {
        public static IEnumerable<StoreREST> SearchStores(ManagerREST manager, string searchString)
        {
            IEnumerable<StoreREST> stores = manager.Stores;

            if (!string.IsNullOrEmpty(searchString))
            {
                stores = stores.Where(u => u.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return stores;
        }

        public static IEnumerable<StoreREST> OrderStores(ManagerREST manager, string sortOrder)
        {
            IEnumerable<StoreREST> stores = manager.Stores;

            switch (sortOrder)
            {
                case "name_desc":
                    stores = stores.OrderByDescending(u => u.StoreName).ToList();
                    break;
                default:
                    stores = stores.OrderBy(u => u.StoreName).ToList();
                    break;
            }

            return stores;
        }
    }
}