using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DiscountCatalog.Common.Models;
using System.Threading.Tasks;
using DiscountCatalog.WebAPI.ModelState;
using DiscountCatalog.WebAPI.Validation.Validators;
using DiscountCatalog.WebAPI.Extensions;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Implementation
{
    public class StoreRepository : Repository<StoreEntity>, IStoreRepository
    {
        public ApplicationUserDbContext DbContext
        {
            get
            {
                return Context as ApplicationUserDbContext;
            }
        }

        public StoreRepository(ApplicationUserDbContext context)
            : base(context)
        {
        }

        private List<StoreEntity> Search(IEnumerable<StoreEntity> stores, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                stores = stores.Where(s => s.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return stores.ToList();
        }

        private List<StoreEntity> Order(IEnumerable<StoreEntity> store, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    store = store.OrderByDescending(s => s.StoreName).ToList();
                    break;
                default:
                    store = store.OrderBy(s => s.StoreName).ToList();
                    break;
            }

            return store.ToList();
        }

        public async Task<Result> CreateAsync(StoreEntity store, string storeAdminId)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Store created."
            };
            UnitOfWork uow = new UnitOfWork(DbContext);
            store.Administrator = uow.StoreAdmins.GetByIdentityId(storeAdminId);
            var validator = new StoreValidator();
            var validationResult = await validator.ValidateAsync(store);
            modelState.Add(validationResult);
            if (validationResult.IsValid)
            {
                Add(store);
            }
            return modelState.GetResult();
        }

        public IEnumerable<StoreEntity> GetAllApproved(string sortOrder, string searchString)
        {
            var stores = DbContext.Stores
                .Include(s => s.Administrator)
                .Include(s => s.Managers)
                .Include(s => s.Products)
                .Where(s => s.Approved && !s.Deleted)
                .ToList();

            stores = Search(stores, searchString);
            stores = Order(stores, sortOrder);

            return stores;
        }

        public IEnumerable<StoreEntity> GetAllLoaded(string sortOrder, string searchString)
        {
            var stores = DbContext.Stores
                .Include(s => s.Administrator)
                .Include(s => s.Managers)
                .Include(s => s.Products)
                .ToList();

            stores = Search(stores, searchString);
            stores = Order(stores, sortOrder);

            return stores;
        }

        public StoreEntity GetLoaded(string id)
        {
            return DbContext.Stores
                .Include(s => s.Administrator)
                .Include(s => s.Managers)
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<StoreEntity> GetAllDeleted(string sortOrder, string searchString)
        {
            var stores = DbContext.Stores
                .Include(s => s.Administrator)
                .Where(s => s.Deleted)
                .ToList();

            stores = Search(stores, searchString);
            stores = Order(stores, sortOrder);

            return stores;
        }

        public async Task<Result> UpdateAsync(StoreEntity store)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Info updated."
            };

            var dbStore = DbContext.Stores
                .Include(s => s.Administrator)
                .FirstOrDefault(s => s.Id == store.Id);
            if (dbStore != null)
            {
                dbStore.StoreName = store.StoreName;
                dbStore.WorkingHoursWeekBegin = store.WorkingHoursWeekBegin;
                dbStore.WorkingHoursWeekEnd = store.WorkingHoursWeekEnd;
                dbStore.WorkingHoursWeekendsBegin = store.WorkingHoursWeekendsBegin;
                dbStore.WorkingHoursWeekendsEnd = store.WorkingHoursWeekendsEnd;
                dbStore.WorkingHoursHolidaysBegin = store.WorkingHoursHolidaysBegin;
                dbStore.WorkingHoursHolidaysEnd = store.WorkingHoursHolidaysEnd;
                dbStore.Country = store.Country;
                dbStore.City = store.City;
                dbStore.PostalCode = store.PostalCode;
                dbStore.Street = store.Street;

                var validator = new StoreValidator();
                var validationResult = await validator.ValidateAsync(dbStore);
                modelState.Add(validationResult);
            }
            return modelState.GetResult();
        }

        public Result MarkAsDeleted(string id)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store deleted."
            };
            var store = Get(id);
            if (store != null)
            {
                store.Deleted = true;
            }
            else
            {
                modelState.Add("Store does not exist.");
            }
            return modelState.GetResult();
        }

        public Result MarkAsRestored(string id)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store restored."
            };
            var store = Get(id);
            if (store != null)
            {
                store.Deleted = false;
            }
            else
            {
                modelState.Add("Store does not exist.");
            }
            return modelState.GetResult();
        }

        public StoreEntity GetApproved(string id)
        {
            return DbContext.Stores
                .Include(s => s.Administrator)
                .Include(s => s.Managers)
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == id && s.Approved && !s.Deleted);
        }

        //public IEnumerable<StoreEntity> GetAllLoadedByStoreAdminId(string id, string sortOrder, string searchString)
        //{
        //    var stores = DbContext.Stores
        //        .Include(s => s.Managers)
        //        .Include(s => s.Administrator.Identity)
        //        .Where(s => s.Administrator.Id == id)
        //        .ToList();

        //    stores = Search(stores, searchString);
        //    stores = Order(stores, sortOrder);

        //    return stores;
        //}

        //public IEnumerable<StoreEntity> GetAllApprovedByStoreAdminId(string id, string sortOrder, string searchString)
        //{
        //    var stores = DbContext.Stores
        //        .Include(s => s.Managers)
        //        .Include(s => s.Administrator.Identity)
        //        .Where(s => s.Administrator.Id == id && s.Approved && !s.Deleted)
        //        .ToList();

        //    stores = Search(stores, searchString);
        //    stores = Order(stores, sortOrder);

        //    return stores;
        //}

        //public StoreEntity GetLoadedByStoreAdminId(string id, string storeId)
        //{
        //    return DbContext.Stores
        //        .Include(s => s.Managers)
        //        .Include(s => s.Administrator.Identity)
        //        .FirstOrDefault(s => s.Administrator.Id == id && s.Id == storeId);
        //}

        //public StoreEntity GetApprovedByStoreAdminId(string id, string storeId)
        //{
        //    return DbContext.Stores
        //        .Include(s => s.Managers)
        //        .Include(s => s.Administrator.Identity)
        //        .FirstOrDefault(s => s.Administrator.Id == id && s.Id == storeId && s.Approved && !s.Deleted);
        //}

        public Result PostStoreImage(string id, byte[] image)
        {
            var result = new Result();

            if (ImageProcessor.IsValid(image))
            {
                StoreEntity store = GetApproved(id);

                store.StoreImage = image;
            }
            else
            {
                result.AddModelError(string.Empty, "Image is not valid.");
            }

            return result;
        }

        public byte[] GetStoreImage(string id)
        {
            StoreEntity store = GetApproved(id);

            return store.StoreImage;
        }
    }
}