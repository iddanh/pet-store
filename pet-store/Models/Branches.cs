using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Models
{
    public class Branches
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch name is required")]
        [StringLength(30, ErrorMessage = "Maximum 30 Characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Branch city is required")]
        [StringLength(20, ErrorMessage = "Maximum 20 Characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Branch street is required")]
        [StringLength(20, ErrorMessage = "Maximum 20 Characters")]
        public string Street { get; set; }

        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Positive number required")]
        public int Apartment { get; set; }
    }
}
