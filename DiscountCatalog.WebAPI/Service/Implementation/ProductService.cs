using AbatementHelper.WebAPI.Mapping;
using AutoMapper;
using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Extensions;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.Paging.Implementation;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.REST.Product;
using DiscountCatalog.WebAPI.Service.Contractor;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.WebAPI.Service.Implementation
{
    public class ProductService : IService<ProductREST>, IProductService
    {
        #region Properties

        readonly IMapper mapper;

        #endregion

        #region Constructors

        public ProductService()
        {
            mapper = AutoMapping.Initialise();
        }

        #endregion

        #region Methods

        //SEARCH/ORDER

        #region Search/Order

        public IList<ProductREST> Order(IList<ProductREST> products, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName).ToList();
                    break;
            }

            return products.ToList();
        }

        public IList<ProductREST> Search(IList<ProductREST> products, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString, StringComparer.OrdinalIgnoreCase) || p.Store.Administrator.Identity.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return products.ToList();
        }



        #endregion

        //FILTER

        #region Filter

        public IEnumerable<ProductEntity> FilterPrice(IEnumerable<ProductEntity> products, string priceFilter)
        {
            if (!string.IsNullOrEmpty(priceFilter))
            {
                string[] arr = priceFilter.Split(",".ToCharArray());

                int from = Convert.ToInt32(arr[0]);
                int to = Convert.ToInt32(arr[1]);

                products = products.Where(p => p.NewPrice >= from && p.NewPrice <= to);
            }

            return products;
        }

        public IEnumerable<ProductEntity> FilterDate(IEnumerable<ProductEntity> products, string dateFilter)
        {
            if (!string.IsNullOrEmpty(dateFilter))
            {
                DateTime date = DateTime.Parse(dateFilter);

                if (date != null)
                {
                    products = products.Where(p => DateTime.Parse(p.DiscountDateEnd).CompareTo(date) >= 0);
                }
            }

            return products;
        }

        public IEnumerable<ProductEntity> FilterStoreAdmin(IEnumerable<ProductEntity> products)
        {
            foreach (var product in products)
            {
                if (product.Store.Administrator.Stores != null)
                    product.Store.Administrator.Stores.Clear();
                if (product.Store.Administrator.Managers != null)
                    product.Store.Administrator.Managers.Clear();
            }

            return products;
        }

        public ProductEntity FilterStoreAdmin(ProductEntity product)
        {
            if (product.Store.Administrator.Stores != null)
                product.Store.Administrator.Stores.Clear();
            if (product.Store.Administrator.Managers != null)
                product.Store.Administrator.Managers.Clear();

            return product;
        }

        #endregion

        //CREATE

        #region Create

        public Result Create(ProductRESTPost product)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ProductEntity productEntity = mapper.Map<ProductEntity>(product);

                productEntity.Approved = true;
                productEntity.Deleted = false;

                Result result = uow.Products.Create(product.StoreId, productEntity);

                uow.Complete();

                return result;
            }
        }

        #endregion

        //GET ALL

        #region GetAll

        public IPagingList<ProductREST> GetAll(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IEnumerable<ProductEntity> products = uow.Products.GetAllApproved(storeId);

                products = FilterStoreAdmin(products);
                products = FilterDate(products, dateFilter);
                products = FilterPrice(products, priceFilter);

                IList<ProductREST> mapped = mapper.Map<IList<ProductREST>>(products);
                
                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                //mapped.ToList().ForEach(p => p.ProductImage = ImageProcessor.CreateThumbnail(p.ProductImage));
                mapped.ToList().ForEach(p => p.Store.StoreImage = ImageProcessor.CreateThumbnail(p.Store.StoreImage));
                mapped.ToList().ForEach(p => p.Store.Administrator.Identity.UserImage = ImageProcessor.CreateThumbnail(p.Store.Administrator.Identity.UserImage));

                IPagedList<ProductREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<ProductREST> result = new PagingList<ProductREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        //jos jedan getall

        public IPagingList<ProductREST> GetAllDeleted(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IEnumerable<ProductEntity> products = uow.Products.GetAllDeleted(storeId);

                IList<ProductREST> mapped = mapper.Map<IList<ProductREST>>(products);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<ProductREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<ProductREST> result = new PagingList<ProductREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        public IPagingList<ProductREST> GetAllExpired(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IEnumerable<ProductEntity> products = uow.Products.GetAllExpired(storeId);

                IList<ProductREST> mapped = mapper.Map<IList<ProductREST>>(products);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<ProductREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<ProductREST> result = new PagingList<ProductREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        #endregion

        //GET

        #region Get

        public ProductREST Get(string storeId, string productId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ProductEntity product = uow.Products.GetApproved(storeId, productId);

                var mapped = mapper.Map<ProductREST>(product);

                return mapped;
            }
        }

        #endregion

        //UPDATE

        #region Update

        public async Task<Result> UpdateAsync(string storeId, ProductRESTPut product)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = await uow.Products.UpdateAsync(storeId, product);

                if (result.Success)
                {
                    uow.Complete();
                }
                else
                {
                    uow.Dispose();
                }

                return result;
            }
        }

        #endregion

        //DELETE/RESTORE

        #region Delete/Restore

        public Result Delete(string storeId, string productId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Products.MarkAsDeleted(storeId, productId);

                uow.Complete();

                return result;
            }
        }

        public Result Restore(string storeId, string productId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Products.MarkAsRestored(storeId, productId);

                uow.Complete();

                return result;
            }
        }

        #endregion

        //GET/POST IMAGE

        #region Get/Post Image

        public byte[] GetImage(string productId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                byte[] image = uow.Products.GetProductImage(productId);

                //return ImageProcessor.CreateThumbnail(image);

                return image;
            }
        }

        public Result PostProductImage(string storeId, string productId, byte[] image)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Products.PostProductImage(productId, image);

                uow.Complete();

                return result;
            }
        }

        #endregion

        //PRICE

        #region Price

        public decimal GetMinPrice(string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IEnumerable<ProductEntity> products = uow.Products.GetAllApproved(storeId);

                decimal? min = products.Select(p => p.NewPrice).Min();

                return min.Value;
            }
        }

        public decimal GetMaxPrice(string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IEnumerable<ProductEntity> products = uow.Products.GetAllApproved(storeId);

                decimal? max = products.Select(p => p.NewPrice).Max();

                return max.Value;
            }
        }

        #endregion

        #endregion


    }
}