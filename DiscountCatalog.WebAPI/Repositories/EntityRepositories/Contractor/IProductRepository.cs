using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor
{
    public interface IProductRepository : IRepository<ProductEntity>
    {
        IEnumerable<ProductEntity> GetProductsWithStore();
    }
}
