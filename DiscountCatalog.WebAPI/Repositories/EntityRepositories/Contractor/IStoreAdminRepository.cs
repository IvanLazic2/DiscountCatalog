using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.REST.StoreAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor
{
    public interface IStoreAdminRepository : IRepository<StoreAdminEntity>
    {
        Task<Result> CreateAsync(ApplicationUser user, string password, StoreAdminEntity storeAdmin);

        StoreAdminEntity GetWithIdentity(string id);
        StoreAdminEntity GetByIdentityId(string identityId);
        IEnumerable<StoreAdminEntity> GetAllWithIdentity();

        StoreAdminEntity GetLoadedByIdentityId(string identityId);
        IEnumerable<StoreAdminEntity> GetAllLoaded(string sortOrder, string searchString);
        StoreAdminEntity GetLoaded(string id);
        StoreAdminEntity GetApproved(string id);

        IEnumerable<StoreAdminEntity> GetAllApproved(string sortOrder, string searchString);
        IEnumerable<StoreAdminEntity> GetAllDeleted(string sortOrder, string searchString);

        Task<Result> UpdateAsync(StoreAdminRESTPut storeAdmin);

        Result MarkAsDeleted(string id);
        Result MarkAsRestored(string id);

    }
}
