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
using DiscountCatalog.WebAPI.REST.Store;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Implementation
{
    public class StoreRepository : Repository<StoreEntity>, IStoreRepository
    {
        #region Properties

        public ApplicationUserDbContext DbContext
        {
            get
            {
                return Context as ApplicationUserDbContext;
            }
        }

        #endregion

        #region Constructors

        public StoreRepository(ApplicationUserDbContext context)
                    : base(context)
        {
        }

        #endregion

        #region Methods

        //CREATE

        #region Create

        public async Task<Result> CreateAsync(StoreEntity store, string storeAdminId)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Store created."
            };

            if (store.StoreImage == null || !(store.StoreImage.Length > 0))
            {
                store.StoreImage = ImageProcessor.SetDefault("Store");
            }

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

        #endregion

        //GET ALL

        #region GetAllApproved

        public IEnumerable<StoreEntity> GetAllApproved()
        {
            return DbContext.Stores
                .Include(s => s.Administrator.Identity)
                //.Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Products)
                .Where(s => s.Approved && !s.Deleted)
                .ToList();
        }

        public IEnumerable<StoreEntity> GetAllApproved(string storeAdminIdentityId)
        {
            return DbContext.Stores
                .Include(s => s.Administrator.Identity)
                
                //.Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Products)
                .Where(s => s.Administrator.Identity.Id == storeAdminIdentityId && s.Approved && !s.Deleted)
                .ToList();
        }

        #endregion

        #region GetAllDeleted

        public IEnumerable<StoreEntity> GetAllDeleted()
        {
            return DbContext.Stores
                .Include(s => s.Administrator.Identity)
                .Where(s => s.Deleted)
                .ToList();
        }

        public IEnumerable<StoreEntity> GetAllDeleted(string storeAdminIdentityId)
        {
            return DbContext.Stores
                .Include(s => s.Administrator.Identity)
                .Where(s => s.Administrator.Identity.Id == storeAdminIdentityId && s.Deleted)
                .ToList();
        }

        #endregion

        #region GetAllLoaded

        public IEnumerable<StoreEntity> GetAllLoaded()
        {
            return DbContext.Stores
                .Include(s => s.Administrator.Identity)
                .Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Products)
                .ToList();
        }

        public IEnumerable<StoreEntity> GetAllLoaded(string storeAdminIdentityId)
        {
            return DbContext.Stores
                .Include(s => s.Administrator.Identity)
                .Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Products)
                .Where(s => s.Administrator.Identity.Id == storeAdminIdentityId)
                .ToList();
        }

        #endregion

        //GET

        #region GetApproved

        public StoreEntity GetApproved(string storeId)
        {
            return DbContext.Stores
                .Include(s => s.Administrator.Identity)
                .Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == storeId && s.Approved && !s.Deleted);
        }

        public StoreEntity GetApproved(string storeAdminIdentityId, string storeId)
        {
            return DbContext.Stores
                .Include(s => s.Administrator.Identity)
                .Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == storeId && s.Administrator.Identity.Id == storeAdminIdentityId && s.Approved && !s.Deleted);
        }

        #endregion

        #region GetLoaded

        public StoreEntity GetLoaded(string storeId)
        {
            return DbContext.Stores
                .Include(s => s.Administrator)
                .Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == storeId);
        }

        public StoreEntity GetLoaded(string storeAdminIdentityId, string storeId)
        {
            return DbContext.Stores
                .Include(s => s.Administrator)
                .Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == storeId && s.Administrator.Identity.Id == storeAdminIdentityId);
        }


        #endregion

        //UPDATE

        #region Update

        public async Task<Result> UpdateAsync(StoreRESTPut store)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Info updated."
            };

            var dbStore = GetApproved(store.Id);

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

        public async Task<Result> UpdateAsync(string storeAdminIdentityId, StoreRESTPut store)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Info updated."
            };

            var dbStore = GetApproved(storeAdminIdentityId, store.Id);

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
                if (store.StoreImage.Length > 0)
                {
                    if (ImageProcessor.IsValid(store.StoreImage))
                    {
                        dbStore.StoreImage = store.StoreImage;
                    }
                    else
                    {
                        modelState.Add("Image is not valid.");
                    }
                }

                var validator = new StoreValidator();
                var validationResult = await validator.ValidateAsync(dbStore);
                modelState.Add(validationResult);
            }
            else
            {
                modelState.Add("Store does not exist.");
            }

            return modelState.GetResult();
        }

        #endregion

        //DELETE

        #region MarkAsDeleted

        public Result MarkAsDeleted(string storeId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store deleted."
            };
            var store = GetApproved(storeId);
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

        public Result MarkAsDeleted(string storeAdminIdentityId, string storeId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store deleted."
            };
            var store = GetApproved(storeAdminIdentityId, storeId);
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

        #endregion

        //RESTORE

        #region MarkAsRestored

        public Result MarkAsRestored(string storeId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store restored."
            };

            var store = DbContext.Stores
                .FirstOrDefault(s => s.Id == storeId && s.Deleted);

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

        public Result MarkAsRestored(string storeAdminIdentityId, string storeId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Store restored."
            };

            var store = DbContext.Stores
                .Include(s => s.Administrator.Identity)
                .FirstOrDefault(s => s.Id == storeId && s.Administrator.Identity.Id == storeAdminIdentityId && s.Deleted);

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

        #endregion

        //GET AND POST IMAGE

        #region Get/Post Image

        public Result PostStoreImage(string id, byte[] image)
        {
            var result = new Result();

            if (ImageProcessor.IsValid(image))
            {
                StoreEntity store = GetApproved(id);

                if (store != null)
                {
                    store.StoreImage = image;
                }
                else
                {
                    result.Add("Store does not exist.");
                }
            }
            else
            {
                result.Add("Image is not valid.");
            }

            return result;
        }

        public byte[] GetStoreImage(string id)
        {
            StoreEntity store = GetLoaded(id);

            return store.StoreImage;
        }

        #endregion

        #endregion

    }
}