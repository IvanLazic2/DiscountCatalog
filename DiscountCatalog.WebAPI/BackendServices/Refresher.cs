using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace DiscountCatalog.WebAPI.BackendServices
{
    public class Refresher
    {
        public static async void Refresh()
        {
            while (true)
            {
                using (var uow = new UnitOfWork(new ApplicationUserDbContext()))
                {
                    try
                    {
                        var products = uow.Products.GetAll();

                        foreach (var product in products)
                        {
                            if (DateTime.Parse(product.DiscountDateEnd).CompareTo(DateTime.Now) < 1)
                            {
                                uow.Products.MarkAsExpired(product.Id);
                            }
                            else
                            {
                                uow.Products.MarkAsActive(product.Id);
                            }
                        }
                    }
                    catch (Exception exc)
                    {

                        throw;
                    }
                }

                await Task.Delay(1000); /*3600000*/
            }
        }
    }
}