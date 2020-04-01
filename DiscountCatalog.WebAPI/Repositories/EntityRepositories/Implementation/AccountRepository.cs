using AbatementHelper.WebAPI.Mapping;
using AutoMapper;
using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.Models.Extended;
using DiscountCatalog.WebAPI.Extensions;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.ModelState;
using DiscountCatalog.WebAPI.Paging;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor;
using DiscountCatalog.WebAPI.REST.Account;
using DiscountCatalog.WebAPI.Validation.Validators;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Implementation
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        private readonly UserStore<ApplicationUser> _userStore;
        private readonly UserManager _userManager;
        private HttpClient apiClient;

        public ApplicationUserDbContext DbContext
        {
            get
            {
                return Context as ApplicationUserDbContext;
            }
        }

        public AccountRepository(ApplicationUserDbContext context)
            : base(context)
        {
            _userStore = new UserStore<ApplicationUser>(Context);
            _userStore.AutoSaveChanges = false;
            _userManager = new UserManager(_userStore);

            _userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireUppercase = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonLetterOrDigit = false
            };

            _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = true
            };

            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private List<ApplicationUser> Search(IEnumerable<ApplicationUser> users, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return users.ToList();
        }

        private List<ApplicationUser> Order(IEnumerable<ApplicationUser> users, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.UserName).ToList();
                    break;
                default:
                    users = users.OrderBy(u => u.UserName).ToList();
                    break;
            }

            return users.ToList();
        }


        private IdentityUserRole GetIdentityUserRole(IdentityRole role, ApplicationUser user)
        {
            if (role != null)
            {
                return new IdentityUserRole { RoleId = role.Id, UserId = user.Id };
            }
            else
            {
                return null;
            }
        }

        public async Task<Result> CreateAsync(ApplicationUser user, string role, string password)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "User created."
            };
            user.UserName = user.Email.Split('@')[0];
            user.Approved = true;
            user.Deleted = false;
            var identityRole = DbContext.Roles.Where(r => r.Name == role).FirstOrDefault();
            user.Roles.Add(GetIdentityUserRole(identityRole, user));
            var validator = new UserValidator();
            var validationResult = await validator.ValidateAsync(user);
            modelState.Add(validationResult);
            if (validationResult.IsValid)
            {
                var userResult = await _userManager.UserValidator.ValidateAsync(user);

                var passResult = await _userManager.PasswordValidator.ValidateAsync(password);
                IdentityResult identityResult = await _userManager.CreateAsync(user, password);
                identityResult.Errors.Concat(userResult.Errors);
                identityResult.Errors.Concat(passResult.Errors);
                modelState.Add(identityResult);
            }
            return modelState.GetResult();
        }

        public async Task<Result> UpdateAsync(AccountRESTPut user, string role)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Info updated."
            };
            var dbUser = await FindByIdAsync(user.Id);
            if (dbUser != null)
            {
                dbUser.Email = user.Email;
                dbUser.UserName = user.UserName;
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.PhoneNumber = user.PhoneNumber;
                dbUser.Country = user.Country;
                dbUser.City = user.City;
                dbUser.PostalCode = user.PostalCode;
                dbUser.Street = user.Street;

                dbUser.Roles.Clear();
                var identityRole = DbContext.Roles.Where(r => r.Name == role).FirstOrDefault();
                dbUser.Roles.Add(GetIdentityUserRole(identityRole, dbUser));

                var validator = new UserValidator();
                var validationResult = await validator.ValidateAsync(dbUser);
                modelState.Add(validationResult);
                if (validationResult.IsValid)
                {
                    var userResult = await _userManager.UserValidator.ValidateAsync(dbUser);
                    IdentityResult identityResult = await _userManager.UpdateAsync(dbUser); //temp
                    identityResult.Errors.Concat(userResult.Errors);
                    modelState.Add(identityResult);
                }
            }
            else
            {
                modelState.Add("User does not exist.");
            }
            return modelState.GetResult();
        }

        public async Task<Result> DeleteAsync(ApplicationUser user)
        {
            var modelState = new EntityModelState
            {
                SuccessMessage = "User removed from database."
            };
            var result = await _userManager.DeleteAsync(user);
            modelState.Add(result);
            return modelState.GetResult();
        }

        public Result MarkAsDeleted(string id)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "User deleted."
            };
            var user = Get(id);
            if (user != null)
            {
                user.Deleted = true;
            }
            else
            {
                modelState.Add("User does not exist.");
            }
            return modelState.GetResult();
        }

        public Result MarkAsRestored(string id)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "User restored."
            };
            var user = Get(id);
            if (user != null)
            {
                user.Deleted = false;
            }
            else
            {
                modelState.Add("User does not exist.");
            }
            return modelState.GetResult();
        }

        public Task<ApplicationUser> FindByIdAsync(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public Task<ApplicationUser> FindByNameAsync(string name)
        {
            return _userManager.FindByNameAsync(name);
        }

        public Task<IList<string>> GetRolesAsync(string id)
        {
            return _userManager.GetRolesAsync(id);
        }

        public async Task<bool> IsInRoleAsync(string id, string role)
        {
            if (!string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(id))
            {
                return await _userManager.IsInRoleAsync(id, role);
            }

            return false;
        }

        public Task<IdentityResult> RemoveFromRoleAsync(string id, string role)
        {
            return _userManager.RemoveFromRoleAsync(id, role);
        }

        public Task SendEmailAsync(string id, string subject, string body)
        {
            return _userManager.SendEmailAsync(id, subject, body);
        }

        public Task<IdentityResult> AddToRoleAsync(string id, string role)
        {
            return _userManager.AddToRoleAsync(id, role);
        }

        public IEnumerable<ApplicationUser> GetAllLoaded(string sortOrder, string searchString)
        {
            var users = DbContext.Users
                .Include(u => u.Roles)
                .OrderBy(u => u.UserName)
                .ToList();

            users = Search(users, searchString);
            users = Order(users, sortOrder);

            return users;
        }

        public IEnumerable<ApplicationUser> GetAllApproved(string sortOrder, string searchString)
        {
            var users = DbContext.Users
                .Include(u => u.Roles)
                .Where(u => u.Approved && !u.Deleted)
                .OrderBy(u => u.UserName)
                .ToList();

            users = Search(users, searchString);
            users = Order(users, sortOrder);

            return users;
        }

        public IEnumerable<ApplicationUser> GetAllDeleted(string sortOrder, string searchString)
        {
            var users = DbContext.Users
                .Where(u => u.Deleted)
                .OrderBy(u => u.UserName)
                .ToList();

            users = Search(users, searchString);
            users = Order(users, sortOrder);

            return users;
        }

        public ApplicationUser GetLoaded(string id)
        {
            return DbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == id);
        }

        public string GetRoleName(string id)
        {
            try
            {
                var user = GetLoaded(id);
                if (user != null)
                {
                    var roleId = user.Roles.FirstOrDefault().RoleId;

                    return DbContext.Roles.Find(roleId).Name;
                }
                return null;

                //var roleId = DbContext.Users.Find(id)
                //                .Roles.FirstOrDefault().RoleId;
            }
            catch (Exception exc)
            {

                throw;
            }
            //DbContext.Roles.Find(roleId).Name;
        }

        private async Task<string> ReturnUserNameAsync(string usernameOrEmail)
        {
            string username = usernameOrEmail;

            if (usernameOrEmail.Contains("@"))
            {
                var userForEmail = await FindByEmailAsync(usernameOrEmail);

                if (userForEmail != null)
                {
                    username = userForEmail.UserName;
                }

            }

            return username;
        }

        public async Task<AuthenticatedUserResult> Authenticate(string emailOrUserName, string password)
        {
            var result = new AuthenticatedUserResult();

            ApplicationUser user = GetApprovedByName(await ReturnUserNameAsync(emailOrUserName));

            if (user != null)
            {
                var data = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", user.UserName),
                    new KeyValuePair<string, string>("password", password),
                });

                HttpResponseMessage response = await apiClient.PostAsync("/token", data);

                AuthenticatedUser authenticatedUser = await response.Content.ReadAsAsync<AuthenticatedUser>();

                if (authenticatedUser == null || !response.IsSuccessStatusCode)
                {
                    result.AddModelError("Password", "Incorrect password.");
                }
                else
                {
                    result.Id = authenticatedUser.Id;
                    result.Access_Token = authenticatedUser.Access_Token;
                    result.UserName = authenticatedUser.UserName;
                    result.Email = authenticatedUser.Email;
                    result.Role = authenticatedUser.Role;

                    result.SuccessMessage = $"Logged in as {result.UserName}";
                }
            }
            else
            {
                result.AddModelError("EmailOrUserName", "Account does not exist.");
            }

            return result;
        }

        public async Task<Result> PostUserImage(string id, byte[] image)
        {
            var modelState = new EntityModelState()
            {
                SuccessMessage = "Image uploaded."
            };

            if (ImageProcessor.IsValid(image))
            {
                ApplicationUser user = await FindByIdAsync(id);

                if (user != null)
                {
                    user.UserImage = image;
                }
                else
                {
                    modelState.Add("User does not exist.");
                }
            }
            else
            {
                modelState.Add("Image is not valid.");
            }

            return modelState.GetResult();
        }

        public async Task<byte[]> GetUserImage(string id)
        {
            ApplicationUser user = await FindByIdAsync(id);

            return user.UserImage;
        }

        public ApplicationUser GetApproved(string id)
        {
            return DbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == id && !u.Deleted && u.Approved);
        }

        public ApplicationUser GetApprovedByName(string name)
        {
            return DbContext.Users
               .Include(u => u.Roles)
               .FirstOrDefault(u => u.UserName == name && !u.Deleted && u.Approved);
        }
    }
}