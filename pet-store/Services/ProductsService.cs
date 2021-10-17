using Microsoft.EntityFrameworkCore;
using pet_store.Data;
using pet_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Services
{
    public class ProductsService
    {
        private readonly PetStoreDBContext _context;

        public ProductsService(PetStoreDBContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Search(string searchString, string productCategory, string productCompany, double minPrice=0, double maxPrice=0)
        {
            var res = _context.Product.Include(x=>x.Category).AsQueryable<Product>();
            if (!String.IsNullOrEmpty(searchString))
            {
                res = from product in res
                      where product.Name.Contains(searchString) ||
                      product.Description.Contains(searchString)
                      select product;
            }
            if(!String.IsNullOrEmpty(productCategory))
            {
                res = from product in res
                      join category in _context.Category
                      on product.CategoryId equals category.Id
                      where category.Name.Contains(productCategory)
                      select product;
            }
            if (!String.IsNullOrEmpty(productCompany))
            {
                res = from product in res
                      where product.Company.Contains(productCompany)
                      select product;
            }
            if(minPrice > 0 && minPrice < maxPrice)
            {
                res = from product in res
                      where product.Price >= minPrice &&
                      product.Price <= maxPrice
                      select product;
            }
            if (res.Any())
            {
                foreach (Product prod in res)
                {
                    prod.Category.Products.Clear();
                }
            }
            
            return res;
        }
    }
}
