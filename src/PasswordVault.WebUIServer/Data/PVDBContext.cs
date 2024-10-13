using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Vault> Vault { get; set; } = default!;
        public DbSet<Secret> Secret { get; set; } = default!;
    }
}
