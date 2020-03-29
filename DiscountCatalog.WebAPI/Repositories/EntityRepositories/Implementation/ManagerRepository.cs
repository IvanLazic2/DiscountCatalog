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
using DiscountCatalog.WebAPI.REST.Manager;
using AutoMapper;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Implementation
{
    public class ManagerRepository : Repository<ManagerEntity>, IManagerRepository
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

        public ManagerRepository(ApplicationUserDbContext context)
                    : base(context)
        {
        }
        
        #endregion

        #region Methods

        //CREATE

        #region Create

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

        #endregion

        //GET ALL

        #region GetAllApproved

        public IEnumerable<ManagerEntity> GetAllApproved()
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .Where(m => m.Identity.Approved && !m.Identity.Deleted)
                .ToList();
        }

        public IEnumerable<ManagerEntity> GetAllApproved(string storeAdminIdentityId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .Where(m => m.Identity.Approved && m.Administrator.Identity.Id == storeAdminIdentityId && !m.Identity.Deleted)
                .ToList();
        }

        #endregion

        #region GetAllDeleted

        public IEnumerable<ManagerEntity> GetAllDeleted()
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Where(m => m.Identity.Deleted)
                .ToList();
        }

        public IEnumerable<ManagerEntity> GetAllDeleted(string storeAdminIdentityId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Where(m => m.Identity.Deleted && m.Administrator.Identity.Id == storeAdminIdentityId)
                .ToList();
        }

        #endregion

        #region GetAllLoaded

        public IEnumerable<ManagerEntity> GetAllLoaded()
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .ToList();
        }

        public IEnumerable<ManagerEntity> GetAllLoaded(string storeAdminIdentityId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .Where(m => m.Administrator.Identity.Id == storeAdminIdentityId)
                .ToList();
        }

        #endregion

        //GET

        #region GetApproved

        public ManagerEntity GetApproved(string managerId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator)
                .Include(m => m.Stores)
                .FirstOrDefault(m => m.Id == managerId && m.Identity.Approved && !m.Identity.Deleted);
        }

        public ManagerEntity GetApproved(string storeAdminIdentityId, string managerId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator)
                .Include(m => m.Stores)
                .FirstOrDefault(m => m.Id == managerId && m.Administrator.Identity.Id == storeAdminIdentityId && m.Identity.Approved && !m.Identity.Deleted);
        }

        #endregion

        #region GetLoaded

        public ManagerEntity GetLoaded(string managerId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .FirstOrDefault(m => m.Id == managerId);
        }

        public ManagerEntity GetLoaded(string storeAdminIdentityId, string managerId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .FirstOrDefault(m => m.Id == managerId && m.Administrator.Id == storeAdminIdentityId);
        }

        #endregion

        //UPDATE

        #region Update

        public async Task<Result> UpdateAsync(ManagerRESTPut manager)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "info updated."
            };

            var uow = new UnitOfWork(DbContext);

            ManagerEntity managerEntity = GetApproved(manager.Id);

            if (managerEntity != null)
            {   
                Result result = await uow.Accounts.UpdateAsync(manager.Identity, "Manager");
                modelState.Add(result);
            }
            else
            {
                modelState.Add("Manager does not exist.");
            }

            return modelState.GetResult();
        }

        public async Task<Result> UpdateAsync(string storeAdminIdentityId, ManagerRESTPut manager)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "info updated."
            };

            var uow = new UnitOfWork(DbContext);

            ManagerEntity managerEntity = GetApproved(storeAdminIdentityId, manager.Id);

            if (managerEntity != null)
            {
                Result result = await uow.Accounts.UpdateAsync(manager.Identity, "Manager");
                modelState.Add(result);
            }
            else
            {
                modelState.Add("Manager does not exist.");
            }

            return modelState.GetResult();
        }

        #endregion

        //DELETE

        #region MarkAsDeleted

        public Result MarkAsDeleted(string managerId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Manager deleted."
            };

            var manager = GetApproved(managerId);

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

        public Result MarkAsDeleted(string storeAdminIdentityId, string managerId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Manager deleted."
            };

            var manager = GetApproved(storeAdminIdentityId, managerId);

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

        #endregion

        //RESTORE

        #region MarkAsRestored

        public Result MarkAsRestored(string managerId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Manager restored."
            };

            var manager = DbContext.Managers
                .Include(m => m.Identity)
                .FirstOrDefault(m => m.Id == managerId && m.Identity.Deleted);

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

        public Result MarkAsRestored(string storeAdminIdentityId, string managerId)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Manager restored."
            };

            var manager = DbContext.Managers
                .Include(m => m.Identity)
                .FirstOrDefault(m => m.Id == managerId && m.Administrator.Identity.Id == storeAdminIdentityId && m.Identity.Deleted);

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

        #endregion
    }
}