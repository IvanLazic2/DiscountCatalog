using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Repositories;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiscountCatalog.MVC.Extensions;
using System.Threading.Tasks;
using DiscountCatalog.MVC.Models;
using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.REST.Store;
using DiscountCatalog.MVC.Validators;
using DiscountCatalog.MVC.Processors;
using AutoMapper;
using AbatementHelper.MVC.Mapping;
using DiscountCatalog.MVC.Cookies.Contractor;
using DiscountCatalog.MVC.Cookies.Implementation;

namespace DiscountCatalog.MVC.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICookieHandler cookieHandler;
        private readonly ManagerRepository managerRepository;

        public ManagerController()
        {
            mapper = AutoMapping.Initialise();
            cookieHandler = new CookieHandler();
            managerRepository = new ManagerRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("GetAllStores")]
        public async Task<ActionResult> GetAllStores(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            PagingEntity<StoreREST> stores = await managerRepository.GetAllStores(sortOrder, searchString, pageNumber, pageSize);

            StaticPagedList<StoreREST> list = new StaticPagedList<StoreREST>(stores.Items, stores.MetaData.PageNumber, stores.MetaData.PageSize, stores.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("StoreDetails/{id}")]
        public async Task<ActionResult> StoreDetails(string id)
        {
            StoreREST store = await managerRepository.GetStore(id);

            if (GlobalValidator.IsStoreValid(store))
            {
                return View(store);
            }

            return RedirectToAction("GetAllStores").Error("Something went wrong, please try again.");
        }

        [HttpGet]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(string id)
        {
            StoreREST store = await managerRepository.GetStore(id);

            if (GlobalValidator.IsStoreValid(store))
            {
                return View(store);
            }

            return RedirectToAction("StoreDetails").Error("Something went wrong, please try again.");
        }

        [HttpPost]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(StoreRESTPut store)
        {
            store.StoreImage = ImageProcessor.ToValidByteArray(HttpContext.Request.Files[0]);

            Result result = await managerRepository.EditStore(store);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(mapper.Map<StoreREST>(store));
            }

            return RedirectToAction("StoreDetails", new { id = store.Id }).Success(result.SuccessMessage);
        }

        [HttpGet]
        [Route("Select/{id}")]
        public async Task<ActionResult> Select(string id)
        {
            StoreREST store = await managerRepository.GetStore(id);

            if (store != null)
            {
                cookieHandler.Set("StoreID", store.Id, true, System.Web.HttpContext.Current);
                cookieHandler.Set("StoreName", store.StoreName, true, System.Web.HttpContext.Current);

                return RedirectToAction("Index", "Store").Success($"{store.StoreName} selected.");
            }

            return RedirectToAction("GetAllStores").Error("Something went wrong, please select the store again.");

        }

        [Route("PostStoreImage/{id}")]
        public async Task<ActionResult> PostStoreImage(string id, HttpPostedFileBase file)
        {
            byte[] image = ImageProcessor.ToValidByteArray(file);

            Result result = await managerRepository.PostStoreImage(id, image);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("StoreDetails", new { id }).Success(result.SuccessMessage);
        }
    }
}