using AbatementHelper.WebAPI.Mapping;
using AutoMapper;
using DiscountCatalog.WebAPI.Extensions;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.Paging.Implementation;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.REST.Product;
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.REST.StoreAdmin;
using DiscountCatalog.WebAPI.Service.Contractor;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Service.Implementation
{
    public class UserService : IService<ProductREST>, IUserService
    {
        #region Properties

        readonly IMapper mapper;

        #endregion

        #region Constructor

        public UserService()
        {
            mapper = AutoMapping.Initialise();
        }

        #endregion

        #region Methods

        //SEARCH/ORDER

        #region Search/Order

        public IList<ProductREST> Order(IList<ProductREST> products, string sortOrder)
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

        public IList<ProductREST> Search(IList<ProductREST> products, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString, StringComparer.OrdinalIgnoreCase) || p.Store.Administrator.Identity.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return products.ToList();
        }

        #endregion

        //FILTER

        #region Filter

        public IEnumerable<ProductEntity> FilterPrice(IEnumerable<ProductEntity> products, string priceFilter)
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

        public IEnumerable<ProductEntity> FilterDate(IEnumerable<ProductEntity> products, string dateFilter, bool includeUpcoming)
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

        public IEnumerable<ProductEntity> FilterStoreAdmin(IEnumerable<ProductEntity> products)
        {
            foreach (var product in products)
            {
                if (product.Store.Administrator.Stores != null)
                    product.Store.Administrator.Stores.Clear();
                if (product.Store.Administrator.Managers != null)
                    product.Store.Administrator.Managers.Clear();
            }

            return products;
        }

        public ProductEntity FilterStoreAdmin(ProductEntity product)
        {
            if (product.Store.Administrator.Stores != null)
                product.Store.Administrator.Stores.Clear();
            if (product.Store.Administrator.Managers != null)
                product.Store.Administrator.Managers.Clear();

            return product;
        }

        public IList<StoreEntity> FilterManagers(IList<StoreEntity> stores, bool clear)
        {
            if (clear)
            {
                foreach (var store in stores)
                {
                    if (store.Managers != null)
                    {
                        store.Managers.Clear();
                    }
                }
            }
            else
            {
                foreach (var store in stores)
                {
                    List<ManagerEntity> managers = store.Managers.ToList();
                    managers.RemoveAll(m => m.Identity.Deleted || !m.Identity.Approved);
                    store.Managers = managers;
                }
            }

            return stores;
        }

        public StoreEntity FilterManagers(StoreEntity store, bool clear)
        {
            if (clear)
            {
                if (store.Managers != null)
                {
                    store.Managers.Clear();
                }
            }
            else
            {
                List<ManagerEntity> managers = store.Managers.ToList();
                managers.RemoveAll(m => m.Identity.Deleted || !m.Identity.Approved);
                store.Managers = managers;
            }

            return store;
        }

        public IList<StoreEntity> FilterStoreAdmin(IList<StoreEntity> stores)
        {
            foreach (var store in stores)
            {
                if (store.Administrator.Stores != null)
                {
                    store.Administrator.Stores.Clear();
                }
                if (store.Administrator.Managers != null)
                {
                    store.Administrator.Managers.Clear();
                }
            }

            return stores;
        }

        public StoreEntity FilterStoreAdmin(StoreEntity store)
        {
            if (store.Administrator.Stores != null)
            {
                store.Administrator.Stores.Clear();
            }
            if (store.Administrator.Managers != null)
            {
                store.Administrator.Managers.Clear();
            }

            return store;
        }

        #endregion

        //GETALL

        #region GetAll

        public IPagingList<ProductREST> GetAllProducts(string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming)
        {
            try
            {
                using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
                {
                    IEnumerable<ProductEntity> products = uow.Products.GetAllApproved();

                    products = FilterStoreAdmin(products);
                    products = FilterDate(products, dateFilter, includeUpcoming);
                    products = FilterPrice(products, priceFilter);

                    IList<ProductREST> mapped = mapper.Map<IList<ProductREST>>(products);

                    mapped = Search(mapped, searchString);
                    mapped = Order(mapped, sortOrder);

                    //mapped.ToList().ForEach(p => p.ProductImage = ImageProcessor.CreateThumbnail(p.ProductImage));
                    mapped.ToList().ForEach(p => p.Store.StoreImage = ImageProcessor.CreateThumbnail(p.Store.StoreImage));
                    mapped.ToList().ForEach(p => p.Store.Administrator.Identity.UserImage = ImageProcessor.CreateThumbnail(p.Store.Administrator.Identity.UserImage));

                    IPagedList<ProductREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                    IPagingList<ProductREST> result = new PagingList<ProductREST>(subset, subset.GetMetaData());

                    return result;
                }
            }
            catch (Exception exc)
            {

                throw;
            }

        }

        #endregion

        //GET

        #region Get

        public ProductREST GetProduct(string productId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ProductEntity product = uow.Products.GetApproved(productId);

                product.Store.StoreImage = ImageProcessor.CreateThumbnail(product.Store.StoreImage);

                var mapped = mapper.Map<ProductREST>(product);

                //mapped.ProductImage = ImageProcessor.CreateThumbnail(product.ProductImage);

                return mapped;
            }
        }

        public StoreREST GetStore(string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                StoreEntity store = uow.Stores.GetApproved(storeId);

                store.StoreImage = ImageProcessor.CreateThumbnail(store.StoreImage);
                store.Administrator.Identity.UserImage = ImageProcessor.CreateThumbnail(store.Administrator.Identity.UserImage);
                store.Managers.Clear();

                store = FilterManagers(store, false);
                store = FilterStoreAdmin(store);

                var mapped = mapper.Map<StoreREST>(store);

                return mapped;
            }
        }

        public StoreAdminREST GetStoreAdmin(string storeAdminIdentityId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                StoreAdminEntity storeAdmin = uow.StoreAdmins.GetByIdentityId(storeAdminIdentityId);

                storeAdmin.Stores = FilterManagers(storeAdmin.Stores.ToList(), true);

                storeAdmin.Stores.ToList().ForEach(s => s.StoreImage = ImageProcessor.CreateThumbnail(s.StoreImage));
                storeAdmin.Identity.UserImage = ImageProcessor.CreateThumbnail(storeAdmin.Identity.UserImage);

                var mapped = mapper.Map<StoreAdminREST>(storeAdmin);

                return mapped;
            }
        }

        #endregion

        //PRICE

        #region Price

        public decimal GetMinPrice()
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var products = uow.Products.GetAllApproved();

                decimal? min = products.Select(p => p.NewPrice).Min();

                if (min == null)
                {
                    min = 0;
                }

                return min.Value;
            }
        }

        public decimal GetMaxPrice()
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var products = uow.Products.GetAllApproved();

                decimal? max = products.Select(p => p.NewPrice).Max();

                if (max == null)
                {
                    max = 0;
                }

                return max.Value;
            }
        }

        #endregion

        #endregion
    }
}