using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.REST.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor
{
    public interface IProductRepository : IRepository<ProductEntity>
    {
        Result Create(string storeId, ProductEntity product);

        IEnumerable<ProductEntity> GetAllApproved();
        IEnumerable<ProductEntity> GetAllDeleted();
        IEnumerable<ProductEntity> GetAllLoaded();
        IEnumerable<ProductEntity> GetAllExpired();
        IEnumerable<ProductEntity> GetAllApproved(string storeId);
        IEnumerable<ProductEntity> GetAllDeleted(string storeId);
        IEnumerable<ProductEntity> GetAllLoaded(string storeId);
        IEnumerable<ProductEntity> GetAllExpired(string storeId);

        ProductEntity GetLoaded(string productId);
        ProductEntity GetApproved(string productId);
        ProductEntity GetLoaded(string storeId, string productId);
        ProductEntity GetApproved(string storeId, string productId);

        Result PostProductImage(string productId, byte[] image);
        byte[] GetProductImage(string productId);

        Task<Result> UpdateAsync(ProductRESTPut product);
        Task<Result> UpdateAsync(string storeId, ProductRESTPut product);

        Result MarkAsDeleted(string productId);
        Result MarkAsRestored(string productId);
        Result MarkAsExpired(string productId);
        Result MarkAsDeleted(string storeId, string productId);
        Result MarkAsRestored(string storeId, string productId);
        Result MarkAsExpired(string storeId, string productId);

        bool IsExpired(ProductEntity product);

        //refresh
    }
}
