using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories
{
    public interface IStoreRepository : IRepository<StoreEntity>
    {
        Task<Result> CreateAsync(StoreEntity store, string storeAdminId);

        IEnumerable<StoreEntity> GetAllApproved(string sortOrder, string searchString);
        IEnumerable<StoreEntity> GetAllDeleted(string sortOrder, string searchString);
        IEnumerable<StoreEntity> GetAllLoaded(string sortOrder, string searchString);
        StoreEntity GetLoaded(string id);
        StoreEntity GetApproved(string id);

        Task<Result> UpdateAsync(StoreEntity store);

        Result MarkAsDeleted(string id);
        Result MarkAsRestored(string id);
    }
}
