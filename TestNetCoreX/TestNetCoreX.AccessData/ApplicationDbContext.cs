using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestNetCoreX.AccessData.Entities;

namespace TestNetCoreX.AccessData
{
    public class ApplicationDbContext : DbContext//IdentityDbContext<ApplicationUserX>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        public DbSet<Usuario> Usuario { get; set; }

    }
}
