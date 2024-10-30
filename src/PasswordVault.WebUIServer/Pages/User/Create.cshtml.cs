using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PVDB.Data;
using System.Data;
using PasswordVault.core;
using PasswordVault.core.Model;
using  PasswordVault.WebUIServer.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace pv.Pages_User
{ [Authorize(AuthenticationSchemes = "PVScheme", Roles = "User,Admin")]
    public class CreateVaultModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        private readonly ServerConfiguration _myConfig;

        public CreateVaultModel(PVDB.Data.PVDBContext context, IOptions<ServerConfiguration> config)
        {
            _context = context;
            _myConfig = config.Value;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Vault Vault { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            byte[] key = new byte[32]; // 256 bits
            string keyBase64 = string.Empty;

            //Generate a 32-byte key                
            RandomNumberGenerator.Fill(key);
            
            //Convert the key to Base64  
            keyBase64 = Convert.ToBase64String(key);

            //Get User Id
            User userId = _context.User.FirstOrDefault(u => u.Name == this.HttpContext.User.Identity.Name);

            
            _context.Vault.Add(Vault);
          

            UserVault userVault = new UserVault { User = userId, Vault = Vault,  VaultKey = keyBase64, Role = "Admin" };

            _context.UserVault.Add(userVault);

            await _context.SaveChangesAsync();
/*
            var customer = new Customer(); //no Id yet;
            var order = new Order{Customer = customer}; 
            context.Orders.Add(order);
            context.SaveChanges();

*/
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                

                // Ensure the key is 32 bytes
                if (key.Length != 32)
                {
                    throw new Exception("The encryption key must be 32 bytes long.");
                }
                List<Secret> passwords = new List<Secret>();

                Secret p1 = new Secret {Id=1, Username = "admin", Password = "password123", URL = "http://example.com", Notes = "This is a test password"};
                Secret p2 = new Secret {Id=2, Username = "user", Password = "letmein", URL = "http://example.org", Notes = "This is another test password"};
        

                passwords.Add(p1);
                passwords.Add(p2);

                SecretsStore encryptedCsvDataSet = new SecretsStore(key);
                string vaultPath = _myConfig.VaultStoragePath + "\\" + Vault.Path;

                encryptedCsvDataSet.CreateNewVault(vaultPath, passwords);


            }
            catch (System.Exception)
            {
                
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
