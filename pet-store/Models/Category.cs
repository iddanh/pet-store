using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Models
{
    public class Category
    {
        public int Id { get; set; }

        public Category Parent { get; set; }

        public string Name { get; set; }
    }
}
