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

        public async Task<Response> CreateProductAsync(WebApiProduct product)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {

                    ProductEntity processedProduct = await ProductProcessor.WebApiProductToProductEntityAsync(product);

                    var discountModel = new DiscountModel
                    {
                        OldPrice = processedProduct.ProductOldPrice,
                        NewPrice = processedProduct.ProductNewPrice,
                        Discount = processedProduct.DiscountPercentage
                    };

                    DiscountResponseModel discountResponse = DiscountProcessor.DiscountCalculator(discountModel);

                    if (!discountResponse.Success)
                    {
                        response.Message = discountResponse.Message;
                        response.Success = false;

                        return response;
                    }

                    if (discountResponse.Discount.OldPrice != 0 && discountResponse.Discount.NewPrice != 0 && discountResponse.Discount.Discount != 0)
                    {
                        processedProduct.ProductOldPrice = discountResponse.Discount.OldPrice;
                        processedProduct.ProductNewPrice = discountResponse.Discount.NewPrice;
                        processedProduct.DiscountPercentage = discountResponse.Discount.Discount;
                    }
                    else
                    {
                        response.Message = "An error has occured!";
                        response.Success = false;

                        return response;
                    }

                    if (processedProduct.ProductNewPrice > processedProduct.ProductOldPrice)
                    {
                        response.Message = "New price has to be a discount";
                        response.Success = false;

                        return response;
                    }

                    DateTime discountDateEnd = DateTime.Parse(processedProduct.DiscountDateEnd);
                    DateTime discountDateBegin = DateTime.Parse(processedProduct.DiscountDateBegin);

                    if (DateTime.Compare(discountDateBegin, discountDateEnd) >= 0)
                    {
                        response.Message = "Discount end date cannot be earlier or same as discount begin date!";
                        response.Success = false;

                        return response;
                    }

                    if (DateTime.Compare(discountDateEnd, DateTime.Now) >= 0)
                    {
                        processedProduct.Expired = false;
                    }
                    else
                    {
                        processedProduct.Expired = true;
                    }

                    processedProduct.Store = await context.Stores.FindAsync(product.Store.Id);

                    processedProduct.Deleted = false;
                    processedProduct.Approved = true;

                    processedProduct.DateCreated = DateTime.Now;

                    context.Products.Add(processedProduct);

                    await context.SaveChangesAsync();

                    response.Message = "Successfully created";
                    response.Success = true;

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