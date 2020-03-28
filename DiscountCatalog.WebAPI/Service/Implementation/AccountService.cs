﻿using AbatementHelper.WebAPI.Mapping;
using AutoMapper;
using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.Models.Extended;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.BindingModels;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.REST.Account;
using DiscountCatalog.WebAPI.Service.Contractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.WebAPI.Service.Implementation
{
    public class AccountService : IAccountService
    {
        readonly IMapper mapper;

        public AccountService()
        {
            mapper = AutoMapping.Initialise();
        }

        public async Task<AuthenticatedUserResult> AuthenticateAsync(AuthenticationBindingModel model)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                AuthenticatedUserResult result = await uow.Accounts.Authenticate(model.EmailOrUserName, model.Password);

                return result;
            }
        }

        public Result Delete(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Accounts.MarkAsDeleted(id);

                uow.Complete();

                return result;
            }
        }

        public AccountREST Details(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ApplicationUser user = uow.Accounts.GetApproved(id);

                AccountREST mapped = mapper.Map<AccountREST>(user);

                return mapped;
            }
        }

        public async Task<byte[]> GetUserImageAsync(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                byte[] image = await uow.Accounts.GetUserImage(id);

                return ImageProcessor.CreateThumbnail(image);
            }
        }

        public async Task<Result> PostUserImageAsync(string id, byte[] image)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = await uow.Accounts.PostUserImage(id, image);

                uow.Complete();

                return result;
            }
        }

        public async Task<Result> RegisterAsync(RegisterBindingModel model)
        {
            var result = new Result();

            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0]
            };

            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                if (model.Role == "User")
                {
                    result = await uow.Accounts.CreateAsync(user, model.Role, model.Password);
                }
                else if (model.Role == "StoreAdmin")
                {
                    result = await uow.StoreAdmins.CreateAsync(user, model.Password, new StoreAdminEntity());
                }

                uow.Complete();
            }

            return result;
        }

        public Result Restore(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Accounts.MarkAsRestored(id);

                uow.Complete();

                return result;
            }
        }

        public async Task<Result> UpdateAsync(AccountRESTPut model)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                ApplicationUser user = uow.Accounts.GetApproved(model.Id);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Country = model.Country;
                user.City = model.City;
                user.PostalCode = model.PostalCode;
                user.Street = model.Street;

                Result result = await uow.Accounts.UpdateAsync(user, "User");

                if (result.Success)
                {
                    uow.Complete();
                }
                else
                {
                    uow.Dispose();
                }

                return result;
            }
        }
    }
}