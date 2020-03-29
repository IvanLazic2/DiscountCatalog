using AbatementHelper.WebAPI.Mapping;
using AbatementHelper.WebAPI.Models.BindingModels;
using AutoMapper;
using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.ModelState;
using DiscountCatalog.WebAPI.Paging;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.Validation.Validators;
using FluentValidation.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DiscountCatalog.WebAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        private readonly IMapper mapper;

        public AdminController()
        {
            mapper = AutoMapping.Initialise();
        }

        #region Create

        //CREATE

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IHttpActionResult> CreateUser(UserBindingModel model)
        {
            try
            {
                using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
                {
                    var user = mapper.Map<ApplicationUser>(model);

                    Result result = await uow.Accounts.CreateAsync(user, model.Role, model.Password);

                    uow.Complete();

                    if (result.Success)
                    {
                        return Content(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        return Content(HttpStatusCode.BadRequest, result);
                    }
                }
            }
            catch (Exception exc)
            {

                throw;
            }

        }

        [HttpPost]
        [Route("CreateStoreAdmin")]
        public async Task<IHttpActionResult> CreateStoreAdmin(StoreAdminBindingModel model)
        {
            try
            {
                using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
                {
                    var user = mapper.Map<ApplicationUser>(model);

                    Result result = await uow.StoreAdmins.CreateAsync(user, model.Password, new StoreAdminEntity());

                    uow.Complete();

                    if (result.Success)
                    {
                        return Content(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        return Content(HttpStatusCode.BadRequest, result);
                    }
                }
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("CreateStore")]
        public async Task<IHttpActionResult> CreateStore(StoreBindingModel model)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var store = mapper.Map<StoreEntity>(model);

                Result result = await uow.Stores.CreateAsync(store, model.StoreAdminId);

                uow.Complete();

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }

        }

        //[HttpPost]
        //[Route("CreateManager")]
        //public async Task<IHttpActionResult> CreateManager(ManagerBindingModel model)
        //{
        //    try
        //    {
        //        using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //        {
        //            var user = mapper.Map<ApplicationUser>(model);

        //            Result result = await uow.Managers.CreateAsync(user, model.Password, new ManagerEntity(), model.StoreAdminId);

        //            uow.Complete();

        //            if (result.Success)
        //            {
        //                return Content(HttpStatusCode.OK, result);
        //            }
        //            else
        //            {
        //                return Content(HttpStatusCode.BadRequest, result);
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {

        //        throw;
        //    }
        //}

        #endregion

        #region Read


        //READ

        [HttpGet]
        [Route("GetAllUsers")]
        public IHttpActionResult GetAllUsers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            try
            {
                using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
                {
                    var users = uow.Accounts.GetAllLoaded(sortOrder, searchString);

                    var mapped = mapper.Map<List<User>>(users);

                    //foreach (var item in mapped)
                    //{
                    //    item.Role = uow.Accounts.GetRoleName(item.Id);
                    //}

                    mapped.ForEach(u => u.Role = uow.Accounts.GetRoleName(u.Id));

                    var subset = mapped.ToPagedList(pageIndex, pageSize);
                    var result = new { items = subset, metaData = subset.GetMetaData() };

                    return Ok(result);
                }
            }
            catch (Exception exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetAllApprovedUsers")]
        public IHttpActionResult GetAllApprovedUsers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var users = uow.Accounts.GetAllApproved(sortOrder, searchString);

                var mapped = mapper.Map<List<User>>(users);

                mapped.ForEach(u => u.Role = uow.Accounts.GetRoleName(u.Id));

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }

        }

        [HttpGet]
        [Route("GetAllDeletedUsers")]
        public IHttpActionResult GetAllDeletedUsers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var users = uow.Accounts.GetAllDeleted(sortOrder, searchString);

                var mapped = mapper.Map<List<User>>(users);

                mapped.ForEach(u => u.Role = uow.Accounts.GetRoleName(u.Id));

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetUser/{id}")]
        public IHttpActionResult GetUser(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var user = uow.Accounts.GetLoaded(id);

                if (user != null)
                {
                    var mapped = mapper.Map<User>(user);

                    mapped.Role = uow.Accounts.GetRoleName(id);

                    return Ok(mapped);
                }
                else
                {
                    return BadRequest("User does not exist");
                }
            }
        }



        [HttpGet]
        [Route("GetAllStoreAdmins")]
        public IHttpActionResult GetAllStoreAdmins(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmins = uow.StoreAdmins.GetAllLoaded(sortOrder, searchString);

                var mapped = mapper.Map<List<StoreAdmin>>(storeAdmins);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetAllApprovedStoreAdmins")]
        public IHttpActionResult GetAllApprovedStoreAdmins(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmins = uow.StoreAdmins.GetAllApproved(sortOrder, searchString);

                var mapped = mapper.Map<List<StoreAdmin>>(storeAdmins);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetAllDeletedStoreAdmins")]
        public IHttpActionResult GetAllDeletedStoreAdmins(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmins = uow.StoreAdmins.GetAllDeleted(sortOrder, searchString);

                var mapped = mapper.Map<List<StoreAdmin>>(storeAdmins);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetStoreAdminByIdentityId/{id}")]
        public IHttpActionResult GetStoreAdminByIdentityId(string identityId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmin = uow.StoreAdmins.GetLoadedByIdentityId(identityId);

                if (storeAdmin != null)
                {
                    var mapped = mapper.Map<StoreAdmin>(storeAdmin);

                    return Ok(mapped);
                }
                else
                {
                    return BadRequest("Store admin does not exist.");
                }
            }
        }

        [HttpGet]
        [Route("GetStoreAdmin/{id}")]
        public IHttpActionResult GetStoreAdmin(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmin = uow.StoreAdmins.GetLoaded(id);

                if (storeAdmin != null)
                {
                    var mapped = mapper.Map<StoreAdmin>(storeAdmin);

                    return Ok(mapped);
                }
                else
                {
                    return BadRequest("Store admin does not exist.");
                }
            }
        }



        [HttpGet]
        [Route("GetAllManagers")]
        public IHttpActionResult GetAllManagers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var managers = uow.Managers.GetAllLoaded();

                var mapped = mapper.Map<List<Manager>>(managers);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetAllApprovedManagers")]
        public IHttpActionResult GetAllApprovedManagers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var managers = uow.Managers.GetAllApproved();

                var mapped = mapper.Map<List<Manager>>(managers);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetAllDeletedManagers")]
        public IHttpActionResult GetAllDeletedManagers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var managers = uow.Managers.GetAllDeleted();

                var mapped = mapper.Map<List<Manager>>(managers);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        //[HttpGet]
        //[Route("GetManagerByIdentityId/{id}")]
        //public IHttpActionResult GetManagerByIdentityId(string id)
        //{
        //    using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //    {
        //        var managers = uow.Managers.GetLoadedByIdentityId(id);

        //        var mapped = mapper.Map<List<Manager>>(managers);

        //        return Ok(mapped);
        //    }
        //}

        [HttpGet]
        [Route("GetManager/{id}")]
        public IHttpActionResult GetManager(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var manager = uow.Managers.GetLoaded(id);

                var mapped = mapper.Map<Manager>(manager);

                return Ok(mapped);
            }
        }



        [HttpGet]
        [Route("GetAllStores")]
        public IHttpActionResult GetAllStores(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var stores = uow.Stores.GetAllLoaded();

                var mapped = mapper.Map<List<Store>>(stores);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetAllApprovedManagers")]
        public IHttpActionResult GetAllApprovedStores(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var stores = uow.Stores.GetAllApproved();

                var mapped = mapper.Map<List<Store>>(stores);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetAllDeletedManagers")]
        public IHttpActionResult GetAllDeletedStores(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var stores = uow.Stores.GetAllDeleted();

                var mapped = mapper.Map<List<Store>>(stores);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetStore/{id}")]
        public IHttpActionResult GetStore(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var store = uow.Stores.GetLoaded(id);

                var mapped = mapper.Map<Store>(store);

                return Ok(mapped);
            }
        }

        #endregion

        #region Update
        
        //UPDATE

        //[HttpPut]
        //[Route("EditUser/{id}")]
        //public async Task<IHttpActionResult> EditUser(string id, UserBindingModel model)
        //{
        //    try
        //    {
        //        using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //        {
        //            var user = mapper.Map<ApplicationUser>(model);

        //            user.Id = id;

        //            Result result = await uow.Accounts.UpdateAsync(user, model.Role);

        //            if (result.Success)
        //            {
        //                uow.Complete();
        //                return Content(HttpStatusCode.OK, result);
        //            }
        //            else
        //            {
        //                uow.Dispose();
        //                return Content(HttpStatusCode.BadRequest, result);
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {

        //        throw;
        //    }

        //}

        //[HttpPut]
        //[Route("EditStoreAdmin/{id}")]
        //public async Task<IHttpActionResult> EditStoreAdmin(string id, StoreAdminBindingModel model)
        //{
        //    try
        //    {
        //        using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //        {
        //            var identity = mapper.Map<ApplicationUser>(model.Identity);
        //            identity.Id = id;

        //            var storeAdmin = new StoreAdminEntity { Identity = identity };
        //            Result result = await uow.StoreAdmins.UpdateAsync(storeAdmin);

        //            if (result.Success)
        //            {
        //                uow.Complete();
        //            }
        //            else
        //            {
        //                uow.Dispose();
        //            }

        //            if (result.Success)
        //            {
        //                return Content(HttpStatusCode.OK, result);
        //            }
        //            else
        //            {
        //                return Content(HttpStatusCode.BadRequest, result);
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {

        //        throw;
        //    }

        //}

        //[HttpPut]
        //[Route("EditManager")]
        //public async Task<IHttpActionResult> EditManager(ManagerBindingModel model)
        //{
        //    using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //    {
        //        var manager = new ManagerEntity { Identity = mapper.Map<ApplicationUser>(model) };
        //        Result result = await uow.Managers.UpdateAsync(manager);

        //        if (result.Success)
        //        {
        //            uow.Complete();
        //            return Content(HttpStatusCode.OK, result);
        //        }
        //        else
        //        {
        //            uow.Dispose();
        //            return Content(HttpStatusCode.BadRequest, result);
        //        }
        //    }
        //}

        //[HttpPut]
        //[Route("EditStore")]
        //public async Task<IHttpActionResult> EditStore(StoreBindingModel model)
        //{
        //    using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //    {
        //        var store = mapper.Map<StoreEntity>(model);
        //        Result result = await uow.Stores.UpdateAsync(store);

        //        if (result.Success)
        //        {
        //            uow.Complete();
        //            return Content(HttpStatusCode.OK, result);
        //        }
        //        else
        //        {
        //            uow.Dispose();
        //            return Content(HttpStatusCode.BadRequest, result);
        //        }
        //    }
        //}

        #endregion

        #region Delete
        
        //DELETE

        [HttpGet]
        [Route("DeleteUser/{id}")]
        public IHttpActionResult DeleteUser(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Accounts.MarkAsDeleted(id);

                uow.Complete();

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
        }

        [HttpGet]
        [Route("DeleteStoreAdmin/{id}")]
        public IHttpActionResult DeleteStoreAdmin(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.StoreAdmins.MarkAsDeleted(id);

                uow.Complete();

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
        }

        [HttpGet]
        [Route("DeleteManager/{id}")]
        public IHttpActionResult DeleteManager(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Managers.MarkAsDeleted(id);

                uow.Complete();

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
        }

        [HttpGet]
        [Route("DeleteStore/{id}")]
        public IHttpActionResult DeleteStore(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.MarkAsDeleted(id);

                uow.Complete();

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
        }

        #endregion

        #region Restore
        
        //RESTORE

        [HttpGet]
        [Route("RestoreUser/{id}")]
        public IHttpActionResult RestoreUser(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Accounts.MarkAsRestored(id);

                uow.Complete();

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
        }

        [HttpGet]
        [Route("RestoreStoreAdmin/{id}")]
        public IHttpActionResult RestoreStoreAdmin(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.StoreAdmins.MarkAsRestored(id);

                uow.Complete();

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
        }

        [HttpGet]
        [Route("RestoreManager/{id}")]
        public IHttpActionResult RestoreManager(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Managers.MarkAsRestored(id);

                uow.Complete();

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
        }

        [HttpGet]
        [Route("RestoreStore/{id}")]
        public IHttpActionResult RestoreStore(string id)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.MarkAsRestored(id);

                uow.Complete();

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
        }

        #endregion

    }
}
