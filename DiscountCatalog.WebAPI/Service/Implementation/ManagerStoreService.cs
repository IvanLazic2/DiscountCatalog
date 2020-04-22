using AbatementHelper.WebAPI.Mapping;
using AutoMapper;
using DiscountCatalog.WebAPI.Extensions;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.ModelState;
using DiscountCatalog.WebAPI.Validation.Validators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor;
using DiscountCatalog.WebAPI.Models.ManyToManyModels.Manager;
using DiscountCatalog.WebAPI.Service.Contractor;
using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.REST.Manager;
using PagedList;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.Paging.Implementation;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.Processors;

namespace DiscountCatalog.WebAPI.Service.Implementation
{
    public class ManagerStoreService : IService<ManagerStore>, IManagerStoreService
    {
        private readonly IMapper mapper;
        //private readonly IStoreService storeService;
        //private readonly IManagerService managerService;

        public ManagerStoreService()
        {
            mapper = AutoMapping.Initialise();
            //storeService = new StoreService();
            //managerService = new ManagerService();
        }

        public IList<ManagerStore> Search(IList<ManagerStore> managerStores, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                managerStores = managerStores.Where(m => m.Store.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return managerStores.ToList();
        }

        public IList<ManagerStore> Order(IList<ManagerStore> managerStores, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    managerStores = managerStores.OrderByDescending(m => m.Store.StoreName).ToList();
                    break;
                default:
                    managerStores = managerStores.OrderBy(m => m.Store.StoreName).ToList();
                    break;
            }

            return managerStores.ToList();
        }

        public ManagerEntity FilterStores(ManagerEntity manager)
        {
            if (manager.Stores != null)
            {
                manager.Stores.Clear();
            }

            return manager;
        }

        public IList<StoreEntity> FilterManagers(IList<StoreEntity> stores)
        {
            foreach (var store in stores)
            {
                if (store.Managers != null)
                {
                    store.Managers.Clear();
                }
            }

            return stores;
        }

        public ManagerEntity FilterStoreAdmin(ManagerEntity manager)
        {
            if (manager.Administrator != null)
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

            return manager;
        }

        public IList<StoreEntity> FilterStoreAdmin(IList<StoreEntity> stores)
        {
            foreach (var store in stores)
            {
                if (store.Administrator != null)
                {
                    if (store.Administrator.Managers != null)
                    {
                        store.Administrator.Managers.Clear();
                    }
                    if (store.Administrator.Stores != null)
                    {
                        store.Administrator.Stores.Clear();
                    }
                }
            }

            return stores;
        }

        public ManagerStores GetManagerStores(string storeAdminIdentityId, string managerId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IList<ManagerStore> managerStores = new List<ManagerStore>();

                ManagerEntity manager = uow.Managers.GetApproved(storeAdminIdentityId, managerId);

                //manager.Identity.UserImage = ImageProcessor.CreateThumbnail(manager.Identity.UserImage);

                if (manager != null)
                {
                    IList<StoreEntity> stores = uow.Stores.GetAllApproved(storeAdminIdentityId).ToList();

                    stores = FilterManagers(stores);
                    stores = FilterStoreAdmin(stores);

                    foreach (var store in stores)
                    {
                        //store.StoreImage = ImageProcessor.CreateThumbnail(store.StoreImage);

                        bool assigned = manager.Stores.Any(s => s.Id == store.Id);

                        managerStores.Add
                        (
                            new ManagerStore
                            {
                                Store = mapper.Map<StoreREST>(store),
                                Assigned = assigned
                            }
                        );
                    }

                    manager = FilterStores(manager);
                    manager = FilterStoreAdmin(manager);

                    managerStores = Search(managerStores, searchString);
                    managerStores = Order(managerStores, sortOrder);

                    IPagedList<ManagerStore> subset = managerStores.ToPagedList(pageIndex, pageSize);

                    IPagingList<ManagerStore> result = new PagingList<ManagerStore>(subset, subset.GetMetaData());

                    return new ManagerStores
                    {
                        Manager = mapper.Map<ManagerREST>(manager),
                        Stores = result
                    };
                }
                else
                {
                    return null;
                }
            }
        }


        public Result Assign(string storeAdminIdentityId, string managerId, string storeId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store assigned."
            };

            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity manager = uow.Managers.GetApproved(storeAdminIdentityId, managerId);
                StoreEntity store = uow.Stores.GetApproved(storeAdminIdentityId, storeId);

                ManagerStoreValidator validator = new ManagerStoreValidator();
                ValidationResult validationResult = validator.Validate(
                    new ManagerStoreModel(manager, store));

                if (validationResult.IsValid)
                {
                    manager.Stores.Add(store);
                    uow.Complete();
                }
                else
                {
                    modelState.Add(validationResult);
                }

                return modelState.GetResult();
            }
        }

        public Result Unassign(string storeAdminIdentityId, string managerId, string storeId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store unassigned."
            };

            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ManagerEntity manager = uow.Managers.GetApproved(storeAdminIdentityId, managerId);
                StoreEntity store = uow.Stores.GetApproved(storeAdminIdentityId, storeId);

                ManagerStoreValidator validator = new ManagerStoreValidator();
                ValidationResult validationResult = validator.Validate(
                    new ManagerStoreModel(manager, store));

                if (validationResult.IsValid)
                {
                    manager.Stores.Remove(store);
                    uow.Complete();
                }
                else
                {
                    modelState.Add(validationResult);
                }

                return modelState.GetResult();
            }
        }
    }
}