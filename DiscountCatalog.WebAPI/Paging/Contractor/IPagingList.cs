using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Paging.Contractor
{
    public interface IPagingList<TEntity> where TEntity : class
    {
        IPagedList<TEntity> Items { get; set; }
        IPagedList MetaData { get; set; }
    }
}
