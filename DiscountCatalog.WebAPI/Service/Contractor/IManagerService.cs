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

        ManagerREST Get(string storeAdminId, string managerId);

        IPagingList<ManagerREST> GetAll(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize);

        Task<Result> UpdateAsync(string storeAdminId, ManagerRESTPut manager);

        IPagingList<ManagerStore> GetManagerStores(string storeAdminId, string managerId, string sortOrder, string searchString, int pageIndex, int pageSize);

        IPagingList<ManagerREST> GetAllDeleted(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize);

        Result Delete(string storeAdminId, string managerId);

        Result Restore(string storeAdminId, string managerId);

        Task<Result> PostImageAsync(string storeAdminId, string managerId, byte[] image);

        Task<byte[]> GetImageAsync(string managerId);

        Result Assign(string storeAdminId, string managerId, string storeId);

        Result Unassign(string storeAdminId, string managerId, string storeId);
    }
}
