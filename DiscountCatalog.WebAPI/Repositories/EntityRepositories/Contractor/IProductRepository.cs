using DiscountCatalog.Common.Models;
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
        Result Create(ProductEntity product, string storeId);

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

        Result PostProductImage(string id, byte[] image);
        byte[] GetProductImage(string id);

        Task<Result> UpdateAsync(ProductEntity product);
        Task<Result> UpdateAsync(string storeId, ProductEntity product);

        Result MarkAsDeleted(string productId);
        Result MarkAsRestored(string productId);
        Result MarkAsExpired(string productId);
        Result MarkAsDeleted(string storeId, string productId);
        Result MarkAsRestored(string storeId, string productId);
        Result MarkAsExpired(string storeId, string productId);

        //refresh
    }
}
