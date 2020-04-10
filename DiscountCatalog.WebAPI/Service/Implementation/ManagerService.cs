using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

namespace DiscountCatalog.WebAPI.Service.Implementation
{
    public class ManagerService : IService<ManagerREST>, IManagerService
    {
        #region Properties

        readonly IMapper mapper;

        #endregion

        #region Constructors

        public ManagerService()
        {
            mapper = AutoMapping.Initialise();
        }

        #endregion

        #region Methods

        //SEARCH/ORDER/FILTER

        #region Search/Order/Filter

        public IList<ManagerREST> Search(IList<ManagerREST> managers, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                managers = managers.Where(m => m.Identity.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return managers.ToList();
        }

        public IList<ManagerREST> Order(IList<ManagerREST> managers, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    managers = managers.OrderByDescending(m => m.Identity.UserName).ToList();
                    break;
                default:
                    managers = managers.OrderBy(m => m.Identity.UserName).ToList();
                    break;
            }

            return managers.ToList();
        }

        public IList<ManagerEntity> FilterStores(IList<ManagerEntity> managers, bool clear)
        {
            if (clear)
            {
                foreach (var manager in managers)
                {
                    if (manager.Stores != null)
                    {
                        manager.Stores.Clear();
                    }
                }
            }
            else
            {
                foreach (var manager in managers)
                {
                    List<StoreEntity> stores = manager.Stores.ToList();
                    stores.RemoveAll(s => s.Deleted || !s.Approved);
                    manager.Stores = stores;
                }
            }

            return managers;
        }

        public ManagerEntity FilterStores(ManagerEntity manager, bool clear)
        {
            if (clear)
            {
                if (manager.Stores != null)
                {
                    manager.Stores.Clear();
                }
            }
            else
            {
                List<StoreEntity> stores = manager.Stores.ToList();
                stores.RemoveAll(s => s.Deleted || !s.Approved);
                manager.Stores = stores;
            }

            return manager;
        }

        public IList<ManagerEntity> FilterStoreAdmin(IList<ManagerEntity> managers)
        {
            List<ManagerEntity> managerList = managers.ToList();

            foreach (var manager in managerList)
            {
                if (manager.Administrator.Managers != null)
                {
                    manager.Administrator.Managers.Clear();
                }
                if (manager.Administrator.Stores != null)
                {
                    manager.Administrator.Stores.Clear();
                }
            }

            return managerList;
        }

        public ManagerEntity FilterStoreAdmin(ManagerEntity manager)
        {
            if (manager.Administrator.Managers != null)
            {
                manager.Administrator.Managers.Clear();
            }
            if (manager.Administrator.Stores != null)
            {
                manager.Administrator.Stores.Clear();
            }

            return manager;
        }

        #endregion

        //CREATE

        #region Create

        public async Task<Result> CreateAsync(ManagerRESTPost manager)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity managerEntity = mapper.Map<ManagerEntity>(manager);

                managerEntity.Identity.Approved = true;
                managerEntity.Identity.Deleted = false;

                Result result = await uow.Managers.CreateAsync(manager.StoreAdminId, managerEntity, manager.Identity.Password);

                if (result.Success)
                {
                    uow.Complete();
                }

                return result;
            }
        }

        #endregion

        //GET ALl

        #region GetAll

        public IPagingList<ManagerREST> GetAll(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IEnumerable<ManagerEntity> managers = uow.Managers.GetAllApproved(storeAdminIdentityId);

                managers.ToList().ForEach(m => m.Identity.UserImage = ImageProcessor.CreateThumbnail(m.Identity.UserImage));

                managers = FilterStores(managers.ToList(), true);
                managers = FilterStoreAdmin(managers.ToList());

                IList<ManagerREST> mapped = mapper.Map<IList<ManagerREST>>(managers);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<ManagerREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<ManagerREST> result = new PagingList<ManagerREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        public IPagingList<ManagerREST> GetAllDeleted(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IEnumerable<ManagerEntity> managers = uow.Managers.GetAllDeleted(storeAdminIdentityId);

                managers.ToList().ForEach(m => m.Identity.UserImage = ImageProcessor.CreateThumbnail(m.Identity.UserImage));

                managers = FilterStores(managers.ToList(), true);
                managers = FilterStoreAdmin(managers.ToList());

                IList<ManagerREST> mapped = mapper.Map<IList<ManagerREST>>(managers);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<ManagerREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<ManagerREST> result = new PagingList<ManagerREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        #endregion

        //GET

        #region Get

        public ManagerREST Get(string storeAdminIdentityId, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity manager = uow.Managers.GetApproved(storeAdminIdentityId, managerId);

                manager.Identity.UserImage = ImageProcessor.CreateThumbnail(manager.Identity.UserImage);
                manager.Stores.ToList().ForEach(s => s.StoreImage = ImageProcessor.CreateThumbnail(s.StoreImage));

                manager = FilterStores(manager, false);
                manager = FilterStoreAdmin(manager);

                var mapped = mapper.Map<ManagerREST>(manager);

                return mapped;
            }
        }

        #endregion

        //UPDATE

        #region Update

        public async Task<Result> UpdateAsync(string storeAdminIdentityId, ManagerRESTPut manager)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = await uow.Managers.UpdateAsync(storeAdminIdentityId, manager);

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

        public Result Delete(string storeAdminIdentityId, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Managers.MarkAsDeleted(storeAdminIdentityId, managerId);

                uow.Complete();

                return result;
            }
        }

        public Result Restore(string storeAdminIdentityId, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Managers.MarkAsRestored(storeAdminIdentityId, managerId);

                uow.Complete();

                return result;
            }
        }

        #endregion

        //GET/POST IMAGE

        #region Get/Post Image

        public async Task<Result> PostImageAsync(string storeAdminIdentityId, string managerId, byte[] image)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity manager = uow.Managers.GetApproved(managerId);

                Result result = await uow.Accounts.PostUserImage(manager.Identity.Id, image);

                uow.Complete();

                return result;
            }
        }

        public async Task<byte[]> GetImageAsync(string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity manager = uow.Managers.GetLoaded(managerId);

                byte[] image = await uow.Accounts.GetUserImage(manager.Identity.Id);

                return ImageProcessor.CreateThumbnail(image);
            }
        }



        #endregion

        #endregion

    }
}