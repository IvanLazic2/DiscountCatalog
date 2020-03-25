using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Service.Contractor
{
    public interface IService<TEntity> where TEntity : class
    {
        IList<TEntity> Search(IList<TEntity> list, string searchString);
        IList<TEntity> Order(IList<TEntity> list, string sortOrder);
    }
}
