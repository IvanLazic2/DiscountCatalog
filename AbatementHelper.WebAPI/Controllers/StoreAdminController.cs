using AbatementHelper.CommonModels.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AbatementHelper.WebAPI.Controllers
{
    [Authorize(Roles = "StoreAdmin")]
    [RoutePrefix("api/StoreAdmin")]
    public class StoreAdminController : ApiController
    {
        //[HttpGet]
        //[Route("GetAllStores/{MasterStoreID}")]
        //public DataBaseResultListOfStores GetAllStores(string masterStoreID)
        //{
        //    var stores = DataBaseReader.ReadAllStoresByMasterID(masterStoreID);
        //    return stores;
        //}

        //[HttpGet]
        //[Route("GetAllDeletedStores/{MasterStoreID}")]
        //public DataBaseResultListOfStores GetAllDeletedStores(string masterStoreID)
        //{
        //    var stores = DataBaseReader.ReadDeletedStores(masterStoreID);
        //    return stores;
        //}

        //[HttpGet]
        //[Route("Edit/{id}")]
        //public DataBaseStore Edit(string id)
        //{
        //    DataBaseStore store = new DataBaseStore();

        //    store = DataBaseReader.ReadStoreById(id).Value;

        //    return store;
        //}

        //[HttpPut]
        //[Route("Edit")]
        //public IHttpActionResult Edit(DataBaseStore store)
        //{
        //    DataBaseReader.EditStore(store);

        //    return Ok(store);
        //}

        //[HttpPut]
        //[Route("Delete/{id}")]
        //public IHttpActionResult DeleteStore(string id)
        //{
        //    DataBaseReader.DeleteStore(id);
        //    return Ok("Store deleted");
        //}

        //[HttpPut]
        //[Route("Restore/{id}")]
        //public IHttpActionResult RestoreStore(string id)
        //{
        //    DataBaseReader.RestoreStore(id);
        //    return Ok("StoreRestored");
        //}
    }
}