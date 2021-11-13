using Microsoft.EntityFrameworkCore;
using pet_store.Data;
using pet_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pet_store.Services
{
    public class ProductsService
    {
        private readonly PetStoreDBContext _context;

        public ProductsService(PetStoreDBContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Search(string searchString, string productCategory, string productCompany, string brands, double minPrice = 0, double maxPrice = 0)
        {
            if(minPrice > maxPrice)
            {
                var temp = minPrice;
                minPrice = maxPrice;
                maxPrice = minPrice;
            }
            var res = _context.Product.Include(x => x.Category).AsQueryable<Product>();
            if (!String.IsNullOrEmpty(searchString))
            {
                res = from product in res
                      where product.Name.Contains(searchString) ||
                      product.Description.Contains(searchString)
                      select product;
            }
            if (!String.IsNullOrEmpty(productCategory))
            {
                res = from product in res
                      join category in _context.Category
                      on product.CategoryId equals category.Id
                      where category.Name.Contains(productCategory)
                      select product;
            }
            if (!String.IsNullOrEmpty(productCompany) && String.IsNullOrEmpty(brands))
            {
                res = from product in res
                      where product.Company.Contains(productCompany)
                      select product;
            }
            if(!String.IsNullOrEmpty(brands))
            {
                res = from product in res
                      where brands.Contains(product.Company)
                      select product;
            }
            if (minPrice >= 0 && minPrice < maxPrice)
            {
                res = from product in res
                      where product.Price >= minPrice &&
                      product.Price <= maxPrice+1
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

        public IEnumerable<Category> GetBreadCrumbs(Category category)
        {
            var breadCrumbs = new List<Category>();
            var parent = category;
            while (parent != null)
            {
                breadCrumbs.Add(parent);
                parent = _context.Category.Find(parent.ParentId);
            }
            breadCrumbs.Reverse();
            return breadCrumbs;
        }
    }
}