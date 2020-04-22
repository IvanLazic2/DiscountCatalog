using AbatementHelper.WebAPI.Mapping;
using AutoMapper;
using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Extensions;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Models.ManyToManyModels.Store;
using DiscountCatalog.WebAPI.ModelState;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.Paging.Implementation;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.REST.Manager;
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.Service.Contractor;
using DiscountCatalog.WebAPI.Validation.Validators;
using FluentValidation.Results;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Service.Implementation
{
    public class StoreManagerService : IService<StoreManager>, IStoreManagerService
    {
        private readonly IMapper mapper;

        public StoreManagerService()
        {
            mapper = AutoMapping.Initialise();
        }

        public IList<StoreManager> Search(IList<StoreManager> storeManagers, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                storeManagers = storeManagers.Where(s => s.Manager.Identity.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return storeManagers.ToList();
        }

        public IList<StoreManager> Order(IList<StoreManager> storeManagers, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    storeManagers = storeManagers.OrderByDescending(s => s.Manager.Identity.UserName).ToList();
                    break;
                default:
                    storeManagers = storeManagers.OrderBy(s => s.Manager.Identity.UserName).ToList();
                    break;
            }

            return storeManagers.ToList();
        }

        public StoreEntity FilterManagers(StoreEntity store)
        {
            if (store.Managers != null)
            {
                store.Managers.Clear();
            }

            return store;
        }

        public IList<ManagerEntity> FilterStores(IList<ManagerEntity> managers)
        {
            foreach (var manager in managers)
            {
                if (manager.Stores != null)
                {
                    manager.Stores.Clear();
                }
            }
            
            return managers;
        }

        public StoreEntity FilterStoreAdmin(StoreEntity store)
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

            return store;
        }

        public IList<ManagerEntity> FilterStoreAdmin(IList<ManagerEntity> managers)
        {
            foreach (var manager in managers)
            {
                if (manager.Administrator != null)
                {
                    if (manager.Administrator.Managers != null)
                    {
                        manager.Administrator.Managers.Clear();
                    }
                    if (manager.Administrator.Stores != null)
                    {
                        manager.Administrator.Managers.Clear();
                    }
                }
            }

            return managers;
        }

        public StoreManagers GetStoreManagers(string storeAdminIdentityId, string storeId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                IList<StoreManager> storeManagers = new List<StoreManager>();

                StoreEntity store = uow.Stores.GetApproved(storeAdminIdentityId, storeId);
                //store.StoreImage = ImageProcessor.CreateThumbnail(store.StoreImage);
                
                if (store != null)
                {
                    IList<ManagerEntity> managers = uow.Managers.GetAll(storeAdminIdentityId).ToList();

                    managers = FilterStores(managers);
                    managers = FilterStoreAdmin(managers);

                    foreach (var manager in managers)
                    {
                        //manager.Identity.UserImage = ImageProcessor.CreateThumbnail(manager.Identity.UserImage);

                        bool assigned = store.Managers.Any(m => m.Identity.Id == manager.Identity.Id);

                        storeManagers.Add
                        (
                            new StoreManager
                            {
                                Manager = mapper.Map<ManagerREST>(manager),
                                Assigned = assigned
                            }
                        );
                    }

                    store = FilterManagers(store);
                    store = FilterStoreAdmin(store);

                    storeManagers = Search(storeManagers, searchString);
                    storeManagers = Order(storeManagers, sortOrder);

                    IPagedList<StoreManager> subset = storeManagers.ToPagedList(pageIndex, pageSize);

                    IPagingList<StoreManager> result = new PagingList<StoreManager>(subset, subset.GetMetaData());

                    return new StoreManagers
                    {
                        Store = mapper.Map<StoreREST>(store),
                        Managers = result
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public Result Assign(string storeAdminIdentityId, string storeId, string managerId)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Manager assigned."
            };

            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                StoreEntity store = uow.Stores.GetApproved(storeAdminIdentityId, storeId);
                ManagerEntity manager = uow.Managers.GetApproved(storeAdminIdentityId, managerId);

                ManagerStoreValidator validator = new ManagerStoreValidator();
                ValidationResult validationResult = validator.Validate(
                    new ManagerStoreModel(manager, store));

                if (validationResult.IsValid)
                {
                    store.Managers.Add(manager);
                    uow.Complete();
                }
                else
                {
                    modelState.Add(validationResult);
                }

                return modelState.GetResult();
            }
        }

        public Result Unassign(string storeAdminIdentityId, string storeId, string managerId)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Manager unassigned."
            };

            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                StoreEntity store = uow.Stores.GetApproved(storeAdminIdentityId, storeId);
                ManagerEntity manager = uow.Managers.GetApproved(storeAdminIdentityId, managerId);

                ManagerStoreValidator validator = new ManagerStoreValidator();
                ValidationResult validationResult = validator.Validate(
                    new ManagerStoreModel(manager, store));

                if (validationResult.IsValid)
                {
                    store.Managers.Remove(manager);
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