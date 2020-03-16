using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Models.Paging
{
    public class PagingEntity<T> where T : class
    {
        public PagingEntity()
        {
            Items = new List<T>();
        }
        public IEnumerable<T> Items { get; set; }
        public PagingMetaData MetaData { get; set; }
    }
}