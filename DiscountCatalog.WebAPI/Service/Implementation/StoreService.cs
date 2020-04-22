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

        public IList<StoreEntity> FilterManagers(IList<StoreEntity> stores, bool clear)
        {
            if (clear)
            {
                foreach (var store in stores)
                {
                    if (store.Managers != null)
                    {
                        store.Managers.Clear();
                    }
                }
            }
            else
            {
                foreach (var store in stores)
                {
                    List<ManagerEntity> managers = store.Managers.ToList();
                    managers.RemoveAll(m => m.Identity.Deleted || !m.Identity.Approved);
                    store.Managers = managers;
                }
            }

            return stores;
        }

        public StoreEntity FilterManagers(StoreEntity store, bool clear)
        {
            if (clear)
            {
                if (store.Managers != null)
                {
                    store.Managers.Clear();
                }
            }
            else
            {
                List<ManagerEntity> managers = store.Managers.ToList();
                managers.RemoveAll(m => m.Identity.Deleted || !m.Identity.Approved);
                store.Managers = managers;
            }

            return store;
        }

        public IList<StoreEntity> FilterStoreAdmin(IList<StoreEntity> stores)
        {
            foreach (var store in stores)
            {
                if (store.Administrator.Stores != null)
                {
                    store.Administrator.Stores.Clear();
                }
                if (store.Administrator.Managers != null)
                {
                    store.Administrator.Managers.Clear();
                }
            }

            return stores;
        }

        public StoreEntity FilterStoreAdmin(StoreEntity store)
        {
            if (store.Administrator.Stores != null)
            {
                store.Administrator.Stores.Clear();
            }
            if (store.Administrator.Managers != null)
            {
                store.Administrator.Managers.Clear();
            }

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

                if (result.Success)
                {
                    uow.Complete();
                }

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

                //stores.ToList().ForEach(s => s.StoreImage = ImageProcessor.CreateThumbnail(s.StoreImage));

                stores = FilterManagers(stores.ToList(), true);
                stores = FilterStoreAdmin(stores.ToList());

                IList<StoreREST> mapped = mapper.Map<IList<StoreREST>>(stores);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<StoreREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<StoreREST> result = new PagingList<StoreREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        public IPagingList<StoreREST> GetAll(string managerIdentityId, string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ApplicationUser identitiy = uow.Accounts.GetApproved(managerIdentityId);
                ManagerEntity manager = uow.Managers.GetAllLoaded().FirstOrDefault(m => m.Identity.Id == identitiy.Id);
                storeAdminIdentityId = manager.Administrator.Identity.Id;

                IEnumerable<StoreEntity> stores = manager.Stores
                    .Where(s => s.Administrator.Identity.Id == storeAdminIdentityId && s.Approved && !s.Deleted);

                //stores.ToList().ForEach(s => s.StoreImage = ImageProcessor.CreateThumbnail(s.StoreImage));

                stores = FilterManagers(stores.ToList(), true);
                stores = FilterStoreAdmin(stores.ToList());

                var mapped = mapper.Map<IList<StoreREST>>(stores);

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

                //stores.ToList().ForEach(s => s.StoreImage = ImageProcessor.CreateThumbnail(s.StoreImage));

                stores = FilterManagers(stores.ToList(), true);
                stores = FilterStoreAdmin(stores.ToList());

                IList<StoreREST> mapped = mapper.Map<IList<StoreREST>>(stores);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<StoreREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<StoreREST> result = new PagingList<StoreREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        //public IPagingList<StoreREST> GetAllDeleted(string managerIdentityId, string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        //{
        //    using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //    {
        //        ManagerEntity manager = uow.Managers.GetApproved(managerIdentityId);

        //        IEnumerable<StoreEntity> stores = manager.Stores
        //            .Where(s => s.Administrator.Identity.Id == storeAdminIdentityId && s.Deleted);

        //        stores.ToList().ForEach(s => s.StoreImage = ImageProcessor.CreateThumbnail(s.StoreImage));

        //        stores = FilterManagers(stores.ToList(), true);
        //        stores = FilterStoreAdmin(stores.ToList());

        //        IList<StoreREST> mapped = mapper.Map<IList<StoreREST>>(stores);

        //        mapped = Search(mapped, searchString);
        //        mapped = Order(mapped, sortOrder);

        //        IPagedList<StoreREST> subset = mapped.ToPagedList(pageIndex, pageSize);

        //        IPagingList<StoreREST> result = new PagingList<StoreREST>(subset, subset.GetMetaData());

        //        return result;
        //    }
        //}

        #endregion

        //GET

        #region Get

        public StoreREST Get(string storeAdminIdentityId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                StoreEntity store = uow.Stores.GetApproved(storeAdminIdentityId, storeId);

                //store.StoreImage = ImageProcessor.CreateThumbnail(store.StoreImage);
                //store.Managers.ToList().ForEach(m => m.Identity.UserImage = ImageProcessor.CreateThumbnail(m.Identity.UserImage));

                store = FilterManagers(store, false);
                store = FilterStoreAdmin(store);

                var mapped = mapper.Map<StoreREST>(store);

                return mapped;
            }
        }

        public StoreREST Get(string managerIdentityId, string storeAdminIdentityId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ApplicationUser identitiy = uow.Accounts.GetApproved(managerIdentityId);
                ManagerEntity manager = uow.Managers.GetAllLoaded().FirstOrDefault(m => m.Identity.Id == identitiy.Id);
                storeAdminIdentityId = manager.Administrator.Identity.Id;

                StoreEntity store = manager.Stores.FirstOrDefault(s => s.Id == storeId && s.Administrator.Identity.Id == storeAdminIdentityId && s.Approved && !s.Deleted);

                store = FilterManagers(store, true);
                store = FilterStoreAdmin(store);

                //store.StoreImage = ImageProcessor.CreateThumbnail(store.StoreImage);

                var mapped = mapper.Map<StoreREST>(store);

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

                return result;
            }
        }

        public async Task<Result> UpdateAsync(string managerIdentityId, string storeAdminIdentityId, StoreRESTPut store)
        {
            Result result = new Result();

            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ApplicationUser identitiy = uow.Accounts.GetApproved(managerIdentityId);
                ManagerEntity manager = uow.Managers.GetAllLoaded().FirstOrDefault(m => m.Identity.Id == identitiy.Id);
                storeAdminIdentityId = manager.Administrator.Identity.Id;

                StoreEntity dbStore = manager.Stores
                    .FirstOrDefault(s => s.Administrator.Identity.Id == storeAdminIdentityId && s.Approved && !s.Deleted);

                result = await uow.Stores.UpdateAsync(storeAdminIdentityId, store);

                if (result.Success)
                {
                    uow.Complete();
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

        public Result PostImage(string managerIdentityId, string storeAdminIdentityId, string storeId, byte[] image)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.PostStoreImage(storeId, image);

                uow.Complete();

                return result;
            }
        }

        //public byte[] GetImage(string storeId)
        //{
        //    using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //    {
        //        byte[] image = uow.Stores.GetStoreImage(storeId);

        //        return ImageProcessor.CreateThumbnail(image);
        //    }
        //}



        #endregion

        #endregion

    }
}