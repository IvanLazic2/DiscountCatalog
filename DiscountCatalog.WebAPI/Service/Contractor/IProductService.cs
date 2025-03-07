﻿using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.REST.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Service.Contractor
{
    public interface IProductService
    {
        Result Create(ProductRESTPost product);

        ProductREST Get(string storeId, string productId);

        //ProductREST Get(string productId);/**/

        ProductREST GetExpired(string storeId, string productId);

        IPagingList<ProductREST> GetAll(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming);

        //IPagingList<ProductREST> GetAll(string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming);/**/

        IPagingList<ProductREST> GetAllDeleted(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming);

        IPagingList<ProductREST> GetAllExpired(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming);

        Task<Result> UpdateAsync(string storeId, ProductRESTPut product);

        Result Delete(string storeId, string productId);

        Result Restore(string storeId, string productId);

        Result PostProductImage(string storeId, string productId, byte[] image);

        byte[] GetImage(string productId);

        decimal GetMinPrice(string storeId);

        decimal GetMaxPrice(string storeId);

        //decimal GetMinPrice(IEnumerable<ProductEntity> products);

        //decimal GetMaxPrice(IEnumerable<ProductEntity> products);

        IEnumerable<ProductEntity> FilterPrice(IEnumerable<ProductEntity> products, string priceFilter);

        IEnumerable<ProductEntity> FilterDate(IEnumerable<ProductEntity> products, string dateFilter, bool includeUpcoming);

        IEnumerable<ProductEntity> FilterStoreAdmin(IEnumerable<ProductEntity> products);
        ProductEntity FilterStoreAdmin(ProductEntity product);


    }
}
