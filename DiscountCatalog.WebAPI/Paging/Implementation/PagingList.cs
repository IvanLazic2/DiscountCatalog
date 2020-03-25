using DiscountCatalog.WebAPI.Paging.Contractor;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Paging.Implementation
{
    public class PagingList<TEntity> : IPagingList<TEntity> where TEntity : class
    {
        public PagingList(IPagedList<TEntity> items, IPagedList metaData)
        {
            Items = items;
            MetaData = metaData;
        }

        public IPagedList<TEntity> Items { get; set; }
        public IPagedList MetaData { get; set; }
    }
}