using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pet_store.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Display(Name ="Parent")]
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Category Parent { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Product> Products { get; set; }

        public string Image { get; set; }
    }
}
