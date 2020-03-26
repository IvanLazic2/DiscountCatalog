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
using DiscountCatalog.WebAPI.Service.Contractor;
using PagedList;

namespace DiscountCatalog.WebAPI.Service.Implementation
{
    public class ManagerService : IService<ManagerREST>, IManagerService
    {
        readonly IMapper mapper;

        public ManagerService()
        {
            mapper = AutoMapping.Initialise();
        }

        #region Methods

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

        public Result Assign(string storeAdminId, string managerId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.ManagerStores.Assign(storeAdminId, managerId, storeId);

                uow.Complete();

                return result;
            }
        }

        public async Task<Result> CreateAsync(ManagerRESTPost manager)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity managerEntity = mapper.Map<ManagerEntity>(manager);

                managerEntity.Identity.Approved = true;
                managerEntity.Identity.Deleted = false;

                Result result = await uow.Managers.CreateAsync(manager.StoreAdminId, managerEntity, manager.Password);

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

        public Result Delete(string storeAdminId, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Managers.MarkAsDeleted(managerId);

                uow.Complete();

                return result;
            }
        }

        public IPagingList<ManagerREST> GetAll(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmin = uow.StoreAdmins.GetLoadedByIdentityId(storeAdminId);

                IEnumerable<ManagerEntity> managers = storeAdmin.Managers
                    .Where(m => m.Identity.Approved && !m.Identity.Deleted)
                    .ToList();

                IList<ManagerREST> mapped = mapper.Map<IList<ManagerREST>>(managers);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<ManagerREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<ManagerREST> result = new PagingList<ManagerREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        public IPagingList<ManagerREST> GetAllDeleted(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize) //ovo neradi!!!!!!!!!!!!!
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmin = uow.StoreAdmins.GetLoadedByIdentityId(storeAdminId);

                IEnumerable<ManagerEntity> managers = storeAdmin.Managers
                    .Where(m => m.Identity.Deleted)
                    .ToList();

                IList<ManagerREST> mapped = mapper.Map<IList<ManagerREST>>(managers);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<ManagerREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<ManagerREST> result = new PagingList<ManagerREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        public async Task<byte[]> GetImageAsync(string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity manager = uow.Managers.GetApproved(managerId);

                byte[] image = await uow.Accounts.GetUserImage(manager.Identity.Id);

                return ImageProcessor.CreateThumbnail(image);
            }
        }

        public IPagingList<ManagerStore> GetManagerStores(string storeAdminId, string managerId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IList<ManagerStore> managerStores = uow.ManagerStores.GetManagerStores(managerId, sortOrder, searchString).ToList();

                IPagedList<ManagerStore> subset = managerStores.ToPagedList(pageIndex, pageSize);

                IPagingList<ManagerStore> result = new PagingList<ManagerStore>(subset, subset.GetMetaData());

                return result;
            }
        }

        public async Task<Result> PostImageAsync(string storeAdminId, string managerId, byte[] image)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity manager = uow.Managers.GetApproved(managerId);

                Result result = await uow.Accounts.PostUserImage(manager.Identity.Id, image);

                uow.Complete();

                return result; // isprobat dal postimage radi!!!!!!!!!!!
            }
        }

        public Result Restore(string storeAdminId, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Managers.MarkAsRestored(managerId);

                uow.Complete();

                return result;
            }
        }

        public Result Unassign(string storeAdminId, string managerId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.ManagerStores.Unassign(storeAdminId, managerId, storeId);

                uow.Complete();

                return result;
            }
        }

        public async Task<Result> UpdateAsync(string storeAdminId, ManagerRESTPut manager)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity managerEntity = uow.Managers.GetApproved(manager.Id);

                managerEntity.Identity.FirstName = manager.Identity.FirstName;
                managerEntity.Identity.LastName = manager.Identity.LastName;
                managerEntity.Identity.UserName = manager.Identity.UserName;
                managerEntity.Identity.Email = manager.Identity.Email;
                managerEntity.Identity.PhoneNumber = manager.Identity.PhoneNumber;
                managerEntity.Identity.Country = manager.Identity.Country;
                managerEntity.Identity.City = manager.Identity.City;
                managerEntity.Identity.PostalCode = manager.Identity.PostalCode;
                managerEntity.Identity.Street = manager.Identity.Street;

                Result result = await uow.Managers.UpdateAsync(managerEntity);

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

        public ManagerREST Get(string storeAdminId, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmin = uow.StoreAdmins.GetLoadedByIdentityId(storeAdminId);

                ManagerEntity manager = storeAdmin.Managers
                    .FirstOrDefault(m => m.Id == managerId);

                var mapped = mapper.Map<ManagerREST>(manager);

                return mapped;
            }
        }

        #endregion
    }
}