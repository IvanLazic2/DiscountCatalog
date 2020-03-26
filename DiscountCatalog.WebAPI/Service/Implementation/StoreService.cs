using AbatementHelper.WebAPI.Mapping;
using AutoMapper;
using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Extensions;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.Paging.Implementation;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.Service.Contractor;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.WebAPI.Service.Implementation
{
    public class StoreService : IService<StoreREST>, IStoreService
    {
        readonly IMapper mapper;

        public StoreService()
        {
            mapper = AutoMapping.Initialise();
        }

        public IList<StoreREST> Search(IList<StoreREST> stores, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                stores = stores.Where(s => s.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            return stores.ToList();
        }

        public IList<StoreREST> Order(IList<StoreREST> stores, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    stores = stores.OrderByDescending(s => s.StoreName).ToList();
                    break;
                default:
                    stores = stores.OrderBy(s => s.StoreName).ToList();
                    break;
            }

            return stores.ToList();
        }

        public async Task<Result> CreateAsync(StoreRESTPost store)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                StoreEntity storeEntity = mapper.Map<StoreEntity>(store);

                storeEntity.Approved = true;
                storeEntity.Deleted = false;

                Result result = await uow.Stores.CreateAsync(storeEntity, store.StoreAdminId);

                uow.Complete();

                return result;
            }
        }

        public Result Delete(string storeAdminId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.MarkAsDeleted(storeId);

                uow.Complete();

                return result;
            }
        }

        public StoreREST Get(string storeAdminId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmin = uow.StoreAdmins.GetLoadedByIdentityId(storeAdminId);

                StoreEntity store = storeAdmin.Stores
                    .FirstOrDefault(s => s.Id == storeId);

                var mapped = mapper.Map<StoreREST>(store);

                return mapped;
            }
        }

        public IPagingList<StoreREST> GetAll(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmin = uow.StoreAdmins.GetLoadedByIdentityId(storeAdminId);

                IEnumerable<StoreEntity> stores = storeAdmin.Stores
                    .Where(s => s.Approved && !s.Deleted)
                    .ToList();

                IList<StoreREST> mapped = mapper.Map<IList<StoreREST>>(stores);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<StoreREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<StoreREST> result = new PagingList<StoreREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        public IPagingList<StoreREST> GetAllDeleted(string storeAdminId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmin = uow.StoreAdmins.GetLoadedByIdentityId(storeAdminId);

                IEnumerable<StoreEntity> stores = storeAdmin.Stores
                    .Where(s => s.Deleted)
                    .ToList();

                IList<StoreREST> mapped = mapper.Map<IList<StoreREST>>(stores);

                mapped = Search(mapped, searchString);
                mapped = Order(mapped, sortOrder);

                IPagedList<StoreREST> subset = mapped.ToPagedList(pageIndex, pageSize);

                IPagingList<StoreREST> result = new PagingList<StoreREST>(subset, subset.GetMetaData());

                return result;
            }
        }

        public byte[] GetImage(string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                byte[] image = uow.Stores.GetStoreImage(storeId);

                return ImageProcessor.CreateThumbnail(image);
            }
        }

        public Result PostImage(string storeAdminId, string storeId, byte[] image)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.PostStoreImage(storeId, image);

                uow.Complete();

                return result;
            }
        }

        public Result Restore(string storeAdminId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                Result result = uow.Stores.MarkAsRestored(storeId);

                uow.Complete();

                return result;
            }
        }

        public SelectedStore Select(string storeAdminId, string storeId)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                var storeAdmin = uow.StoreAdmins.GetLoadedByIdentityId(storeAdminId);

                StoreEntity store = storeAdmin.Stores
                    .FirstOrDefault(s => s.Approved && !s.Deleted && s.Id == storeId);

                return new SelectedStore
                {
                    Id = store.Id,
                    StoreName = store.StoreName
                }; //napravit mapper za ovo
            }
        }

        public async Task<Result> UpdateAsync(string storeAdminId, StoreRESTPut store)
        {
            using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
            {
                StoreEntity storeEntity = uow.Stores.GetApproved(store.Id);

                storeEntity.StoreName = store.StoreName;
                storeEntity.WorkingHoursWeekBegin = store.WorkingHoursWeekBegin;
                storeEntity.WorkingHoursWeekEnd = store.WorkingHoursWeekEnd;
                storeEntity.WorkingHoursWeekendsBegin = store.WorkingHoursWeekendsBegin;
                storeEntity.WorkingHoursWeekendsEnd = store.WorkingHoursWeekendsEnd;
                storeEntity.WorkingHoursHolidaysBegin = store.WorkingHoursHolidaysBegin;
                storeEntity.WorkingHoursHolidaysEnd = store.WorkingHoursHolidaysEnd;
                storeEntity.Country = store.Country;
                storeEntity.City = store.City;
                storeEntity.PostalCode = store.PostalCode;
                storeEntity.Street = store.Street;

                Result result = await uow.Stores.UpdateAsync(storeEntity);

                if (result.Success)
                {
                    uow.Complete();
                }
                else
                {
                    uow.Dispose();
                }

                return result;
            }
        }
    }
}