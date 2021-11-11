using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pet_store.Models;

namespace pet_store.Data
{
    public class PetStoreDBContext : DbContext
    {
        public PetStoreDBContext (DbContextOptions<PetStoreDBContext> options)
            : base(options)
        {
        }

        public DbSet<pet_store.Models.User> User { get; set; }

        public DbSet<pet_store.Models.Category> Category { get; set; }

        public DbSet<pet_store.Models.Product> Product { get; set; }

        public DbSet<pet_store.Models.Branch> Branch { get; set; }

        public DbSet<pet_store.Models.Order> Order { get; set; }
    }
}
