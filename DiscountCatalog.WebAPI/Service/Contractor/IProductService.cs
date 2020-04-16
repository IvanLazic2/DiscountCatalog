using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models;
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

        IPagingList<ProductREST> GetAll(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize);

        IPagingList<ProductREST> GetAllDeleted(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize);

        IPagingList<ProductREST> GetAllExpired(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize);

        Task<Result> UpdateAsync(string storeId, ProductRESTPut product);

        Result Delete(string storeId, string productId);

        Result Restore(string storeId, string productId);

        Result PostProductImage(string storeId, string productId, byte[] image);

        byte[] GetImage(string productId);

        decimal GetMinPrice(string storeId);

        decimal GetMaxPrice(string storeId);
    }
}
