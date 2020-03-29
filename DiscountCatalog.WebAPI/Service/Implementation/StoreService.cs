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
using DiscountCatalog.WebAPI.REST.Manager;
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.Service.Contractor;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.WebAPI.Service.Implementation
{
    public class StoreService : IService<StoreREST>, IStoreService
    {
        #region Properties

        readonly IMapper mapper;

        #endregion

        #region Constructors

        public StoreService()
        {
            mapper = AutoMapping.Initialise();
        }

        #endregion

        #region Methods

        //SEARCH/ORDER/FILTER

        #region Search/Order/Filter

        public IList<StoreREST> Search(IList<StoreREST> stores, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                stores = stores.Where(s => s.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return stores.ToList();
        }

        public IList<StoreREST> Order(IList<StoreREST> stores, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    stores = stores.OrderByDescending(s => s.StoreName).ToList();
                    break;
                default:
                    stores = stores.OrderBy(s => s.StoreName).ToList();
                    break;
            }

            return stores.ToList();
        }

        public IList<StoreREST> FilterManagers(IList<StoreREST> stores)
        {
            List<StoreREST> storeList = stores.ToList();

            foreach (var store in storeList)
            {
                List<ManagerREST> managers = store.Managers.ToList();
                managers.RemoveAll(m => m.Identity.Deleted || !m.Identity.Approved);
                store.Managers = managers;
            }

            return storeList;
        }

        public StoreREST FilterManagers(StoreREST store)
        {
            List<ManagerREST> managers = store.Managers.ToList();
            managers.RemoveAll(m => m.Identity.Deleted || !m.Identity.Approved);
            store.Managers = managers;

            return store;
        }

        #endregion

        //CREATE

        #region Create

        public async Task<Result> CreateAsync(StoreRESTPost store)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                StoreEntity storeEntity = mapper.Map<StoreEntity>(store);

                storeEntity.Approved = true;
                storeEntity.Deleted = false;

                Result result = await uow.Stores.CreateAsync(storeEntity, store.StoreAdminId);

                uow.Complete();

                return result;
            }
        }

        #endregion

        //GET ALL

        #region GetAll

        public IPagingList<StoreREST> GetAll(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IEnumerable<StoreEntity> stores = uow.Stores.GetAllApproved(storeAdminIdentityId);

                IList<StoreREST> mapped = mapper.Map<IList<StoreREST>>(stores);

                mapped = FilterManagers(mapped);
                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<StoreREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<StoreREST> result = new PagingList<StoreREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        public IPagingList<StoreREST> GetAllDeleted(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IEnumerable<StoreEntity> stores = uow.Stores.GetAllDeleted(storeAdminIdentityId);

                IList<StoreREST> mapped = mapper.Map<IList<StoreREST>>(stores);

                mapped = FilterManagers(mapped);
                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<StoreREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<StoreREST> result = new PagingList<StoreREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        #endregion

        //GET

        #region Get

        public StoreREST Get(string storeAdminIdentityId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                StoreEntity store = uow.Stores.GetApproved(storeAdminIdentityId, storeId);

                var mapped = mapper.Map<StoreREST>(store);

                mapped = FilterManagers(mapped);

                return mapped;
            }
        }

        #endregion

        //UPDATE

        #region Update

        public async Task<Result> UpdateAsync(string storeAdminIdentityId, StoreRESTPut store)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = await uow.Stores.UpdateAsync(storeAdminIdentityId, store);

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

        public Result Delete(string storeAdminIdentityId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.MarkAsDeleted(storeAdminIdentityId, storeId);

                uow.Complete();

                return result;
            }
        }

        public Result Restore(string storeAdminIdentityId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.MarkAsRestored(storeAdminIdentityId, storeId);

                uow.Complete();

                return result;
            }
        }

        #endregion

        //GET/POST IMAGE

        #region Get/Post Image

        public Result PostImage(string storeAdminIdentityId, string storeId, byte[] image)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.PostStoreImage(storeId, image);

                uow.Complete();

                return result;
            }
        }

        public byte[] GetImage(string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                byte[] image = uow.Stores.GetStoreImage(storeId);

                return ImageProcessor.CreateThumbnail(image);
            }
        }

        #endregion

        #endregion

    }
}