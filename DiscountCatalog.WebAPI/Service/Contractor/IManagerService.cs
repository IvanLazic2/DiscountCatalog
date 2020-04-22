using AbatementHelper.WebAPI.Models.BindingModels;
using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.REST.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Service.Contractor
{
    public interface IManagerService
    {
        Task<Result> CreateAsync(ManagerRESTPost manager);
        ManagerREST Get(string storeAdminIdentityId, string managerId);
        IPagingList<ManagerREST> GetAll(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize);
        Task<Result> UpdateAsync(string storeAdminIdentityId, ManagerRESTPut manager);
        IPagingList<ManagerREST> GetAllDeleted(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize);
        Result Delete(string storeAdminIdentityId, string managerId);
        Result Restore(string storeAdminIdentityId, string managerId);
        Task<Result> PostImageAsync(string storeAdminIdentityId, string managerId, byte[] image);
        Task<byte[]> GetImageAsync(string managerId);

        IList<ManagerEntity> FilterStores(IList<ManagerEntity> managers, bool clear);
        ManagerEntity FilterStores(ManagerEntity manager, bool clear);
        IList<ManagerEntity> FilterStoreAdmin(IList<ManagerEntity> managers);
        ManagerEntity FilterStoreAdmin(ManagerEntity manager);
    }
}
