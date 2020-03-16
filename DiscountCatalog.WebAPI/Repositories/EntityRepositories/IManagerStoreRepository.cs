using DiscountCatalog.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories
{
    public interface IManagerStoreRepository : IRepository<ManagerStore>
    {
        IEnumerable<ManagerStore> GetManagerStores(string id, string sortOrder, string searchString);

        Result Assign(string managerId, string storeId);
        Result Unassign(string managerId, string storeId);
    }
}
