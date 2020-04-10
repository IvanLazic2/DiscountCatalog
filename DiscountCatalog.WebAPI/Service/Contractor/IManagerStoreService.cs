using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Models.ManyToManyModels.Manager;
using DiscountCatalog.WebAPI.REST.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Service.Contractor
{
    public interface IManagerStoreService
    {
        ManagerEntity FilterStores(ManagerEntity manager);
        IList<StoreEntity> FilterManagers(IList<StoreEntity> stores);
        ManagerEntity FilterStoreAdmin(ManagerEntity manager);
        IList<StoreEntity> FilterStoreAdmin(IList<StoreEntity> stores);
        

        ManagerStores GetManagerStores(string storeAdminIdentityId, string managerId, string sortOrder, string searchString, int pageIndex, int pageSize);

        Result Assign(string storeAdminIdentityId, string managerId, string storeId);
        Result Unassign(string storeAdminIdentityId, string managerId, string storeId);
    }
}
