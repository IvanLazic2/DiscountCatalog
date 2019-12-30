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
        public List<WebApiProduct> GetAllProducts(string id)
        {
            List<WebApiProduct> products = new List<WebApiProduct>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    //List<ProductEntity> productEntities = new List<ProductEntity>();

                    var productEntities = context.Products.Where(p => p.Store.Id == id && !p.Deleted && !p.Expired && p.Approved);



                    if (productEntities != null)
                    {
                        foreach (var product in productEntities)
                        {
                            product.Store = GetStore(id);
                            products.Add(ProductProcessor.ProductEntityToWebApiProduct(product));
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

        public StoreEntity GetStore(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity storeEntity = context.Stores.Find(id);

                return storeEntity;
            }
        }

        public Response CreateProduct(WebApiProduct product)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    if (product.ProductNewPrice < product.ProductOldPrice)
                    {
                        ProductEntity processedProduct = ProductProcessor.WebApiProductToProductEntity(product);
                        processedProduct.Store = context.Stores.Find(product.Store.Id);

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

                        context.SaveChanges();

                        response.ResponseMessage = "Successfully created";
                        response.Success = true;
                    }
                    else
                    {
                        response.ResponseMessage = "New price has to be a discount!";
                        response.Success = false;
                    }
                }
            }
            catch (Exception exception)
            {
                response.ResponseMessage = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            return response;
        }

        public Response EditProduct(WebApiProduct product)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    if (product.ProductNewPrice < product.ProductOldPrice)
                    {
                        ProductEntity productEntity = context.Products.Find(product.Id);
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

                        context.SaveChanges();

                        response.ResponseMessage = "Successfully edited.";
                        response.Success = true;
                    }
                    else
                    {
                        response.ResponseMessage = "New price has to be a discount!";
                        response.Success = false;
                    }
                }
            }
            catch (DbUpdateException exception)
            {
                response.ResponseMessage = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<ProductEntity> ReadProductById(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ProductEntity product = context.Products.Find(id);

                return product;
            }
        }

        public void DeleteProduct(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ProductEntity product = context.Products.Find(id);
                context.Products.Attach(product);
                product.Deleted = true;
                context.SaveChanges();
            }
        }

        public List<WebApiProduct> GetAllDeletedProducts(string id)
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
                            products.Add(ProductProcessor.ProductEntityToWebApiProduct(product));
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

        public void RestoreProduct(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ProductEntity product = context.Products.Find(id);
                context.Products.Attach(product);
                product.Deleted = false;
                context.SaveChanges();
            }
        }

        public List<WebApiProduct> GetAllExpiredProducts(string id)
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
                            products.Add(ProductProcessor.ProductEntityToWebApiProduct(product));
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

        public Response PostProductImage(WebApiProduct product)
        {
            Response response = new Response();

            byte[] image = product.ProductImage;

            if (ImageProcessor.IsValid(image))
            {
                try
                {
                    using (var context = new ApplicationUserDbContext())
                    {
                        ProductEntity productEntity = context.Products.Find(product.Id);

                        context.Products.Attach(productEntity);
                        productEntity.ProductImage = image;

                        context.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    response.ResponseMessage = exception.InnerException.InnerException.Message;
                    response.Success = false;
                }

                response.ResponseMessage = "Successfully uploaded.";
                response.Success = true;
            }
            else
            {
                response.ResponseMessage = "Invalid image type";
                response.Success = false;
            }


            return response;
        }

        public byte[] GetProductImage(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ProductEntity product = context.Products.Find(id);

                return product.ProductImage;
            }

        }
    }
}