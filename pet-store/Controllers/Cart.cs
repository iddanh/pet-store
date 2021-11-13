using Microsoft.AspNetCore.Mvc;
using pet_store.Data;
using pet_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pet_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cart : ControllerBase
    {
        private readonly PetStoreDBContext _context;

        public Cart(PetStoreDBContext context)
        {
            _context = context;
        }

        // GET api/<Cart>/5
        [HttpGet("{productIds}")]
        public List<Product> Get(string productIds)
        {
            List<int> productIdsParsed = productIds.Split(',').Select(id => int.Parse(id)).ToList();
            List<Product> products = new List<Product>();
            productIdsParsed.ForEach(id => products.Add(_context.Product.Find(id)));
            return products;
        }
    }
}
