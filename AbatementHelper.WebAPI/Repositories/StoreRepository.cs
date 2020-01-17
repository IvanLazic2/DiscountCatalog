using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.EntityValidation;
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
        public async Task<WebApiListOfProductsResult> GetAllProductsAsync(string id)
        {
            var result = new WebApiListOfProductsResult();

            var products = new List<WebApiProduct>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiProduct>>();

                    List<ProductEntity> productEntities = context.Products.Where(p => p.Store.Id == id && !p.Deleted).ToList();

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
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            result.Products = products;

            return result;
        }

        public async Task<WebApiListOfProductsResult> GetAllActiveProductsAsync(string id)
        {
            var result = new WebApiListOfProductsResult();

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
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            result.Products = products;

            return result;
        }

        public async Task<StoreEntity> GetStoreAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity storeEntity = await context.Stores.FindAsync(id);

                return storeEntity;
            }
        }

        public async Task<WebApiResult> CreateProductAsync(WebApiProduct product)
        {
            var result = new WebApiResult();

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

                        if (processedProduct.Store == null)
                        {
                            result.AddModelError(string.Empty, "Store does not exist.");
                        }
                        else
                        {
                            processedProduct.Deleted = false;
                            processedProduct.Approved = true;

                            processedProduct.DateCreated = DateTime.Now;

                            context.Products.Add(processedProduct);

                            await context.SaveChangesAsync();

                            result.Message = "Product created.";
                        }
                    }
                }
                else
                {
                    foreach (var error in modelState.ModelState)
                    {
                        result.AddModelError(error.Key, error.Value);
                    }

                    result.Exception = modelState.Exception;
                }

            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiResult> EditProductAsync(WebApiProduct product)
        {
            var result = new WebApiResult();

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

                        if (productEntity == null)
                        {
                            result.AddModelError(string.Empty, "Product does not exist.");
                        }
                        else
                        {
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

                            result.Message = "Product updated.";
                        }
                    }
                }
                else
                {
                    foreach (var error in modelState.ModelState)
                    {
                        result.AddModelError(error.Key, error.Value);
                    }

                    result.Exception = modelState.Exception;
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiProductResult> ReadProductByIdAsync(string id)
        {
            var result = new WebApiProductResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ProductEntity product = await context.Products.FindAsync(id);

                    if (product != null)
                    {
                        WebApiProduct webApiProduct = await ProductProcessor.ProductEntityToWebApiProductAsync(product);

                        result.Product = webApiProduct;
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Product does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }


            return result;
        }

        public async Task<WebApiResult> DeleteProductAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ProductEntity product = await context.Products.FindAsync(id);

                    if (product != null)
                    {
                        context.Products.Attach(product);

                        product.Deleted = true;

                        await context.SaveChangesAsync();

                        result.Message = "Product deleted.";
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Product does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiListOfProductsResult> GetAllDeletedProductsAsync(string id)
        {
            var result = new WebApiListOfProductsResult();

            var products = new List<WebApiProduct>();

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
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            result.Products = products;

            return result;
        }

        public async Task<WebApiResult> RestoreProductAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ProductEntity product = await context.Products.FindAsync(id);

                    if (product != null)
                    {
                        context.Products.Attach(product);

                        product.Deleted = false;

                        await context.SaveChangesAsync();

                        result.Message = "Product restored.";
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Product does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiListOfProductsResult> GetAllExpiredProductsAsync(string id)
        {
            var result = new WebApiListOfProductsResult();

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
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            result.Products = products;

            return result;
        }

        public async Task<WebApiResult> PostProductImageAsync(WebApiPostImage product)
        {
            var result = new WebApiResult();

            var webApiProduct = new WebApiProduct();

            byte[] image = product.Image;

            if (image != null)
            {
                if (ImageProcessor.IsValid(image))
                {
                    try
                    {
                        using (var context = new ApplicationUserDbContext())
                        {
                            ProductEntity productEntity = await context.Products.FindAsync(product.Id);

                            if (productEntity != null)
                            {
                                context.Products.Attach(productEntity);

                                productEntity.ProductImage = image;

                                await context.SaveChangesAsync();

                                result.Message = "Image saved.";
                            }
                            else
                            {
                                result.AddModelError(string.Empty, "Product does not exist.");
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        result.Exception = exception;
                        result.AddModelError(string.Empty, "An exception has occured.");
                    }
                }
                else
                {
                    result.AddModelError(string.Empty, "Invalid image type.");
                }
            }
            else
            {
                result.AddModelError(string.Empty, "Image is empty.");
            }

            return result;
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