using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.ModelState;
using DiscountCatalog.WebAPI.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using DiscountCatalog.WebAPI.Extensions;
using DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Implementation
{
    public class ManagerRepository : Repository<ManagerEntity>, IManagerRepository
    {
        public ApplicationUserDbContext DbContext
        {
            get
            {
                return Context as ApplicationUserDbContext;
            }
        }

        public ManagerRepository(ApplicationUserDbContext context)
            : base(context)
        {
        }

        #region Methods

        public async Task<Result> CreateAsync(string storeAdminId, ManagerEntity manager, string password)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Manager created."
            };
            UnitOfWork uow = new UnitOfWork(DbContext);
            var identityResult = await uow.Accounts.CreateAsync(manager.Identity, "Manager", password);
            modelState.Add(identityResult);
            if (identityResult.Success)
            {
                manager.Administrator = uow.StoreAdmins.GetByIdentityId(storeAdminId);
                var validator = new ManagerValidator();
                var validationResult = await validator.ValidateAsync(manager);
                modelState.Add(validationResult);
                if (validationResult.IsValid)
                {
                    Add(manager);
                }
            }
            return modelState.GetResult();
        }

        public IEnumerable<ManagerEntity> GetAllApproved(string sortOrder, string searchString)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .Where(m => m.Identity.Approved && !m.Identity.Deleted)
                .ToList();
        }

        public IEnumerable<ManagerEntity> GetAllDeleted(string sortOrder, string searchString)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Where(m => m.Identity.Deleted)
                .ToList();
        }

        public IEnumerable<ManagerEntity> GetAllLoaded(string sortOrder, string searchString)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .ToList();
        }

        public ManagerEntity GetApproved(string id)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator)
                .Include(m => m.Stores)
                .FirstOrDefault(m => m.Id == id && m.Identity.Approved && !m.Identity.Deleted);
        }

        public ManagerEntity GetLoaded(string id)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .FirstOrDefault(m => m.Id == id);
        }

        public async Task<Result> UpdateAsync(ManagerEntity manager)
        {
            var uow = new UnitOfWork(DbContext);
            return await uow.Accounts.UpdateAsync(manager.Identity, "Manager");
        }

        public Result MarkAsDeleted(string id)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Manager deleted."
            };
            var manager = DbContext.Managers
                .Include(m => m.Identity)
                .FirstOrDefault(m => m.Id == id);
            if (manager != null)
            {
                if (manager.Identity != null)
                {
                    manager.Identity.Deleted = true;
                }
            }
            else
            {
                modelState.Add("Manager does not exist.");
            }
            return modelState.GetResult();
        }

        public Result MarkAsRestored(string id)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Manager restored."
            };
            var manager = DbContext.Managers
                .Include(m => m.Identity)
                .FirstOrDefault(m => m.Id == id);
            if (manager != null)
            {
                if (manager.Identity != null)
                {
                    manager.Identity.Deleted = false;
                }
            }
            else
            {
                modelState.Add("Manager does not exist.");
            }
            return modelState.GetResult();
        }

        #endregion
    }
}