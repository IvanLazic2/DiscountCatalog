using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.REST.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor
{
    public interface IStoreRepository : IRepository<StoreEntity>
    {
        Task<Result> CreateAsync(StoreEntity store, string storeAdminId);

        IEnumerable<StoreEntity> GetAllApproved();
        IEnumerable<StoreEntity> GetAllDeleted();
        IEnumerable<StoreEntity> GetAllLoaded();
        IEnumerable<StoreEntity> GetAllApproved(string storeAdminIdentityId);
        IEnumerable<StoreEntity> GetAllDeleted(string storeAdminIdentityId);
        IEnumerable<StoreEntity> GetAllLoaded(string storeAdminIdentityId);

        StoreEntity GetLoaded(string storeId);
        StoreEntity GetApproved(string storeId);
        StoreEntity GetLoaded(string storeAdminIdentityId, string storeId);
        StoreEntity GetApproved(string storeAdminIdentityId, string storeId);

        Result PostStoreImage(string id, byte[] image);
        //byte[] GetStoreImage(string id);

        Task<Result> UpdateAsync(StoreRESTPut store);
        Task<Result> UpdateAsync(string storeAdminIdentityId, StoreRESTPut store);

        Result MarkAsDeleted(string storeId);
        Result MarkAsRestored(string storeId);
        Result MarkAsDeleted(string storeAdminIdentityId, string storeId);
        Result MarkAsRestored(string storeAdminIdentityId, string storeId);
    }
}
