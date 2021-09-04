using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pet_store.Models;

namespace pet_store.Data
{
    public class pet_storeContext : DbContext
    {
        public pet_storeContext (DbContextOptions<pet_storeContext> options)
            : base(options)
        {
        }

        public DbSet<pet_store.Models.User> User { get; set; }
    }
}
