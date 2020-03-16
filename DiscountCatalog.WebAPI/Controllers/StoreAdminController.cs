using AbatementHelper.WebAPI.Mapping;
using AbatementHelper.WebAPI.Models.BindingModels;
using AutoMapper;
using DiscountCatalog.Common.CreateModels;
using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Models.Result;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.Validators;
using FluentValidation.Results;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DiscountCatalog.WebAPI.Controllers
{
    //[Authorize(Roles = "StoreAdmin")]
    [RoutePrefix("api/StoreAdmin")]
    public class StoreAdminController : ApiController
    {
        private readonly IMapper mapper;

        public StoreAdminController()
        {
            mapper = AutoMapping.Initialise();
        }

        //private void SimulateValidation(object model)
        //{
        //    var validationContext = new ValidationContext(model, null, null);
        //    var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        //    Validator.TryValidateObject(model, validationContext, validationResults, true);
        //    foreach (var validationResult in validationResults)
        //    {
        //        ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
        //    }
        //}

        #region Manager

        [HttpPost]
        [Route("CreateManager")]
        public async Task<IHttpActionResult> CreateManager(ManagerBindingModel model)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var user = mapper.Map<ApplicationUser>(model);

                Result result = await uow.Managers.CreateAsync(user, model.Password, new ManagerEntity(), model.StoreAdminId);

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
        [Route("GetAllManagers/{id}")]
        public IHttpActionResult GetAllManagers(string id, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var managers = uow.Managers.GetAllByStoreAdminId(id, sortOrder, searchString);

                var mapped = mapper.Map<List<Manager>>(managers);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetAllDeletedManagers/{id}")]
        public IHttpActionResult GetAllDeletedManagers(string id, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var managers = uow.Managers.GetAllDeletedByStoreAdminId(id, sortOrder, searchString);

                var mapped = mapper.Map<List<Manager>>(managers);

                var subset = mapped.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("GetManager/{id}")]
        public IHttpActionResult GetManager(string id, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var manager = uow.Managers.GetByStoreAdminId(id, managerId);

                var mapped = mapper.Map<Manager>(manager);

                return Ok(mapped);
            }
        }

        [HttpPut]
        [Route("EditManager")]
        public async Task<IHttpActionResult> EditManager(ManagerBindingModel model)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var manager = new ManagerEntity { Identity = mapper.Map<ApplicationUser>(model) };
                Result result = await uow.Managers.UpdateAsync(manager);

                if (result.Success)
                {
                    uow.Complete();
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    uow.Dispose();
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
        }

        [HttpGet]
        [Route("DeleteManager/{id}")]
        public IHttpActionResult DeleteManager(string id, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Managers.MarkAsDeleted(managerId);

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
        public IHttpActionResult RestoreManager(string id, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Managers.MarkAsRestored(managerId);

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

        [HttpPut]
        [Route("PostManagerImage/{id}")]
        public async Task<Result> PostManagerImage(string id, string managerId, byte[] image)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = await uow.Accounts.PostUserImage(managerId, image);

                uow.Complete();

                return result;
            }
        }

        [HttpGet]
        [Route("GetManagerImage/{id}")]
        public async Task<byte[]> GetManagerImage(string id, string managerId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                byte[] image = await uow.Accounts.GetUserImage(managerId);

                return ImageProcessor.CreateThumbnail(image);

                //return image;
            }

            //return ImageProcessor.CreateThumbnail(byteArray);
        }

        [HttpGet]
        [Route("GetManagerStores/{id}")]
        public IHttpActionResult GetManagerStores(string id, string managerId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                List<ManagerStore> managerStores = uow.ManagerStores.GetManagerStores(managerId, sortOrder, searchString).ToList();

                var subset = managerStores.ToPagedList(pageIndex, pageSize);
                var result = new { items = subset, metaData = subset.GetMetaData() };

                return Ok(result);
            }
        }

        [HttpPut]
        [Route("Assign/{id}")]
        public Result Assign(string id, string managerId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.ManagerStores.Assign(managerId, storeId);

                uow.Complete();

                return result;
            }
        }

        [HttpPut]
        [Route("Unassign/{id}")]
        public Result Unassign(string id, string managerId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.ManagerStores.Unassign(managerId, storeId);

                uow.Complete();

                return result;
            }
        }

        #endregion

        #region Store

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

        [HttpGet]
        [Route("GetAllStores")]
        public IHttpActionResult GetAllStores(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var stores = uow.Stores.GetAllApproved(sortOrder, searchString);

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

        [HttpPut]
        [Route("EditStore")]
        public async Task<IHttpActionResult> EditStore(StoreBindingModel model)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var store = mapper.Map<StoreEntity>(model);
                Result result = await uow.Stores.UpdateAsync(store);

                if (result.Success)
                {
                    uow.Complete();
                    return Content(HttpStatusCode.OK, result);
                }
                else
                {
                    uow.Dispose();
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

        //Select

        //PostStoreImage

        //GetStoreImage

        #endregion

    }
}