using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }
    }
}
