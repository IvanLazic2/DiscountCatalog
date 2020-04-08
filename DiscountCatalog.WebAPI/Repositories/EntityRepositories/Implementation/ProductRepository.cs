using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor;
using DiscountCatalog.Common.Models;
using System.Threading.Tasks;
using DiscountCatalog.WebAPI.ModelState;
using DiscountCatalog.WebAPI.Validation.Validators;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.REST.Product;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Implementation
{
    public class ProductRepository : Repository<ProductEntity>, IProductRepository
    {
        #region Properties

        public ApplicationUserDbContext DbContext
        {
            get
            {
                return Context as ApplicationUserDbContext;
            }
        }

        #endregion

        #region Constructors

        public ProductRepository(ApplicationUserDbContext context)
            : base(context)
        {
        }

        #endregion

        #region Methods

        //ENTITY TESTING

        #region EntityTesting

        public bool IsExpired(ProductEntity product)
        {
            var date = DateTime.Parse(product.DiscountDateEnd);

            if (date != null)
            {
                if (DateTime.Compare(date, DateTime.Now) < 0)
                {
                    MarkAsExpired(product.Store.Id, product.Id);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        //CREATE

        #region Create

        public Result Create(string storeId, ProductEntity product)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Product created."
            };

            UnitOfWork uow = new UnitOfWork(DbContext);

            product.Store = uow.Stores.GetApproved(storeId);

            product.DateCreated = DateTime.Now;
            product.DateUpdated = DateTime.Now;

            product = DiscountProcessor.CalculateDiscount(product);

            var validator = new ProductValidator();
            var validationResult = validator.Validate(product);
            modelState.Add(validationResult);

            if (validationResult.IsValid)
            {
                Add(product);
            }

            return modelState.GetResult();
        }

        #endregion

        //GET ALL

        #region GetAllApproved

        public IEnumerable<ProductEntity> GetAllApproved()
        {
            return DbContext.Products
                .Include(p => p.Store)
                .Where(p => p.Approved && !p.Deleted && !p.Expired)
                .ToList();
        }

        public IEnumerable<ProductEntity> GetAllApproved(string storeId)
        {
            return DbContext.Products
                .Include(p => p.Store)
                .Where(p => p.Store.Id == storeId && p.Approved && !p.Deleted && !p.Expired)
                .ToList();
        }

        #endregion

        #region GetAllDeleted

        public IEnumerable<ProductEntity> GetAllDeleted()
        {
            return DbContext.Products
                .Include(p => p.Store)
                .Where(p => p.Deleted)
                .ToList();
        }

        public IEnumerable<ProductEntity> GetAllDeleted(string storeId)
        {
            return DbContext.Products
                .Include(p => p.Store)
                .Where(p => p.Store.Id == storeId && p.Deleted)
                .ToList();
        }

        #endregion

        #region GetAllExpired

        public IEnumerable<ProductEntity> GetAllExpired()
        {
            return DbContext.Products
                .Include(p => p.Store)
                .Where(p => p.Approved && !p.Deleted && p.Expired)
                .ToList();
        }

        public IEnumerable<ProductEntity> GetAllExpired(string storeId)
        {
            return DbContext.Products
                .Include(p => p.Store)
                .Where(p => p.Store.Id == storeId && p.Expired && p.Approved && !p.Deleted)
                .ToList();
        }

        #endregion

        #region GetAllLoaded

        public IEnumerable<ProductEntity> GetAllLoaded()
        {
            return DbContext.Products
                .Include(p => p.Store)
                .ToList();
        }

        public IEnumerable<ProductEntity> GetAllLoaded(string storeId)
        {
            return DbContext.Products
                .Include(p => p.Store)
                .Where(p => p.Store.Id == storeId)
                .ToList();
        }

        #endregion

        //GET

        #region GetApproved

        public ProductEntity GetApproved(string productId)
        {
            return DbContext.Products
                .Include(p => p.Store)
                .FirstOrDefault(p => p.Id == productId && !p.Deleted && !p.Expired && p.Approved); //mozda maknit !p.Expired
        }

        public ProductEntity GetApproved(string storeId, string productId)
        {
            return DbContext.Products
                .Include(p => p.Store)
                .FirstOrDefault(p => p.Store.Id == storeId && p.Id == productId && p.Approved && !p.Deleted && !p.Expired);
        }

        #endregion

        #region GetLoaded

        public ProductEntity GetLoaded(string productId)
        {
            return DbContext.Products
                .Include(p => p.Store)
                .FirstOrDefault(p => p.Id == productId);
        }

        public ProductEntity GetLoaded(string storeId, string productId)
        {
            return DbContext.Products
                .Include(p => p.Store)
                .FirstOrDefault(p => p.Store.Id == storeId && p.Id == productId);
        }

        #endregion

        //UPDATE

        #region Update

        public async Task<Result> UpdateAsync(ProductRESTPut product)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Info updated."
            };

            ProductEntity dbProduct = GetApproved(product.Id);

            if (dbProduct != null)
            {
                dbProduct.ProductName = product.ProductName;
                dbProduct.CompanyName = product.CompanyName;
                dbProduct.OldPrice = product.OldPrice;
                dbProduct.NewPrice = product.NewPrice;
                dbProduct.Currency = product.Currency;
                dbProduct.DiscountPercentage = product.DiscountPercentage;
                dbProduct.DiscountDateBegin = product.DiscountDateBegin;
                dbProduct.DiscountDateEnd = product.DiscountDateEnd;
                dbProduct.Quantity = product.Quantity;
                dbProduct.MeasuringUnit = product.MeasuringUnit;
                dbProduct.Description = product.Description;
                dbProduct.Note = product.Note;

                dbProduct.DateUpdated = DateTime.Now;

                dbProduct = DiscountProcessor.CalculateDiscount(dbProduct);

                var validator = new ProductValidator();
                var validationResult = await validator.ValidateAsync(dbProduct);
                modelState.Add(validationResult);
            }
            return modelState.GetResult();
        }

        public async Task<Result> UpdateAsync(string storeId, ProductRESTPut product)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Info updated."
            };

            ProductEntity dbProduct = GetApproved(storeId, product.Id);

            if (dbProduct != null)
            {
                dbProduct.ProductName = product.ProductName;
                dbProduct.CompanyName = product.CompanyName;
                dbProduct.OldPrice = product.OldPrice;
                dbProduct.NewPrice = product.NewPrice;
                dbProduct.Currency = product.Currency;
                dbProduct.DiscountPercentage = product.DiscountPercentage;
                dbProduct.DiscountDateBegin = product.DiscountDateBegin;
                dbProduct.DiscountDateEnd = product.DiscountDateEnd;
                dbProduct.Quantity = product.Quantity;
                dbProduct.MeasuringUnit = product.MeasuringUnit;
                dbProduct.Description = product.Description;
                dbProduct.Note = product.Note;

                dbProduct.DateUpdated = DateTime.Now;

                dbProduct = DiscountProcessor.CalculateDiscount(dbProduct);

                var validator = new ProductValidator();
                var validationResult = await validator.ValidateAsync(dbProduct);
                modelState.Add(validationResult);
            }
            return modelState.GetResult();
        }

        #endregion

        //DELETE

        #region MarkAsDeleted

        public Result MarkAsDeleted(string productId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Product deleted."
            };

            ProductEntity product = GetApproved(productId);

            if (product != null)
            {
                product.Deleted = true;
            }
            else
            {
                modelState.Add("Product does not exist.");
            }

            return modelState.GetResult();
        }

        public Result MarkAsDeleted(string storeId, string productId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Product deleted."
            };

            ProductEntity product = GetApproved(storeId, productId);

            if (product != null)
            {
                product.Deleted = true;
            }
            else
            {
                modelState.Add("Product does not exist.");
            }

            return modelState.GetResult();
        }

        #endregion

        //RESTORE

        #region MarkAsRestored

        public Result MarkAsRestored(string productId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Product restored."
            };

            ProductEntity product = DbContext.Products
                .FirstOrDefault(p => p.Id == productId && p.Deleted);

            if (product != null)
            {
                product.Deleted = false;
            }
            else
            {
                modelState.Add("Product does not exist.");
            }

            return modelState.GetResult();
        }

        public Result MarkAsRestored(string storeId, string productId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Product restored."
            };

            ProductEntity product = DbContext.Products
                .Include(m => m.Store)
                .FirstOrDefault(p => p.Store.Id == storeId && p.Id == productId && p.Deleted);

            if (product != null)
            {
                product.Deleted = false;
            }
            else
            {
                modelState.Add("Product does not exist.");
            }

            return modelState.GetResult();
        }

        #endregion

        //EXPIRE

        #region MarkAsExpired

        public Result MarkAsExpired(string productId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Product set as expired."
            };

            ProductEntity product = GetApproved(productId);

            if (product != null)
            {
                product.Expired = true;
            }
            else
            {
                modelState.Add("Product does not exist.");
            }

            return modelState.GetResult();
        }

        public Result MarkAsExpired(string storeId, string productId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Product set as expired."
            };

            ProductEntity product = GetApproved(storeId, productId);

            if (product != null)
            {
                product.Expired = true;
            }
            else
            {
                modelState.Add("Product does not exist.");
            }

            return modelState.GetResult();
        }

        #endregion

        //GET/POST IMAGE

        #region Get/Post Image

        public Result PostProductImage(string productId, byte[] image)
        {
            var result = new Result();

            if (ImageProcessor.IsValid(image))
            {
                ProductEntity product = GetApproved(productId);

                if (product != null)
                {
                    product.ProductImage = image;
                }
                else
                {
                    result.Add("Product does not exist.");
                }
            }
            else
            {
                result.Add("Image is not valid.");
            }

            return result;
        }

        public byte[] GetProductImage(string productId)
        {
            ProductEntity product = GetLoaded(productId);

            return product.ProductImage;
        }

        #endregion

        #endregion

    }
}