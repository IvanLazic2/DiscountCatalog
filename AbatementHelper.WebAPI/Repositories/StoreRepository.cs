using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class StoreRepository
    {
        public async Task<List<WebApiProduct>> GetAllProductsAsync(string id)
        {
            List<WebApiProduct> products = new List<WebApiProduct>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var productEntities = context.Products.Where(p => p.Store.Id == id && !p.Deleted && !p.Expired && p.Approved);

                    if (productEntities != null)
                    {
                        foreach (var product in productEntities)
                        {
                            product.Store = await GetStoreAsync(id);
                            products.Add(await ProductProcessor.ProductEntityToWebApiProductAsync(product));
                        }
                    }
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return products;
        }

        public async Task<StoreEntity> GetStoreAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity storeEntity = await context.Stores.FindAsync(id);

                return storeEntity;
            }
        }

        public async Task<Response> CreateProductAsync(WebApiProduct product)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    if (product.ProductNewPrice < product.ProductOldPrice)
                    {
                        ProductEntity processedProduct = await ProductProcessor.WebApiProductToProductEntityAsync(product);

                        processedProduct.Store = await context.Stores.FindAsync(product.Store.Id);

                        processedProduct.Deleted = false;
                        processedProduct.Approved = true;

                        if (DateTime.Compare(processedProduct.DiscountDateEnd, DateTime.Now) >= 0)
                        {
                            processedProduct.Expired = false;
                        }
                        else
                        {
                            processedProduct.Expired = true;
                        }

                        processedProduct.DateCreated = DateTime.Now;

                        context.Products.Add(processedProduct);

                        await context.SaveChangesAsync();

                        response.Message = "Successfully created";
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = "New price has to be a discount!";
                        response.Success = false;
                    }
                }
            }
            catch (Exception exception)
            {
                response.Message = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<Response> EditProductAsync(WebApiProduct product)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    if (product.ProductNewPrice < product.ProductOldPrice)
                    {
                        ProductEntity productEntity = await context.Products.FindAsync(product.Id);

                        context.Products.Attach(productEntity);

                        productEntity.ProductName = product.ProductName;
                        productEntity.CompanyName = product.CompanyName;
                        productEntity.ProductOldPrice = product.ProductOldPrice;
                        productEntity.ProductNewPrice = product.ProductNewPrice;
                        productEntity.DiscountPercentage = product.DiscountPercentage;
                        productEntity.DiscountDateBegin = product.DiscountDateBegin;
                        productEntity.DiscountDateEnd = product.DiscountDateEnd;
                        productEntity.Quantity = product.Quantity;
                        productEntity.Description = product.Description;
                        productEntity.Note = product.Note;

                        await context.SaveChangesAsync();

                        response.Message = "Successfully edited.";
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = "New price has to be a discount!";
                        response.Success = false;
                    }
                }
            }
            catch (DbUpdateException exception)
            {
                response.Message = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<ProductEntity> ReadProductByIdAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ProductEntity product = await context.Products.FindAsync(id);

                return product;
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ProductEntity product = await context.Products.FindAsync(id);

                context.Products.Attach(product);

                product.Deleted = true;

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<WebApiProduct>> GetAllDeletedProductsAsync(string id)
        {
            List<WebApiProduct> products = new List<WebApiProduct>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    List<ProductEntity> productEntities = new List<ProductEntity>();

                    productEntities = context.Products.Where(p => p.Store.Id == id && p.Deleted).ToList();

                    if (productEntities != null)
                    {
                        foreach (var product in productEntities)
                        {
                            products.Add(await ProductProcessor.ProductEntityToWebApiProductAsync(product));
                        }
                    }
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return products;
        }

        public async Task RestoreProductAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ProductEntity product = await context.Products.FindAsync(id);

                context.Products.Attach(product);

                product.Deleted = false;

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<WebApiProduct>> GetAllExpiredProductsAsync(string id)
        {
            List<WebApiProduct> products = new List<WebApiProduct>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    List<ProductEntity> productEntities = new List<ProductEntity>();

                    productEntities = context.Products.Where(p => p.Store.Id == id && p.Expired && !p.Deleted).ToList();

                    if (productEntities != null)
                    {
                        foreach (var product in productEntities)
                        {
                            products.Add(await ProductProcessor.ProductEntityToWebApiProductAsync(product));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return products;
        }

        public async Task<Response> PostProductImageAsync(WebApiPostImage product)
        {
            Response response = new Response();

            byte[] image = product.Image;

            if (ImageProcessor.IsValid(image))
            {
                try
                {
                    using (var context = new ApplicationUserDbContext())
                    {
                        ProductEntity productEntity = await context.Products.FindAsync(product.Id);

                        context.Products.Attach(productEntity);

                        productEntity.ProductImage = image;

                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception exception)
                {
                    response.Message = exception.InnerException.InnerException.Message;
                    response.Success = false;
                }

                response.Message = "Successfully uploaded.";
                response.Success = true;
            }
            else
            {
                response.Message = "Invalid image type";
                response.Success = false;
            }


            return response;
        }

        public async Task<byte[]> GetProductImageAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ProductEntity product = await context.Products.FindAsync(id);

                return product.ProductImage;
            }

        }
    }
}