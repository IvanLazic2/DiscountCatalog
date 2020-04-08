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

        public IList<ManagerREST> FilterStores(IList<ManagerREST> managers)
        {
            List<ManagerREST> managerList = managers.ToList();

            foreach (var manager in managerList)
            {
                List<StoreREST> stores = manager.Stores.ToList();
                stores.RemoveAll(s => s.Deleted || !s.Approved);
                manager.Stores = stores;
            }

            return managerList;
        }

        public ManagerREST FilterStores(ManagerREST manager)
        {
            List<StoreREST> stores = manager.Stores.ToList();
            stores.RemoveAll(s => s.Deleted || !s.Approved);
            manager.Stores = stores;

            return manager;
        }

        public IList<ManagerREST> FilterStoreAdmin(IList<ManagerREST> managers)
        {
            List<ManagerREST> managerList = managers.ToList();

            foreach (var manager in managerList)
            {
                manager.Administrator.Managers = Enumerable.Empty<ManagerREST>();
                manager.Administrator.Stores = Enumerable.Empty<StoreREST>();
            }

            return managerList;
        }

        public ManagerREST FilterStoreAdmin(ManagerREST manager)
        {
            manager.Administrator.Managers = Enumerable.Empty<ManagerREST>();
            manager.Administrator.Stores = Enumerable.Empty<StoreREST>();

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

                IList<ManagerREST> mapped = mapper.Map<IList<ManagerREST>>(managers);

                //mapped = FilterStores(mapped);
                mapped = FilterStoreAdmin(mapped);
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

                IList<ManagerREST> mapped = mapper.Map<IList<ManagerREST>>(managers);

                mapped = FilterStores(mapped);
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

                var mapped = mapper.Map<ManagerREST>(manager);

                mapped = FilterStoreAdmin(mapped);
                mapped = FilterStores(mapped);

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

        //ASSIGN/UNASSIGN //ovo neradi vise zbog storeAdminId (terba bit storeAdminIdentityId)

        #region Assign/Unassign 

        public Result Assign(string storeAdminIdentityId, string managerId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.ManagerStores.Assign(storeAdminIdentityId, managerId, storeId);

                uow.Complete();

                return result;
            }
        }

        public Result Unassign(string storeAdminIdentityId, string managerId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.ManagerStores.Unassign(storeAdminIdentityId, managerId, storeId);

                uow.Complete();

                return result;
            }
        }

        #endregion 

        //MANAGER STORES

        #region ManagerStores

        public IPagingList<ManagerStore> GetManagerStores(string storeAdminIdentityId, string managerId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IList<ManagerStore> managerStores = uow.ManagerStores.GetManagerStores(managerId, sortOrder, searchString).ToList();

                IPagedList<ManagerStore> subset = managerStores.ToPagedList(pageIndex, pageSize);

                IPagingList<ManagerStore> result = new PagingList<ManagerStore>(subset, subset.GetMetaData());

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