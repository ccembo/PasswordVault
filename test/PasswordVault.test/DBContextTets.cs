using System.Text;
using System.Data;
using PasswordVault.core;
using PasswordVault.core.Model;
using PasswordVault.WebUIServer;
using Xunit;
using System.Security.Cryptography;
using PVDB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;


namespace PasswordVault.test;

public class DBContextTets
{
    [Fact]
    public void SaveUsersAdnVaults()
    {
        var options = new DbContextOptionsBuilder<PVDBContext>()
            .UseInMemoryDatabase(databaseName: "MovieListDatabase")
            .Options;

        PVDBContext context = new PVDBContext(options);

        User user1 = new User { Name = "admin", Email = ""
            , Password = CryptoUtil.ComputeSha256Hash("admin")
            , Role = "Admin"
            , Token = ""    };

        User user2 = new User { Name = "user", Email = ""    
            , Password = CryptoUtil.ComputeSha256Hash("user123") ,
            Role = "User"
            , Token = ""      };

        Vault vault1 = new Vault { Name = "Vault1", Path = "Vault1 Description" };
        Vault vault2 = new Vault { Name = "Vault2", Path = "Vault2 Description" };


        context.User.Add(user1);
        context.User.Add(user2);

        context.Vault.Add(vault1);
        context.Vault.Add(vault2);

        UserVault userVault1 = new UserVault { UserId = user1.Id, VaultId = vault2.Id
        , VaultKey = "Key", Role = "Admin" };  

        context.UserVault.Add(userVault1);

        context.SaveChanges();

        User user1_db = context.User.Find(1);
        User user2_db = context.User.Find(2);
        UserVault userVault1_db = context.UserVault.First();

        Assert.Equal(user1_db.Password,CryptoUtil.ComputeSha256Hash("admin"));
    }
}