using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Models
{
    public class Order
    {
        [Key]
        [Display(Name = "Order Id")]

        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:$ #,0.##}")]
        [Display(Name = "Total Price")]
        public double Price { get; set; }

        public List<Product> Products { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Street { get; set; }

        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Positive number required")]
        public int Apartment { get; set; }

        [ForeignKey(nameof(User))]
        public int User { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Order Notes")]
        [StringLength(250)]
        /*[MaxLength(400)]*/
        public String OrderNotes { get; set; }
    }
}
