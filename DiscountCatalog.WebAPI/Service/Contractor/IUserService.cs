using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.REST.Product;
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.REST.StoreAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Service.Contractor
{
    public interface IUserService
    {
        //IPagingList<ProductREST> GetAllProducts(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming);

        IPagingList<ProductREST> GetAllProducts(string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming);

        ProductREST GetProduct(string productId);

        StoreREST GetStore(string storeId);

        StoreAdminREST GetStoreAdmin(string storeAdminIdentityId);

        decimal GetMinPrice();

        decimal GetMaxPrice();

        //PRODUCT

        IEnumerable<ProductEntity> FilterPrice(IEnumerable<ProductEntity> products, string priceFilter);

        IEnumerable<ProductEntity> FilterDate(IEnumerable<ProductEntity> products, string dateFilter, bool includeUpcoming);

        IEnumerable<ProductEntity> FilterStoreAdmin(IEnumerable<ProductEntity> products);
        ProductEntity FilterStoreAdmin(ProductEntity product);

        //STORE

        IList<StoreEntity> FilterManagers(IList<StoreEntity> stores, bool clear);
        StoreEntity FilterManagers(StoreEntity store, bool clear);
        IList<StoreEntity> FilterStoreAdmin(IList<StoreEntity> stores);
        StoreEntity FilterStoreAdmin(StoreEntity store);
    }
}
