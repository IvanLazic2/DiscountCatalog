using AbatementHelper.WebAPI.Models.BindingModels;
using DiscountCatalog.Common.Models;
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

        IPagingList<ManagerStore> GetManagerStores(string storeAdminIdentityId, string managerId, string sortOrder, string searchString, int pageIndex, int pageSize);

        IPagingList<ManagerREST> GetAllDeleted(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize);

        Result Delete(string storeAdminIdentityId, string managerId);

        Result Restore(string storeAdminIdentityId, string managerId);

        Task<Result> PostImageAsync(string storeAdminIdentityId, string managerId, byte[] image);

        Task<byte[]> GetImageAsync(string managerId);

        Result Assign(string storeAdminIdentityId, string managerId, string storeId);

        Result Unassign(string storeAdminIdentityId, string managerId, string storeId);

        IList<ManagerREST> FilterStores(IList<ManagerREST> managers);

        ManagerREST FilterStores(ManagerREST manager);

        IList<ManagerREST> FilterStoreAdmin(IList<ManagerREST> managers);

        ManagerREST FilterStoreAdmin(ManagerREST manager);
    }
}
