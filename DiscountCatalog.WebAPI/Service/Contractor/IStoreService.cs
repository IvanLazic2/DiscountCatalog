using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.REST.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Service.Contractor
{
    public interface IStoreService
    {
        Task<Result> CreateAsync(StoreRESTPost store);

        StoreREST Get(string storeAdminId, string storeId);

        IPagingList<StoreREST> GetAll(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize);

        IPagingList<StoreREST> GetAllDeleted(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize);

        Task<Result> UpdateAsync(string storeAdminId, StoreRESTPut store);

        Result Delete(string storeAdminId, string storeId);

        Result Restore(string storeAdminId, string storeId);

        Result PostImage(string storeAdminId, string storeId, byte[] image);

        byte[] GetImage(string storeId);

        SelectedStore Select(string storeAdminId, string storeId);
    }
}
