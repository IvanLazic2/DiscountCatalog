using AbatementHelper.WebAPI.Mapping;
using AutoMapper;
using DiscountCatalog.Common.Models;
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

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Implementation
{
    public class ManagerStoreRepository : Repository<ManagerStore>, IManagerStoreRepository
    {
        private IMapper mapper;

        public ApplicationUserDbContext DbContext
        {
            get
            {
                return Context as ApplicationUserDbContext;
            }
        }

        public ManagerStoreRepository(ApplicationUserDbContext context)
            : base(context)
        {
            mapper = AutoMapping.Initialise();
        }

        private List<ManagerStore> Search(IEnumerable<ManagerStore> managerStores, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                managerStores = managerStores.Where(u => u.Store.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return managerStores.ToList();
        }

        private List<ManagerStore> Order(IEnumerable<ManagerStore> managerStores, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    managerStores = managerStores.OrderByDescending(u => u.Store.StoreName).ToList();
                    break;
                default:
                    managerStores = managerStores.OrderBy(u => u.Store.StoreName).ToList();
                    break;
            }

            return managerStores.ToList();
        }

        public IEnumerable<ManagerStore> GetManagerStores(string id, string sortOrder, string searchString)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                List<ManagerStore> managerStores = new List<ManagerStore>();

                ManagerEntity manager = uow.Managers.GetApproved(id);
                List<StoreEntity> stores = uow.Stores.GetAllApproved().ToList();

                foreach (var store in stores)
                {
                    bool assigned;

                    if (manager.Administrator.Id == store.Administrator.Id)
                    {
                        StoreEntity tempStore = manager.Stores.FirstOrDefault(s => s.Id == store.Id && s.Approved && !s.Deleted);

                        if (tempStore != null)
                        {
                            assigned = true;
                        }
                        else
                        {
                            assigned = false;
                        }

                        managerStores.Add
                        (
                            new ManagerStore
                            {
                                Manager = mapper.Map<Manager>(manager),
                                Store = mapper.Map<Store>(store),
                                Assigned = assigned
                            }
                        );
                    }
                }

                managerStores = Search(managerStores, searchString);
                managerStores = Order(managerStores, sortOrder);

                return managerStores;
            }
        }

        public Result Assign(string storeAdminId, string managerId, string storeId)
        {

            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store assigned"
            };

            ManagerEntity manager = DbContext.Managers
                .Include(m => m.Stores)
                .Include(m => m.Administrator.Identity)
                .FirstOrDefault(m => m.Id == managerId && m.Administrator.Identity.Id == storeAdminId && m.Identity.Approved && !m.Identity.Deleted);

            StoreEntity store = DbContext.Stores
                .Include(s => s.Administrator.Identity)
                .FirstOrDefault(s => s.Id == storeId && s.Administrator.Identity.Id == storeAdminId && s.Approved && !s.Deleted);

            ManagerStoreValidator validator = new ManagerStoreValidator();
            ValidationResult validationResult = validator.Validate(new ManagerStoreModel(manager, store));

            if (validationResult.IsValid)
            {
                manager.Stores.Add(store);
            }
            else
            {
                modelState.Add(validationResult);
            }

            return modelState.GetResult();
        }

        public Result Unassign(string storeAdminId, string managerId, string storeId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store unassigned"
            };

            ManagerEntity manager = DbContext.Managers
                .Include(m => m.Stores)
                .Include(m => m.Administrator.Identity)
                .FirstOrDefault(m => m.Id == managerId && m.Administrator.Identity.Id == storeAdminId && m.Identity.Approved && !m.Identity.Deleted);

            StoreEntity store = DbContext.Stores
                .Include(s => s.Administrator.Identity)
                .FirstOrDefault(s => s.Id == storeId && s.Administrator.Identity.Id == storeAdminId && s.Approved && !s.Deleted);

            ManagerStoreValidator validator = new ManagerStoreValidator();
            ValidationResult validationResult = validator.Validate(new ManagerStoreModel(manager, store));

            if (validationResult.IsValid)
            {
                manager.Stores.Remove(store);
            }
            else
            {
                modelState.Add(validationResult);
            }

            return modelState.GetResult();

        }
    }
}