using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor
{
    public interface IManagerRepository : IRepository<ManagerEntity>
    {
        Task<Result> CreateAsync(string storeAdminId, ManagerEntity manager, string password);

        IEnumerable<ManagerEntity> GetAllApproved(string sortOrder, string searchString);
        IEnumerable<ManagerEntity> GetAllDeleted(string sortOrder, string searchString);
        IEnumerable<ManagerEntity> GetAllLoaded(string sortOrder, string searchString);

        //IEnumerable<ManagerEntity> GetAllByStoreAdminId(string storeAdminId, string sortOrder, string searchString);
        //IEnumerable<ManagerEntity> GetAllDeletedByStoreAdminId(string storeAdminId, string sortOrder, string searchString);

        ManagerEntity GetApproved(string id);
        ManagerEntity GetLoaded(string id);

        //ManagerEntity GetByIdentityId(string identityId);
        //ManagerEntity GetLoadedByIdentityId(string identityId);
        //ManagerEntity GetApprovedByIdentityId(string identityId);

        //ManagerEntity GetByStoreAdminId(string storeAdminId, string managerId);


        Task<Result> UpdateAsync(ManagerEntity manager);

        Result MarkAsDeleted(string id);
        Result MarkAsRestored(string id);
    }
}
