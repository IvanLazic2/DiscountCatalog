﻿//using DiscountCatalog.Common.Models;
//using DiscountCatalog.Common.WebApiModels;
//using DiscountCatalog.MVC.Repositories;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DiscountCatalog.MVC.Processors;
//using DiscountCatalog.MVC.Models;
//using System.Threading.Tasks;
//using FluentValidation.Results;
//using DiscountCatalog.MVC.Extensions;
//using PagedList;
//using DiscountCatalog.MVC.ViewModels;
//using System.Globalization;

//namespace DiscountCatalog.MVC.Controllers
//{
//    public class StoreController : Controller
//    {
//        private StoreRepository store = new StoreRepository();

//        public ActionResult Index()
//        {
//            return View();
//        }

//        //SORT BY DATE
//        //by newest/oldest
//        //by 

//        [Route("GetAllProducts")]
//        public async Task<ActionResult> GetAllProducts(string sortOrder, string CurrentFilter, string searchString, int? page)
//        {
//            ViewBag.CurrentSort = sortOrder;

//            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
//            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";

//            if (searchString != null)
//            {
//                page = 1;
//            }
//            else
//            {
//                searchString = CurrentFilter;
//            }

//            ViewBag.CurrentFilter = searchString;

//            WebApiListOfProductsResult result = await store.GetAllProductsAsync();

//            if (!result.Success)
//            {
//                foreach (var error in result.ModelState)
//                {
//                    ModelState.AddModelError(error.Key, error.Value);
//                }
//            }

//            List<WebApiProduct> products = result.Products;

//            if (!string.IsNullOrEmpty(searchString))
//            {
//                products = products.Where(u => u.ProductName.Contains(searchString, StringComparer.OrdinalIgnoreCase) ||
//                                               u.CompanyName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
//            }

//            switch (sortOrder)
//            {
//                case "name_desc":
//                    products = products.OrderByDescending(p => p.ProductName).ToList();
//                    break;
//                case "price":
//                    products = products.OrderBy(p => p.ProductNewPrice).ToList();
//                    break;
//                case "price_desc":
//                    products = products.OrderByDescending(p => p.ProductNewPrice).ToList();
//                    break;
//                case "percentage":
//                    products = products.OrderBy(p => p.DiscountPercentage).ToList();
//                    break;
//                case "percentage_desc":
//                    products = products.OrderByDescending(p => p.DiscountPercentage).ToList();
//                    break;
//                default:
//                    products = products.OrderBy(p => p.ProductName).ToList();
//                    break;
//            }

//            int pageSize = 15;
//            int pageNumber = (page ?? 1);

//            return View(products.ToPagedList(pageNumber, pageSize));
//        }

//        [HttpGet]
//        [Route("CreateProduct")]
//        public ActionResult CreateProduct()
//        {
//            return View();
//        }

//        [HttpPost]
//        [Route("CreateProduct")]
//        public async Task<ActionResult> CreateProduct(WebApiProduct product)
//        {
//            WebApiResult result = await store.CreateProductAsync(product);

//            if (result == null)
//            {
//                return RedirectToAction("GetAllProducts");
//            }

//            if (!result.Success)
//            {
//                foreach (var error in result.ModelState)
//                {
//                    ModelState.AddModelError(error.Key, error.Value);
//                }

//                return View(product);
//            }

//            return RedirectToAction("GetAllProducts");
//        }

//        [HttpGet]
//        [Route("EditProduct/{id}")]
//        public async Task<ActionResult> EditProduct(string id)
//        {
//            WebApiProductResult result = await store.ProductDetailsAsync(id);

//            Response.Cookies.Add(new HttpCookie("ProductID")
//            {
//                Value = result.Product.Id,
//                HttpOnly = true
//            });

//            return View(result.Product);
//        }

//        [HttpPost]
//        [Route("EditProduct")]
//        public async Task<ActionResult> EditProduct(WebApiProduct product)
//        {
//            WebApiProductResult result = await store.EditProductAsync(product);

//            if (!result.Success)
//            {
//                foreach (var error in result.ModelState)
//                {
//                    ModelState.AddModelError(error.Key, error.Value);
//                }

//                return View(product);
//            }

//            return RedirectToAction("GetAllProducts");
//        }

//        [HttpGet]
//        [Route("ProductDetails/{id}")]
//        public async Task<ActionResult> ProductDetails(string id)
//        {
//            WebApiProductResult result = await store.ProductDetailsAsync(id);

//            Response.Cookies.Add(new HttpCookie("ProductID")
//            {
//                Value = result.Product.Id,
//                HttpOnly = true
//            });

//            return View(result.Product);
//        }

//        [Route("UploadProductImage")]
//        public async Task<ActionResult> PostProductImage(PostImage image)
//        {
//            if (image.File != null)
//            {
//                using (MemoryStream ms = new MemoryStream())
//                {
//                    image.File.InputStream.CopyTo(ms);
//                    byte[] array = ms.GetBuffer();

//                    float mb = (array.Length / 1024f) / 1024f;

//                    if (mb < 1)
//                    {
//                        WebApiPostImage webApiImage = new WebApiPostImage
//                        {
//                            Id = image.Id,
//                            Image = array

//                        };

//                        WebApiResult result = await store.PostProductImageAsync(webApiImage);
//                    }
//                    else
//                    {
//                        byte[] arrayScaled = ImageProcessor.To1MB(array);
//                        float mbScaled = (arrayScaled.Length / 1024f) / 1024f;

//                        if (arrayScaled != null)
//                        {
//                            WebApiPostImage webApiImage = new WebApiPostImage
//                            {
//                                Id = image.Id,
//                                Image = arrayScaled

//                            };

//                            WebApiResult result =  await store.PostProductImageAsync(webApiImage);
//                        }
//                    }
//                }
//            }

//            return RedirectToAction("Index", "Home");
//        }

//        [HttpGet]
//        [Route("GetProductImage/{id}")]
//        public async Task<ActionResult> GetProductImage(string id)
//        {
//            byte[] byteArray = await store.GetProductImageAsync(id);

//            return File(byteArray, "image/png");
//        }

//        [HttpGet]
//        [Route("DeleteProduct/{id}")]
//        public async Task<ActionResult> DeleteProduct(string id)
//        {
//            WebApiProductResult result = await store.ProductDetailsAsync(id);

//            return View(result.Product);
//        }

//        [HttpPost]
//        [Route("DeleteProduct")]
//        public async Task<ActionResult> DeleteProduct(WebApiProduct product)
//        {
//            await store.DeleteProductAsync(product.Id);

//            return RedirectToAction("GetAllProducts");
//        }

//        [HttpGet]
//        [Route("GetAllDeletedProducts")]
//        public async Task<ActionResult> GetAllDeletedProducts()
//        {
//            WebApiListOfProductsResult result = await store.GetAllDeletedProductsAsync();

//            return View(result.Products);
//        }

//        [HttpGet]
//        [Route("RestoreProduct/{id}")]
//        public async Task<ActionResult> RestoreProduct(string id)
//        {
//            await store.DeleteProductAsync(id);

//            return RedirectToAction("GetAllProducts");
//        }

//        [HttpGet]
//        [Route("GetAllExpiredProducts")]
//        public async Task<ActionResult> GetAllExpiredProducts()
//        {
//            WebApiListOfProductsResult result = await store.GetAllExpiredProductsAsync();

//            return View(result.Products);
//        }
//    }
//}