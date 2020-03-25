using DiscountCatalog.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor
{
    public interface IManagerStoreRepository : IRepository<ManagerStore>
    {
        IEnumerable<ManagerStore> GetManagerStores(string id, string sortOrder, string searchString);

        Result Assign(string storeAdminId, string managerId, string storeId);
        Result Unassign(string storeAdminId, string managerId, string storeId);
    }
}
