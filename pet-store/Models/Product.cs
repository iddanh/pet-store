using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace pet_store.Models
{
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public Category Category { get; set; }

        [Required]
        public double Price { get; set; }

        public string Company { get; set; }

        public string Image { get; set; }
    }
}
