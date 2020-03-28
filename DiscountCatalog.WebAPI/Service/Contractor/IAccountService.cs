using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.Models.Extended;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.BindingModels;
using DiscountCatalog.WebAPI.REST.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Service.Contractor
{
    public interface IAccountService
    {
        Task<Result> RegisterAsync(RegisterBindingModel model);
        Task<AuthenticatedUserResult> AuthenticateAsync(AuthenticationBindingModel model);
        AccountREST Details(string id);
        Task<Result> UpdateAsync(AccountRESTPut model);
        Result Delete(string id);
        Result Restore(string id);
        Task<Result> PostUserImageAsync(string id, byte[] image);
        Task<byte[]> GetUserImageAsync(string id);
    }
}
