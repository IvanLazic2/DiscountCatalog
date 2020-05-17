using DiscountCatalog.MVC.Extensions;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Product;
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

        public static IEnumerable<ManagerREST> OrderManagers(StoreREST store, string sortOrder)
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

        //PRODUCTS

        public static IList<ProductREST> OrderProducts(IList<ProductREST> products, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName).ToList();
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.NewPrice).ToList();
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.NewPrice).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName).ToList();
                    break;
            }

            return products.ToList();
        }

        public static IList<ProductREST> SearchProducts(IList<ProductREST> products, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString, StringComparer.OrdinalIgnoreCase) || p.Store.Administrator.Identity.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return products.ToList();
        }

        public static IEnumerable<ProductREST> FilterPrice(IEnumerable<ProductREST> products, string priceFilter)
        {
            if (!string.IsNullOrEmpty(priceFilter))
            {
                string[] arr = priceFilter.Split(",".ToCharArray());

                int from = Convert.ToInt32(arr[0]);
                int to = Convert.ToInt32(arr[1]);

                products = products.Where(p => p.NewPrice >= from && p.NewPrice <= to);
            }

            return products;
        }

        public static IEnumerable<ProductREST> FilterDate(IEnumerable<ProductREST> products, string dateFilter, bool includeUpcoming)
        {
            if (!string.IsNullOrEmpty(dateFilter))
            {
                DateTime date = DateTime.Parse(dateFilter);

                if (date != null)
                {
                    if (includeUpcoming)
                        products = products.Where(p => DateTime.Parse(p.DiscountDateEnd).CompareTo(date) >= 0);
                    else
                        products = products.Where(p => DateTime.Parse(p.DiscountDateEnd).CompareTo(date) >= 0 && DateTime.Parse(p.DiscountDateBegin).CompareTo(date) <= 0);
                }
            }

            return products;
        }
    }
}