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
        #region Properties

        private readonly IMapper mapper;
        private readonly IManagerService managerService;
        public readonly IStoreService storeService;

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
        [Route("GetAllStores/{storeAdminId}")]
        public IHttpActionResult GetAllStores(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<StoreREST> list = storeService.GetAll(storeAdminId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllDeletedStores/{storeAdminId}")]
        public IHttpActionResult GetAllDeletedStores(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<StoreREST> list = storeService.GetAllDeleted(storeAdminId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetStore/{storeAdminId}")]
        public IHttpActionResult GetStore(string storeAdminId, string storeId)
        {
            StoreREST store = storeService.Get(storeAdminId, storeId);

            return Ok(store);
        }

        [HttpPut]
        [Route("EditStore/{storeAdminId}")]
        public async Task<IHttpActionResult> EditStore(string storeAdminId, StoreRESTPut model)
        {
            Result result = await storeService.UpdateAsync(storeAdminId, model);

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
        [Route("DeleteStore/{storeAdminId}")]
        public IHttpActionResult DeleteStore(string storeAdminId, string storeId)
        {
            Result result = storeService.Delete(storeAdminId, storeId);

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
        [Route("RestoreStore/{storeAdminId}")]
        public IHttpActionResult RestoreStore(string storeAdminId, string storeId)
        {
            Result result = storeService.Restore(storeAdminId, storeId);

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
        [Route("PostStoreImage/{storeAdminId}")]
        public IHttpActionResult PostStoreImage(string storeAdminId, string storeId, byte[] image)
        {
            Result result = storeService.PostImage(storeAdminId, storeId, image);

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
        [Route("GetStoreImage/{storeAdminId}")]
        public byte[] GeStoreImage(string storeAdminId, string storeId)
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