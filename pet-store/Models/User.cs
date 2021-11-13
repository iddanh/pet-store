using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Models
{
    public enum UserType
    {
        Customer,
        Supplier,
        Admin
    }

    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        public UserType Type { get; set; } = UserType.Customer;

        public DateTime RegisterTime { get; set; }

        public virtual Branch Branch { get; set; }

        [DisplayName("Company Name")] 
        public string CompanyName { get; set; }

        [DisplayName("Company Id")] 
        public string CompanyId { get; set; }

    }
}
