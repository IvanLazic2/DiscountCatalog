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
using AbatementHelper.MVC.Models;
using System.Threading.Tasks;

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
        public async Task<ActionResult> GetAllProducts()
        {
            List<WebApiProduct> products = await store.GetAllProductsAsync();

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
        public async Task<ActionResult> CreateProduct(WebApiProduct product)
        {
            Response response = await store.CreateProductAsync(product);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("EditProduct/{id}")]
        public async Task<ActionResult> EditProduct(string id)
        {
            WebApiProduct product = await store.ProductDetailsAsync(id);

            Response.Cookies.Add(new HttpCookie("ProductID")
            {
                Value = product.Id,
                HttpOnly = true
            });

            return View(product);
        }

        [HttpPost]
        [Route("EditProduct")]
        public async Task<ActionResult> EditProduct(WebApiProduct product)
        {
            Response editResponse = await store.EditProductAsync(product);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("ProductDetails/{id}")]
        public async Task<ActionResult> ProductDetails(string id)
        {
            WebApiProduct product = await store.ProductDetailsAsync(id);

            Response.Cookies.Add(new HttpCookie("ProductID")
            {
                Value = product.Id,
                HttpOnly = true
            });

            return View(product);
        }

        [Route("UploadProductImage")]
        public async Task<ActionResult> PostProductImage(PostImage image)
        {
            if (image.File != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.File.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    float mb = (array.Length / 1024f) / 1024f;

                    if (mb < 1)
                    {
                        WebApiPostImage webApiImage = new WebApiPostImage
                        {
                            Id = image.Id,
                            Image = array

                        };
                        await store.PostProductImageAsync(webApiImage);
                    }
                    else
                    {
                        byte[] arrayScaled = ImageProcessor.To1MB(array);
                        float mbScaled = (arrayScaled.Length / 1024f) / 1024f;

                        if (arrayScaled != null)
                        {
                            WebApiPostImage webApiImage = new WebApiPostImage
                            {
                                Id = image.Id,
                                Image = arrayScaled

                            };
                            await store.PostProductImageAsync(webApiImage);
                        }
                    }

                }

            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("GetProductImage/{id}")]
        public async Task<ActionResult> GetProductImage(string id)
        {
            byte[] byteArray = await store.GetProductImageAsync(id);

            return File(byteArray, "image/png");
        }

        [HttpGet]
        [Route("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            WebApiProduct product = await store.ProductDetailsAsync(id);

            return View(product);
        }

        [HttpPost]
        [Route("DeleteProduct")]
        public async Task<ActionResult> DeleteProduct(WebApiProduct product)
        {
            await store.DeleteProductAsync(product.Id);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("GetAllDeletedProducts")]
        public async Task<ActionResult> GetAllDeletedProducts()
        {
            List<WebApiProduct> products = await store.GetAllDeletedProductsAsync();

            return View(products);
        }

        [HttpGet]
        [Route("RestoreProduct/{id}")]
        public async Task<ActionResult> RestoreProduct(string id)
        {
            await store.DeleteProductAsync(id);

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        [Route("GetAllExpiredProducts")]
        public async Task<ActionResult> GetAllExpiredProducts()
        {
            List<WebApiProduct> products = await store.GetAllExpiredProductsAsync();

            return View(products);
        }
    }
}