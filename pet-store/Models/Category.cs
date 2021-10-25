using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public Category Parent { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Product> Products { get; set; }

        public string Image { get; set; }
    }
}
