using AbatementHelper.WebAPI.Mapping;
using AbatementHelper.WebAPI.Models.BindingModels;
using AutoMapper;
using DiscountCatalog.Common.CreateModels;
using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Models.Result;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.REST.Manager;
using DiscountCatalog.WebAPI.Service.Contractor;
using DiscountCatalog.WebAPI.Service.Implementation;
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
        private readonly IManagerService managerService;

        public StoreAdminController()
        {
            mapper = AutoMapping.Initialise();
            managerService = new ManagerService();
        }

        #region Manager

        [HttpPost]
        [Route("CreateManager")]
        public async Task<IHttpActionResult> CreateManager(ManagerRESTPost model)
        {
            Result result = await managerService.CreateAsync(model);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        [Route("GetAllManagers/{storeAdminId}")]
        public IHttpActionResult GetAllManagers(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<ManagerREST> list = managerService.GetAll(storeAdminId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllDeletedManagers/{storeAdminId}")]
        public IHttpActionResult GetAllDeletedManagers(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<ManagerREST> list = managerService.GetAllDeleted(storeAdminId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetManager/{storeAdminId}")]
        public IHttpActionResult GetManager(string storeAdminId, string managerId)
        {
            ManagerREST manager = managerService.Get(storeAdminId, managerId);

            return Ok(manager);
        }

        [HttpPut]
        [Route("EditManager/{storeAdminId}")]
        public async Task<IHttpActionResult> EditManager(string storeAdminId, ManagerRESTPut model)
        {
            Result result = await managerService.UpdateAsync(storeAdminId, model);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        [Route("DeleteManager/{storeAdminId}")]
        public IHttpActionResult DeleteManager(string storeAdminId, string managerId)
        {
            Result result = managerService.Delete(storeAdminId, managerId);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        [Route("RestoreManager/{storeAdminId}")]
        public IHttpActionResult RestoreManager(string storeAdminId, string managerId)
        {
            Result result = managerService.Restore(storeAdminId, managerId);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPut]
        [Route("PostManagerImage/{storeAdminId}")]
        public async Task<IHttpActionResult> PostManagerImage(string storeAdminId, string managerId, byte[] image)
        {
            Result result = await managerService.PostImageAsync(storeAdminId, managerId, image);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        [Route("GetManagerImage/{storeAdminId}")]
        public async Task<byte[]> GetManagerImage(string storeAdminId, string managerId)
        {
            byte[] image = await managerService.GetImageAsync(managerId);

            return image;
        }

        [HttpGet]
        [Route("GetManagerStores/{storeAdminId}")]
        public IHttpActionResult GetManagerStores(string storeAdminId, string managerId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<ManagerStore> managerStores = managerService.GetManagerStores(storeAdminId, managerId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(managerStores);
        }

        [HttpGet]
        [Route("Assign/{storeAdminId}")]
        public IHttpActionResult Assign(string storeAdminId, string managerId, string storeId)
        {
            Result result = managerService.Assign(storeAdminId, managerId, storeId);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        [Route("Unassign/{storeAdminId}")]
        public IHttpActionResult Unassign(string storeAdminId, string managerId, string storeId)
        {
            Result result = managerService.Unassign(storeAdminId, managerId, storeId);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
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

        //[HttpGet]
        //[Route("GetAllStores/{id}")]
        //public IHttpActionResult GetAllStores(string id, string sortOrder, string searchString, int pageIndex, int pageSize)
        //{
        //    using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //    {
        //        var stores = uow.Stores.GetAllApprovedByStoreAdminId(id, sortOrder, searchString);

        //        var mapped = mapper.Map<List<Store>>(stores);

        //        var subset = mapped.ToPagedList(pageIndex, pageSize);
        //        var result = new { items = subset, metaData = subset.GetMetaData() };

        //        return Ok(result);
        //    }
        //}

        //[HttpGet]
        //[Route("GetStore/{id}")]
        //public IHttpActionResult GetStore(string id, string storeId)
        //{
        //    using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //    {
        //        var store = uow.Stores.GetApprovedByStoreAdminId(id, storeId);

        //        var mapped = mapper.Map<Store>(store);

        //        return Ok(mapped);
        //    }
        //}

        [HttpPut]
        [Route("EditStore/{id}")]
        public async Task<IHttpActionResult> EditStore(string id, StoreBindingModel model)
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

        [HttpPut]
        [Route("PostStoreImage/{id}")]
        public Result PostStoreImage(string id, string storeId, byte[] image)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.PostStoreImage(storeId, image);

                uow.Complete();

                return result;
            }
        }

        [HttpGet]
        [Route("GetStoreImage/{id}")]
        public byte[] GeStoreImage(string id, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                byte[] image = uow.Stores.GetStoreImage(storeId);

                return ImageProcessor.CreateThumbnail(image);

                //return image;
            }

            //return ImageProcessor.CreateThumbnail(byteArray);
        }

        //[HttpGet]
        //[Route("SelectStore")]
        //public SelectedStore SelectStore(string id, string storeId)
        //{
        //    using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
        //    {
        //        var store = uow.Stores.GetApprovedByStoreAdminId(id, storeId);

        //        return new SelectedStore
        //        {
        //            Id = store.Id,
        //            StoreName = store.StoreName
        //        };
        //    }
        //}


        #endregion

    }
}