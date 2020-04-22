using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Filter.Contractor
{
    public interface IFilter<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Filter(IQueryable<TEntity> entities);
        TEntity Filter(TEntity entity);
    }
}
