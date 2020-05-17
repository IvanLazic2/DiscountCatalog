using AbatementHelper.MVC.Mapping;
using AutoMapper;
using DiscountCatalog.MVC.Cookies.Contractor;
using DiscountCatalog.MVC.Cookies.Implementation;
using DiscountCatalog.MVC.Extensions;
using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.Processors;
using DiscountCatalog.MVC.Repositories.MVCRepositories;
using DiscountCatalog.MVC.REST.Product;
using DiscountCatalog.MVC.REST.Store;
using DiscountCatalog.MVC.REST.StoreAdmin;
using DiscountCatalog.MVC.Validators;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DiscountCatalog.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICookieHandler cookieHandler;
        private readonly ProductRepository productRepository;
        private readonly UserRepository userRepository;

        public UserController()
        {
            mapper = AutoMapping.Initialise();
            cookieHandler = new CookieHandler();
            productRepository = new ProductRepository();
            userRepository = new UserRepository();
        }

        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("GetAllProducts");
        }

        [Route("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts(string sortOrder,
                                                       string currentFilter,
                                                       string searchString,
                                                       int? page,
                                                       string currentPrice,
                                                       string priceFilter,
                                                       string currentDate,
                                                       string dateFilter,
                                                       bool includeUpcoming = false)
        {
            ViewBag.Min = Convert.ToInt32(await userRepository.GetMinPrice());
            ViewBag.Max = Convert.ToInt32(await userRepository.GetMaxPrice());

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(priceFilter))
            {
                string[] arr = priceFilter.Split(",".ToCharArray());

                ViewBag.From = Convert.ToInt32(arr[0]);
                ViewBag.To = Convert.ToInt32(arr[1]);

                page = 1;
            }
            else
            {
                priceFilter = currentPrice;

                if (currentPrice != null)
                {
                    string[] arr = currentPrice.Split(",".ToCharArray());

                    ViewBag.From = Convert.ToInt32(arr[0]);
                    ViewBag.To = Convert.ToInt32(arr[1]);
                }
                else
                {
                    ViewBag.From = ViewBag.Min;
                    ViewBag.To = ViewBag.Max;
                }
            }

            ViewBag.CurrentPrice = priceFilter;


            if (!string.IsNullOrEmpty(dateFilter))
            {
                page = 1;
            }
            else
            {
                dateFilter = currentDate;
            }

            ViewBag.CurrentDate = dateFilter;

            int pageIndex = (page ?? 1);
            int pageSize = 14;

            PagingEntity<ProductREST> products = await userRepository.GetAllProducts(sortOrder, searchString, pageIndex, pageSize, priceFilter, dateFilter, includeUpcoming);

            StaticPagedList<ProductREST> list = new StaticPagedList<ProductREST>(products.Items, products.MetaData.PageNumber, products.MetaData.PageSize, products.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("ProductDetails/{id}")]
        public async Task<ActionResult> ProductDetails(string id)
        {
            ProductREST product = await userRepository.GetProduct(id);

            if (GlobalValidator.IsProductValid(product))
            {
                return View(product);
            }

            return RedirectToAction("Index").Error("Something went wrong, please try again.");
        }

        public async Task<ActionResult> StoreAdminDetails(string id, string sortOrder, string currentFilter, string searchString, int? page)
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
            int pageSize = 4;

            StoreAdminREST storeAdmin = await userRepository.GetStoreAdmin(id);

            if (GlobalValidator.IsStoreAdminValid(storeAdmin))
            {
                storeAdmin.Stores = StoreAdminProcessor.OrderStores(storeAdmin, sortOrder);
                storeAdmin.Stores = StoreAdminProcessor.SearchStores(storeAdmin, searchString);

                storeAdmin.Stores = storeAdmin.Stores.ToPagedList(pageIndex, pageSize);

                return View(storeAdmin);
            }

            return RedirectToAction("Index").Error("Something went wrong, please try again.");
        }

        public async Task<ActionResult> StoreDetails(string id,
                                                     string sortOrder,
                                                     string currentFilter,
                                                     string searchString,
                                                     int? page,
                                                     string currentPrice,
                                                     string priceFilter,
                                                     string currentDate,
                                                     string dateFilter,
                                                     bool includeUpcoming = false)
        {
            ViewBag.Min = Convert.ToInt32(await productRepository.GetMinPrice(id));
            ViewBag.Max = Convert.ToInt32(await productRepository.GetMaxPrice(id));

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(priceFilter))
            {
                string[] arr = priceFilter.Split(",".ToCharArray());

                ViewBag.From = Convert.ToInt32(arr[0]);
                ViewBag.To = Convert.ToInt32(arr[1]);

                page = 1;
            }
            else
            {
                priceFilter = currentPrice;

                if (currentPrice != null)
                {
                    string[] arr = currentPrice.Split(",".ToCharArray());

                    ViewBag.From = Convert.ToInt32(arr[0]);
                    ViewBag.To = Convert.ToInt32(arr[1]);
                }
                else
                {
                    ViewBag.From = ViewBag.Min;
                    ViewBag.To = ViewBag.Max;
                }
            }

            ViewBag.CurrentPrice = priceFilter;


            if (!string.IsNullOrEmpty(dateFilter))
            {
                page = 1;
            }
            else
            {
                dateFilter = currentDate;
            }

            ViewBag.CurrentDate = dateFilter;

            int pageIndex = (page ?? 1);
            int pageSize = 14;

            StoreREST store = await userRepository.GetStore(id);

            if (GlobalValidator.IsStoreValid(store))
            {
                PagingEntity<ProductREST> products = await userRepository.GetStoreProducts(store.Id, sortOrder, searchString, pageIndex, pageSize, priceFilter, dateFilter, includeUpcoming);

                StaticPagedList<ProductREST> list = new StaticPagedList<ProductREST>(products.Items, products.MetaData.PageNumber, products.MetaData.PageSize, products.MetaData.TotalItemCount);

                store.Products = list;

                return View(store);
            }

            return RedirectToAction("Index").Error("Something went wrong, please try again.");
        }
    }
}