using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.DataBaseValidation;
using AbatementHelper.WebAPI.Extentions;
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
                    var tasks = new List<Task<WebApiProduct>>();

                    List<ProductEntity> productEntities = context.Products.Where(p => p.Store.Id == id && !p.Deleted && !p.Expired && p.Approved).ToList();

                    if (productEntities != null)
                    {
                        foreach (var product in productEntities)
                        {
                            product.Store = await GetStoreAsync(id);
                            tasks.Add(Task.Run(() => ProductProcessor.ProductEntityToWebApiProductAsync(product)));
                        }

                        var results = await Task.WhenAll(tasks);

                        products = results.ToList();
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

        public async Task<ModelStateResponse> CreateProductAsync(WebApiProduct product)
        {
            var response = new ModelStateResponse();

            try
            {
                ProductEntity processedProduct = await ProductProcessor.WebApiProductToProductEntityAsync(product);

                ProductValidation validation = new ProductValidation();

                ModelStateResponse modelState = validation.Validate(processedProduct);

                if (modelState.IsValid)
                {
                    processedProduct.ProductOldPrice = validation.OldPrice;
                    processedProduct.ProductNewPrice = validation.NewPrice;
                    processedProduct.DiscountPercentage = validation.Discount;

                    DateTime discountDateEnd = DateTime.Parse(processedProduct.DiscountDateEnd);
                    DateTime discountDateBegin = DateTime.Parse(processedProduct.DiscountDateBegin);

                    if (DateTime.Compare(discountDateEnd, DateTime.Now) >= 0)
                    {
                        processedProduct.Expired = false;
                    }
                    else
                    {
                        processedProduct.Expired = true;
                    }

                    using (var context = new ApplicationUserDbContext())
                    {
                        processedProduct.Store = await context.Stores.FindAsync(product.Store.Id);

                        processedProduct.Deleted = false;
                        processedProduct.Approved = true;

                        processedProduct.DateCreated = DateTime.Now;

                        context.Products.Add(processedProduct);

                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    response = modelState;
                }

            }
            catch (Exception exception)
            {
                //response.Message = exception.InnerException.InnerException.Message;
            }

            return response;
        }

        public async Task<ModelStateResponse> EditProductAsync(WebApiProduct product)
        {
            var response = new ModelStateResponse();

            try
            {
                ProductEntity processedProduct = await ProductProcessor.WebApiProductToProductEntityAsync(product);

                ProductValidation validation = new ProductValidation();

                ModelStateResponse modelState = validation.Validate(processedProduct);

                if (modelState.IsValid)
                {
                    processedProduct.ProductOldPrice = validation.OldPrice;
                    processedProduct.ProductNewPrice = validation.NewPrice;
                    processedProduct.DiscountPercentage = validation.Discount;

                    DateTime discountDateEnd = DateTime.Parse(processedProduct.DiscountDateEnd);
                    DateTime discountDateBegin = DateTime.Parse(processedProduct.DiscountDateBegin);

                    if (DateTime.Compare(discountDateEnd, DateTime.Now) >= 0)
                    {
                        processedProduct.Expired = false;
                    }
                    else
                    {
                        processedProduct.Expired = true;
                    }
                    using (var context = new ApplicationUserDbContext())
                    {
                        ProductEntity productEntity = await context.Products.FindAsync(product.Id);

                        context.Products.Attach(productEntity);

                        productEntity.ProductName = processedProduct.ProductName;
                        productEntity.CompanyName = processedProduct.CompanyName;
                        productEntity.ProductOldPrice = processedProduct.ProductOldPrice;
                        productEntity.ProductNewPrice = processedProduct.ProductNewPrice;
                        productEntity.DiscountPercentage = processedProduct.DiscountPercentage;
                        productEntity.DiscountDateBegin = processedProduct.DiscountDateBegin;
                        productEntity.DiscountDateEnd = processedProduct.DiscountDateEnd;
                        productEntity.Quantity = processedProduct.Quantity;
                        productEntity.Description = processedProduct.Description;
                        productEntity.Note = processedProduct.Note;

                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    response = modelState;
                }
            }
            catch (Exception ex)
            {
                throw;
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
                    var tasks = new List<Task<WebApiProduct>>();

                    List<ProductEntity> productEntities = new List<ProductEntity>();

                    productEntities = context.Products.Where(p => p.Store.Id == id && p.Deleted).ToList();

                    if (productEntities != null)
                    {
                        foreach (var product in productEntities)
                        {
                            tasks.Add(Task.Run(() => ProductProcessor.ProductEntityToWebApiProductAsync(product)));
                        }

                        var results = await Task.WhenAll(tasks);

                        products = results.ToList();
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
                    var tasks = new List<Task<WebApiProduct>>();

                    List<ProductEntity> productEntities = new List<ProductEntity>();

                    productEntities = context.Products.Where(p => p.Store.Id == id && p.Expired && !p.Deleted).ToList();

                    if (productEntities != null)
                    {
                        foreach (var product in productEntities)
                        {
                            tasks.Add(Task.Run(() => ProductProcessor.ProductEntityToWebApiProductAsync(product)));
                        }

                        var results = await Task.WhenAll(tasks);

                        products = results.ToList();
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