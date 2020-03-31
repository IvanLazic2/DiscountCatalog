using DiscountCatalog.Common.Models;
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
        IList<StoreREST> FilterManagers(IList<StoreREST> stores);

        StoreREST FilterManagers(StoreREST store);

        IList<StoreREST> FilterStoreAdmin(IList<StoreREST> stores);

        StoreREST FilterStoreAdmin(StoreREST store);

        Task<Result> CreateAsync(StoreRESTPost store);

        StoreREST Get(string storeAdminIdentityId, string storeId);

        IPagingList<StoreREST> GetAll(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize);

        IPagingList<StoreREST> GetAllDeleted(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize);

        Task<Result> UpdateAsync(string storeAdminIdentityId, StoreRESTPut store);

        Result Delete(string storeAdminIdentityId, string storeId);

        Result Restore(string storeAdminIdentityId, string storeId);

        Result PostImage(string storeAdminIdentityId, string storeId, byte[] image);

        byte[] GetImage(string storeId);

        //SelectedStore Select(string storeAdminId, string storeId);
    }
}
