using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Paging
{
    //[JsonObject]
    //public class SerializablePagedList<T> : PagedList<T>
    //{
    //    public SerializablePagedList(IQueryable<T> subset, int pageNumber, int pageSize)
    //        : base(subset, pageNumber, pageSize)
    //    {
    //    }

    //    public SerializablePagedList(IEnumerable<T> subset, int pageNumber, int pageSize)
    //        : base(subset, pageNumber, pageSize)
    //    {
    //    }

    //    public IEnumerable<T> Items
    //    {
    //        get
    //        {
    //            return Subset;
    //        }
    //    }
    //}
}