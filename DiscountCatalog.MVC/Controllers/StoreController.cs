﻿using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiscountCatalog.MVC.Processors;
using DiscountCatalog.MVC.Models;
using System.Threading.Tasks;
using FluentValidation.Results;
using DiscountCatalog.MVC.Extensions;
using PagedList;
using DiscountCatalog.MVC.ViewModels;
using System.Globalization;
using DiscountCatalog.MVC.REST.Product;
using DiscountCatalog.MVC.Models.Paging;
using AutoMapper;
using AbatementHelper.MVC.Mapping;

namespace DiscountCatalog.MVC.Controllers
{
    public class StoreController : Controller //!!!dodat redirectprocessor il tak nest koji ce primat result i vracat string path
    {
        private readonly IMapper mapper;
        private readonly StoreRepository storeRepository;

        public StoreController()
        {
            storeRepository = new StoreRepository();
            mapper = AutoMapping.Initialise();
        }

        public ActionResult Index() 
        {
            return View();
        }

        //SORT BY DATE
        //by newest/oldest

        //by storename...

        [Route("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts(string sortOrder, string currentFilter, string searchString, int? page)
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

            int pageIndex = (page ?? 1);
            int pageSize = 14;

            PagingEntity<ProductREST> products = await storeRepository.GetAllProducts(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<ProductREST> list = new StaticPagedList<ProductREST>(products.Items, products.MetaData.PageNumber, products.MetaData.PageSize, products.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("CreateProduct")]
        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult> CreateProduct(ProductRESTPost product)
        {
            Result result = await storeRepository.CreateProduct(product);

            if (result == null)
            {
                return RedirectToAction("GetAllProducts");
            }

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(mapper.Map<ProductREST>(product));
            }

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("EditProduct/{id}")]
        public async Task<ActionResult> EditProduct(string id)
        {
            ProductREST product = await storeRepository.GetProduct(id);

            //Response.Cookies.Add(new HttpCookie("ProductID")
            //{
            //    Value = result.Product.Id,
            //    HttpOnly = true
            //});

            return View(product);
        }

        [HttpPost]
        [Route("EditProduct")]
        public async Task<ActionResult> EditProduct(ProductRESTPut product)
        {
            Result result = await storeRepository.EditProduct(product);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(mapper.Map<ProductREST>(product));
            }

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("ProductDetails/{id}")]
        public async Task<ActionResult> ProductDetails(string id)
        {
            ProductREST product = await storeRepository.GetProduct(id);

            //Response.Cookies.Add(new HttpCookie("ProductID")
            //{
            //    Value = result.Product.Id,
            //    HttpOnly = true
            //});

            return View(product);
        }

        [Route("PostProductImage/{id}")]
        public async Task<ActionResult> PostProductImage(string id, PostImage image)
        {
            byte[] array = ImageProcessor.GetBuffer(image.File);

            byte[] imageArray = array;

            float mb = (array.Length / 1024f) / 1024f;

            if (mb > 1)
            {
                byte[] arrayScaled = ImageProcessor.To1MB(array);

                imageArray = arrayScaled;
            }

            Result result = await storeRepository.PostProductImage(id, imageArray);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("ProductDetails", new { id });
        }

        [HttpGet]
        [Route("GetProductImage/{id}")]
        public async Task<ActionResult> GetProductImage(string id)
        {
            byte[] byteArray = await storeRepository.GetProductImage(id);

            return File(byteArray, "image/png");
        }

        [HttpGet]
        [Route("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            ProductREST product = await storeRepository.GetProduct(id);

            return View(product);
        }

        [HttpPost]
        [Route("DeleteProduct")]
        public async Task<ActionResult> DeleteProduct(ProductRESTPut product)
        {
            Result result = await storeRepository.DeleteProduct(product.Id);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(mapper.Map<ProductREST>(product));
            }

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("GetAllDeletedProducts")]
        public async Task<ActionResult> GetAllDeletedProducts(string sortOrder, string currentFilter, string searchString, int? page) //TU SAM STAO sredit view-ove, kontam si iduce napravit da se slike loadaju zajedno sa objektima
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

            int pageIndex = (page ?? 1);
            int pageSize = 14;

            PagingEntity<ProductREST> products = await storeRepository.GetAllDeletedProducts(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<ProductREST> list = new StaticPagedList<ProductREST>(products.Items, products.MetaData.PageNumber, products.MetaData.PageSize, products.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("RestoreProduct/{id}")]
        public async Task<ActionResult> RestoreProduct(string id)
        {
            Result result = await storeRepository.RestoreProduct(id);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("GetAllDeletedProducts");
        }

        [HttpGet]
        [Route("GetAllExpiredProducts")]
        public async Task<ActionResult> GetAllExpiredProducts(string sortOrder, string currentFilter, string searchString, int? page)
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

            int pageIndex = (page ?? 1);
            int pageSize = 14;

            PagingEntity<ProductREST> products = await storeRepository.GetAllExpiredProducts(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<ProductREST> list = new StaticPagedList<ProductREST>(products.Items, products.MetaData.PageNumber, products.MetaData.PageSize, products.MetaData.TotalItemCount);

            return View(list);
        }
    }
}