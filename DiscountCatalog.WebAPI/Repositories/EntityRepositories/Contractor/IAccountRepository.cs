using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.Models.Extended;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.REST.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor
{
    public interface IAccountRepository : IRepository<ApplicationUser> 
    {
        Task<Result> CreateAsync(ApplicationUser user, string role, string password);
        Task<Result> UpdateAsync(AccountRESTPut user, string role);
        Task<Result> DeleteAsync(ApplicationUser user);
        Task<ApplicationUser> FindByIdAsync(string id);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByNameAsync(string name);
        Task<IdentityResult> AddToRoleAsync(string id, string role);
        
        Task<IList<string>> GetRolesAsync(string id);
        string GetRoleName(string id);
        Task<bool> IsInRoleAsync(string id, string role);
        Task<IdentityResult> RemoveFromRoleAsync(string id, string role);
        Task SendEmailAsync(string id, string subject, string body);

        IEnumerable<ApplicationUser> GetAllLoaded(string sortOrder, string searchString);
        IEnumerable<ApplicationUser> GetAllApproved(string sortOrder, string searchString);
        IEnumerable<ApplicationUser> GetAllDeleted(string sortOrder, string searchString);
        ApplicationUser GetLoaded(string id);
        ApplicationUser GetApproved(string id);
        ApplicationUser GetApprovedByName(string name);

        Result MarkAsDeleted(string id);
        Result MarkAsRestored(string id);

        Task<AuthenticatedUserResult> Authenticate(string emailOrUserName, string password);

        Task<Result> PostUserImage(string id, byte[] image);
        Task<byte[]> GetUserImage(string id);
    }
}
