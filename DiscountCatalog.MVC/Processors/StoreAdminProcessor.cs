using DiscountCatalog.MVC.Extensions;
using DiscountCatalog.MVC.REST.Store;
using DiscountCatalog.MVC.REST.StoreAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Processors
{
    public class StoreAdminProcessor
    {
        public static IEnumerable<StoreREST> SearchStores(StoreAdminREST storeAdmin, string searchString)
        {
            IEnumerable<StoreREST> stores = storeAdmin.Stores;

            if (!string.IsNullOrEmpty(searchString))
            {
                stores = stores.Where(u => u.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return stores;
        }

        public static IEnumerable<StoreREST> OrderStores(StoreAdminREST storeAdmin, string sortOrder)
        {
            IEnumerable<StoreREST> stores = storeAdmin.Stores;

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