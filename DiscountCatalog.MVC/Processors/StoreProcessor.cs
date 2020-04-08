using DiscountCatalog.MVC.Extensions;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Processors
{
    public static class StoreProcessor
    {
        public static IEnumerable<ManagerREST> SearchManagers(StoreREST store, string searchString)
        {
            IEnumerable<ManagerREST> managers = store.Managers;

            if (!string.IsNullOrEmpty(searchString))
            {
                managers = managers.Where(u => u.Identity.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return managers;
        }

        public static IEnumerable<ManagerREST> OrderStores(StoreREST store, string sortOrder)
        {
            IEnumerable<ManagerREST> managers = store.Managers;

            switch (sortOrder)
            {
                case "name_desc":
                    managers = managers.OrderByDescending(u => u.Identity.UserName).ToList();
                    break;
                default:
                    managers = managers.OrderBy(u => u.Identity.UserName).ToList();
                    break;
            }

            return managers;
        }
    }
}