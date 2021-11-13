using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Manager")]
        public virtual User User { get; set; }

        public int UserId { get; set; }

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