using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbatementHelper.MVC.Processors;

namespace AbatementHelper.MVC.Controllers
{
    public class StoreController : Controller
    {
        private StoreRepository store = new StoreRepository();

        public ActionResult Index()
        {
            return View();
        }

        [Route("GetAllProducts")]
        public ActionResult GetAllProducts()
        {
            List<WebApiProduct> products = store.GetAllProducts();

            return View(products);
        }

        [HttpGet]
        [Route("CreateProduct")]
        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateProduct")]
        public ActionResult CreateProduct(WebApiProduct product)
        {
            Response response = store.CreateProduct(product);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("EditProduct/{id}")]
        public ActionResult EditProduct(string id)
        {
            WebApiProduct product = store.ProductDetails(id);

            Response.Cookies.Add(new HttpCookie("ProductID")
            {
                Value = product.Id,
                HttpOnly = true
            });

            return View(product);
        }

        [HttpPost]
        [Route("EditProduct")]
        public ActionResult EditProduct(WebApiProduct product)
        {
            Response editResponse = store.EditProduct(product);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("ProductDetails/{id}")]
        public ActionResult ProductDetails(string id)
        {
            WebApiProduct product = store.ProductDetails(id);

            Response.Cookies.Add(new HttpCookie("ProductID")
            {
                Value = product.Id,
                HttpOnly = true
            });

            return View(product);
        }

        [Route("UploadProductImage")]
        public ActionResult PostProductImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    float mb = (array.Length / 1024f) / 1024f;

                    if (mb < 1)
                    {
                        store.PostProductImage(array);
                    }
                    else
                    {
                        byte[] arrayScaled = ImageProcessor.To1MB(array);
                        float mbScaled = (arrayScaled.Length / 1024f) / 1024f;

                        if (arrayScaled != null)
                        {
                            store.PostProductImage(arrayScaled);
                        }
                    }

                }

            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("GetProductImage/{id}")]
        public ActionResult GetProductImage(string id)
        {
            byte[] byteArray = store.GetProductImage(id);

            return File(byteArray, "image/png");
        }

        [HttpGet]
        [Route("DeleteProduct/{id}")]
        public ActionResult DeleteProduct(string id)
        {
            WebApiProduct product = store.ProductDetails(id);

            return View(product);
        }

        [HttpPost]
        [Route("DeleteProduct")]
        public ActionResult DeleteProduct(WebApiProduct product)
        {
            store.DeleteProduct(product.Id);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("GetAllDeletedProducts")]
        public ActionResult GetAllDeletedProducts()
        {
            List<WebApiProduct> products = store.GetAllDeletedProducts();

            return View(products);
        }

        [HttpGet]
        [Route("RestoreProduct/{id}")]
        public ActionResult RestoreProduct(string id)
        {
            store.DeleteProduct(id);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("GetAllExpiredProducts")]
        public ActionResult GetAllExpiredProducts()
        {
            List<WebApiProduct> products = store.GetAllExpiredProducts();

            return View(products);
        }
    }
}