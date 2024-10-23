using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasswordVault.core;
using PasswordVault.core.Model;

namespace PVDB.Data
{
    public class PVDBContext : DbContext
    {
        public PVDBContext (DbContextOptions<PVDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<Vault> Vault { get; set; } = default!;
        public DbSet<Secret> Secret { get; set; } = default!;
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User" },
                new Role { Id = 2, Name = "Read-Only User" },
                new Role { Id = 3, Name = "Admin" }
            );  

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "admin", Email = ""
                , Password = CryptoUtil.ComputeSha256Hash("admin")
                , Role = "Admin"
                , Token = ""    });
        }
    }
}
