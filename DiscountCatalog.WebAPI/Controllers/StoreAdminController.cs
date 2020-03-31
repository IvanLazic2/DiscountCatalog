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
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.Service.Contractor;
using DiscountCatalog.WebAPI.Service.Implementation;
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
        #region Fields

        private readonly IMapper mapper;
        private readonly IManagerService managerService;
        private readonly IStoreService storeService;

        #endregion

        #region Constructors

        public StoreAdminController()
        {
            mapper = AutoMapping.Initialise();
            managerService = new ManagerService();
            storeService = new StoreService();
        }

        #endregion

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
        [Route("GetAllManagers/{storeAdminIdentityId}")]
        public IHttpActionResult GetAllManagers(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<ManagerREST> list = managerService.GetAll(storeAdminIdentityId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllDeletedManagers/{storeAdminIdentityId}")]
        public IHttpActionResult GetAllDeletedManagers(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<ManagerREST> list = managerService.GetAllDeleted(storeAdminIdentityId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetManager/{storeAdminIdentityId}")]
        public IHttpActionResult GetManager(string storeAdminIdentityId, string managerId)
        {
            ManagerREST manager = managerService.Get(storeAdminIdentityId, managerId);

            return Ok(manager);
        }

        [HttpPut]
        [Route("EditManager/{storeAdminIdentityId}")]
        public async Task<IHttpActionResult> EditManager(string storeAdminIdentityId, ManagerRESTPut model)
        {
            Result result = await managerService.UpdateAsync(storeAdminIdentityId, model);

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
        [Route("DeleteManager/{storeAdminIdentityId}")]
        public IHttpActionResult DeleteManager(string storeAdminIdentityId, string managerId)
        {
            Result result = managerService.Delete(storeAdminIdentityId, managerId);

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
        [Route("RestoreManager/{storeAdminIdentityId}")]
        public IHttpActionResult RestoreManager(string storeAdminIdentityId, string managerId)
        {
            Result result = managerService.Restore(storeAdminIdentityId, managerId);

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
        [Route("PostManagerImage/{storeAdminIdentityId}")]
        public async Task<IHttpActionResult> PostManagerImage(string storeAdminIdentityId, string managerId, byte[] image)
        {
            Result result = await managerService.PostImageAsync(storeAdminIdentityId, managerId, image);

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
        [Route("GetManagerImage/{storeAdminIdentityId}")]
        public async Task<byte[]> GetManagerImage(string storeAdminIdentityId, string managerId)
        {
            byte[] image = await managerService.GetImageAsync(managerId);

            return image;
        }

        [HttpGet]
        [Route("GetManagerStores/{storeAdminIdentityId}")]
        public IHttpActionResult GetManagerStores(string storeAdminIdentityId, string managerId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<ManagerStore> managerStores = managerService.GetManagerStores(storeAdminIdentityId, managerId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(managerStores);
        }

        [HttpGet]
        [Route("Assign/{storeAdminIdentityId}")]
        public IHttpActionResult Assign(string storeAdminIdentityId, string managerId, string storeId)
        {
            Result result = managerService.Assign(storeAdminIdentityId, managerId, storeId);

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
        [Route("Unassign/{storeAdminIdentityId}")]
        public IHttpActionResult Unassign(string storeAdminIdentityId, string managerId, string storeId)
        {
            Result result = managerService.Unassign(storeAdminIdentityId, managerId, storeId);

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
        public async Task<IHttpActionResult> CreateStore(StoreRESTPost model)
        {
            Result result = await storeService.CreateAsync(model);

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
        [Route("GetAllStores/{storeAdminIdentityId}")]
        public IHttpActionResult GetAllStores(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<StoreREST> list = storeService.GetAll(storeAdminIdentityId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllDeletedStores/{storeAdminIdentityId}")]
        public IHttpActionResult GetAllDeletedStores(string storeAdminIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<StoreREST> list = storeService.GetAllDeleted(storeAdminIdentityId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetStore/{storeAdminIdentityId}")]
        public IHttpActionResult GetStore(string storeAdminIdentityId, string storeId)
        {
            StoreREST store = storeService.Get(storeAdminIdentityId, storeId);

            return Ok(store);
        }

        [HttpPut]
        [Route("EditStore/{storeAdminIdentityId}")]
        public async Task<IHttpActionResult> EditStore(string storeAdminIdentityId, StoreRESTPut model)
        {
            Result result = await storeService.UpdateAsync(storeAdminIdentityId, model);

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
        [Route("DeleteStore/{storeAdminIdentityId}")]
        public IHttpActionResult DeleteStore(string storeAdminIdentityId, string storeId)
        {
            Result result = storeService.Delete(storeAdminIdentityId, storeId);

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
        [Route("RestoreStore/{storeAdminIdentityId}")]
        public IHttpActionResult RestoreStore(string storeAdminIdentityId, string storeId)
        {
            Result result = storeService.Restore(storeAdminIdentityId, storeId);

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
        [Route("PostStoreImage/{storeAdminIdentityId}")]
        public IHttpActionResult PostStoreImage(string storeAdminIdentityId, string storeId, byte[] image)
        {
            Result result = storeService.PostImage(storeAdminIdentityId, storeId, image);

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
        [Route("GetStoreImage/{storeAdminIdentityId}")]
        public byte[] GeStoreImage(string storeAdminIdentityId, string storeId)
        {
            byte[] image = storeService.GetImage(storeId);

            return image;
        }

        //[HttpGet]
        //[Route("SelectStore{storeAdminId}")]
        //public IHttpActionResult SelectStore(string storeAdminId, string storeId)
        //{
        //    SelectedStore store = storeService.Select(storeAdminId, storeId);

        //    return Ok(store);
        //}

        #endregion

    }
}