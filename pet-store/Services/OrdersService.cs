using pet_store.Data;
using pet_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Services
{
    public class OrdersService
    {
        private readonly PetStoreDBContext _context;
        public OrdersService(PetStoreDBContext context)
        {
            _context = context;
        }

        public IQueryable<Order> Search(int? fromPrice, int? untilPrice, int[] usersIds, int[] productsIds, string city, DateTime? from, DateTime? until)
        {
            var res = _context.Order.AsQueryable();
            if (!String.IsNullOrEmpty(city))
            {
                res = from order in res
                      where city.Equals(order.City)
                      select order;
            }
            //if(productsIds.Length>0)
            return res;
        }
    }
}