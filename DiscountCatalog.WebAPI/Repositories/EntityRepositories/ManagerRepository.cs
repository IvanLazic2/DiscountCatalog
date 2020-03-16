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

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories
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

        private List<ManagerEntity> Search(IEnumerable<ManagerEntity> managers, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                managers = managers.Where(m => m.Identity.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return managers.ToList();
        }

        private List<ManagerEntity> Order(IEnumerable<ManagerEntity> managers, string sortOrder)
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

        public ManagerRepository(ApplicationUserDbContext context)
            : base(context)
        {
        }

        public async Task<Result> CreateAsync(ApplicationUser user, string password, ManagerEntity manager, string storeAdminId)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Manager created."
            };
            UnitOfWork uow = new UnitOfWork(DbContext);
            var identityResult = await uow.Accounts.CreateAsync(user, "Manager", password);
            modelState.Add(identityResult);
            if (identityResult.Success)
            {
                manager.Identity = user;
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
            var managers = DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .Where(m => m.Identity.Approved && !m.Identity.Deleted)
                .ToList();

            managers = Search(managers, searchString);
            managers = Order(managers, sortOrder);

            return managers;
        }

        public IEnumerable<ManagerEntity> GetAllDeleted(string sortOrder, string searchString)
        {
            var managers = DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Where(m => m.Identity.Deleted)
                .ToList();


            managers = Search(managers, searchString);
            managers = Order(managers, sortOrder);

            return managers;
        }

        public IEnumerable<ManagerEntity> GetAllLoaded(string sortOrder, string searchString)
        {
            var managers = DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .ToList();

            managers = Search(managers, searchString);
            managers = Order(managers, sortOrder);

            return managers;
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

        public ManagerEntity GetByIdentityId(string identityId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .FirstOrDefault(m => m.Identity.Id == identityId);
        }

        public ManagerEntity GetLoadedByIdentityId(string identityId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .FirstOrDefault(m => m.Identity.Id == identityId);
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

        public ManagerEntity GetApprovedByIdentityId(string identityId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .FirstOrDefault(m => m.Identity.Id == identityId && !m.Identity.Deleted && m.Identity.Approved);
        }

        public IEnumerable<ManagerEntity> GetAllByStoreAdminId(string storeAdminId, string sortOrder, string searchString)
        {
            var managers = DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .Where(m => m.Administrator.Identity.Id == storeAdminId)
                .ToList();

            managers = Search(managers, searchString);
            managers = Order(managers, sortOrder);

            return managers;
        }

        public ManagerEntity GetByStoreAdminId(string storeAdminId, string managerId)
        {
            return DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Include(m => m.Stores)
                .FirstOrDefault(m => m.Administrator.Identity.Id == storeAdminId && m.Id == managerId && m.Identity.Approved && !m.Identity.Deleted);
        }

        public IEnumerable<ManagerEntity> GetAllDeletedByStoreAdminId(string storeAdminId, string sortOrder, string searchString)
        {
            var managers = DbContext.Managers
                .Include(m => m.Identity)
                .Include(m => m.Administrator.Identity)
                .Where(m => m.Administrator.Identity.Id == storeAdminId && m.Identity.Deleted)
                .ToList();

            managers = Search(managers, searchString);
            managers = Order(managers, sortOrder);

            return managers;
        }
    }
}