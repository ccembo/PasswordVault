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

        public PVDBContext(Func<object, object> value)
        {
            Value = value;
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<Vault> Vault { get; set; } = default!;
        public DbSet<Secret> Secret { get; set; } = default!;
        public DbSet<UserVault> UserVault { get; set; } = default!;
        public Func<object, object> Value { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("PasswordVault.core.Model.UserVault", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VaultId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "VaultId");

                    b.HasIndex("VaultId");

                    b.ToTable("UserVault");
                });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User" },
                new Role { Id = 2, Name = "Read-Only User" },
                new Role { Id = 3, Name = "Admin" }
            );  

            User[] users = new User[] {
                new User { Id = 1, Name = "admin", Email = ""
                , Password = CryptoUtil.ComputeSha256Hash("admin")
                , Role = "Admin"
                , Token = ""    },
                new User { Id = 2, Name = "user", Email = ""
                , Password = CryptoUtil.ComputeSha256Hash("user123")
                , Role = "User"
                , Token = ""    }
            };

            modelBuilder.Entity<User>().HasData(users);
        }
    }
}