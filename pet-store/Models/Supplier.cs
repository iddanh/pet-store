using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pet_store.Models
{
    public class Supplier
    {
        [ForeignKey(nameof(User))]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyId { get; set; }
    }
}
