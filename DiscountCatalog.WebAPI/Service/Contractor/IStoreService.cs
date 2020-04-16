using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.REST.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Service.Contractor
{
    public interface IStoreService
    {
        Task<Result> CreateAsync(StoreRESTPost store);

        StoreREST Get(string storeAdminIdentityId, string storeId);
        StoreREST Get(string managerIdentityId, string storeAdminIdentityId, string storeId);

        IPagingList<StoreREST> GetAll(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize);
        IPagingList<StoreREST> GetAll(string managerIdentityId, string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize);

        IPagingList<StoreREST> GetAllDeleted(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize);
        //IPagingList<StoreREST> GetAllDeleted(string managerIdentityId, string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize);

        Task<Result> UpdateAsync(string storeAdminIdentityId, StoreRESTPut store);
        Task<Result> UpdateAsync(string managerIdentityId, string storeAdminIdentityId, StoreRESTPut store);

        Result Delete(string storeAdminIdentityId, string storeId);
        Result Restore(string storeAdminIdentityId, string storeId);
        Result PostImage(string storeAdminIdentityId, string storeId, byte[] image);
        Result PostImage(string managerIdentityId, string storeAdminIdentityId, string storeId, byte[] image);
        byte[] GetImage(string storeId);

        IList<StoreEntity> FilterManagers(IList<StoreEntity> stores, bool clear);
        StoreEntity FilterManagers(StoreEntity store, bool clear);
        IList<StoreEntity> FilterStoreAdmin(IList<StoreEntity> stores);
        StoreEntity FilterStoreAdmin(StoreEntity store);
    }
}
