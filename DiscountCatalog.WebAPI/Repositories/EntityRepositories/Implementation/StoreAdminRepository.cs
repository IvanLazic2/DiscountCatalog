using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.ModelState;
using DiscountCatalog.WebAPI.Validation.Validators;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using DiscountCatalog.WebAPI.Extensions;
using DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor;
using DiscountCatalog.WebAPI.REST.StoreAdmin;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Implementation
{
    public class StoreAdminRepository : Repository<StoreAdminEntity>, IStoreAdminRepository
    {
        public ApplicationUserDbContext DbContext
        {
            get
            {
                return Context as ApplicationUserDbContext;
            }
        }

        public StoreAdminRepository(ApplicationUserDbContext context)
            : base(context)
        {
        }

        private List<StoreAdminEntity> Search(IEnumerable<StoreAdminEntity> storeAdmins, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                storeAdmins = storeAdmins.Where(s => s.Identity.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return storeAdmins.ToList();
        }

        private List<StoreAdminEntity> Order(IEnumerable<StoreAdminEntity> storeAdmins, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    storeAdmins = storeAdmins.OrderByDescending(s => s.Identity.UserName).ToList();
                    break;
                default:
                    storeAdmins = storeAdmins.OrderBy(s => s.Identity.UserName).ToList();
                    break;
            }

            return storeAdmins.ToList();
        }

        public async Task<Result> CreateAsync(ApplicationUser user, string password, StoreAdminEntity storeAdmin)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "Store admin created"
            };
            UnitOfWork uow = new UnitOfWork(DbContext);
            Result result = await uow.Accounts.CreateAsync(user, "StoreAdmin", password);
            modelState.Add(result);
            if (result.Success)
            {
                storeAdmin.Identity = user;
                var validator = new StoreAdminValidator();
                var validationResult = await validator.ValidateAsync(storeAdmin);
                modelState.Add(validationResult);
                if (validationResult.IsValid)
                {
                    Add(storeAdmin);
                }
            }
            return modelState.GetResult();
        }

        public StoreAdminEntity GetWithIdentity(string id)
        {
            return DbContext.StoreAdmins
                .Include(s => s.Identity)
                .FirstOrDefault(s => s.Id == id);
        }

        public StoreAdminEntity GetByIdentityId(string identityId)
        {
            var identity = DbContext.Users.Find(identityId);
            return DbContext.StoreAdmins
                .FirstOrDefault(s => s.Identity.Id == identity.Id && s.Identity.Approved && !s.Identity.Deleted);
        }

        public IEnumerable<StoreAdminEntity> GetAllWithIdentity()
        {
            return DbContext.StoreAdmins
                .Include(s => s.Identity)
                .ToList();
        }

        public StoreAdminEntity GetLoadedByIdentityId(string identityId)
        {
            var identity = DbContext.Users.Find(identityId);
            return DbContext.StoreAdmins
                .Include(s => s.Identity)
                .Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Managers.Select(m => m.Stores))
                .Include(s => s.Managers.Select(m => m.Administrator))
                .Include(s => s.Stores)
                .FirstOrDefault(s => s.Identity.Id == identity.Id);
        }

        public IEnumerable<StoreAdminEntity> GetAllLoaded(string sortOrder, string searchString)
        {
            var storeAdmins = DbContext.StoreAdmins
                .Include(s => s.Identity)
                .Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Stores)
                .OrderBy(s => s.Identity.UserName)
                .ToList();

            storeAdmins = Search(storeAdmins, searchString);
            storeAdmins = Order(storeAdmins, sortOrder);

            return storeAdmins;
        }

        public IEnumerable<StoreAdminEntity> GetAllApproved(string sortOrder, string searchString)
        {
            var storeAdmins = DbContext.StoreAdmins
                .Include(s => s.Identity)
                .Include(s => s.Managers)
                .Include(s => s.Stores)
                .OrderBy(s => s.Identity.UserName)
                .Where(s => s.Identity.Approved == true && s.Identity.Deleted == false)
                .ToList();

            storeAdmins = Search(storeAdmins, searchString);
            storeAdmins = Order(storeAdmins, sortOrder);

            return storeAdmins;
        }

        public IEnumerable<StoreAdminEntity> GetAllDeleted(string sortOrder, string searchString)
        {
            var storeAdmins = DbContext.StoreAdmins
                .Include(s => s.Identity)
                .Where(s => s.Identity.Deleted == true)
                .OrderBy(s => s.Identity.UserName)
                .ToList();

            storeAdmins = Search(storeAdmins, searchString);
            storeAdmins = Order(storeAdmins, sortOrder);

            return storeAdmins;
        }

        public StoreAdminEntity GetLoaded(string id)
        {
            return DbContext.StoreAdmins
                .Include(s => s.Identity)
                .Include(s => s.Managers.Select(m => m.Identity))
                .Include(s => s.Stores)
                .FirstOrDefault(s => s.Id == id);
        }

        public StoreAdminEntity GetApproved(string id)
        {
            return DbContext.StoreAdmins
                .Include(s => s.Identity)
                .Include(s => s.Managers)
                .Include(s => s.Stores)
                .FirstOrDefault(s => s.Id == id && s.Identity.Approved && !s.Identity.Deleted);
        }

        public Task<Result> UpdateAsync(StoreAdminRESTPut storeAdmin)
        {
            var uow = new UnitOfWork(DbContext);
            return uow.Accounts.UpdateAsync(storeAdmin.Identity, "StoreAdmin");
        }

        public Result MarkAsDeleted(string id)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "StoreAdmin deleted."
            };
            var storeAdmin = DbContext.StoreAdmins
                .Include(s => s.Identity)
                .FirstOrDefault(s => s.Id == id);
            if (storeAdmin != null)
            {
                if (storeAdmin.Identity != null)
                {
                    storeAdmin.Identity.Deleted = true;
                }
            }
            else
            {
                modelState.Add("StoreAdmin does not exist.");
            }
            return modelState.GetResult();
        }

        public Result MarkAsRestored(string id)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "StoreAdmin restored."
            };
            var storeAdmin = DbContext.StoreAdmins
                .Include(s => s.Identity)
                .FirstOrDefault(s => s.Id == id);
            if (storeAdmin != null)
            {
                if (storeAdmin.Identity != null)
                {
                    storeAdmin.Identity.Deleted = false;
                }
            }
            else
            {
                modelState.Add("StoreAdmin does not exist.");
            }
            return modelState.GetResult();
        }



    }
}