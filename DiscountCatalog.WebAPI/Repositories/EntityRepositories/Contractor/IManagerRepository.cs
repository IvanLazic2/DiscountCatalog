using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.REST.Manager;
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

        IQueryable<ManagerEntity> GetAllQuery(bool includeApproved, bool inludeDeleted);
        IEnumerable<ManagerEntity> GetAllDeleted();
        IEnumerable<ManagerEntity> GetAllLoaded();
        IEnumerable<ManagerEntity> GetAll(string storeAdminIdentityId);
        IEnumerable<ManagerEntity> GetAllDeleted(string storeAdminIdentityId);
        IEnumerable<ManagerEntity> GetAllLoaded(string storeAdminIdentityId);

        ManagerEntity GetApproved(string managerId);
        ManagerEntity GetLoaded(string managerId);
        ManagerEntity GetApproved(string storeAdminIdentityId, string managerId);
        ManagerEntity GetLoaded(string storeAdminIdentityId, string managerId);

        Task<Result> UpdateAsync(ManagerRESTPut manager);
        Task<Result> UpdateAsync(string storeAdminIdentityId, ManagerRESTPut manager);

        Result MarkAsDeleted(string managerId);
        Result MarkAsRestored(string managerId);
        Result MarkAsDeleted(string storeAdminIdentityId, string managerId);
        Result MarkAsRestored(string storeAdminIdentityId, string managerId);


        /////////////////////
        ///

        IQueryable<ManagerEntity> GetAllTest();
    }
}
