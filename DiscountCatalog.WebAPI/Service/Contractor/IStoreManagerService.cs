using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Models.ManyToManyModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Service.Contractor
{
    public interface IStoreManagerService
    {
        StoreEntity FilterManagers(StoreEntity store);
        IList<ManagerEntity> FilterStores(IList<ManagerEntity> managers);
        StoreEntity FilterStoreAdmin(StoreEntity store);
        IList<ManagerEntity> FilterStoreAdmin(IList<ManagerEntity> managers);

        StoreManagers GetStoreManagers(string storeAdminIdentityId, string storeId, string sortOrder, string searchString, int pageIndex, int pageSize);
        Result Assign(string storeAdminIdentityId, string storeId, string managerId);
        Result Unassign(string storeAdminIdentityId, string storeId, string managerId);
    }
}
