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
using System.Security.Cryptography;
using System.Text;

namespace pv.Pages_Vault
{
    public class CreateModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        public CreateModel(PVDB.Data.PVDBContext context)
        {
            _context = context;
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Vault.Add(Vault);
            await _context.SaveChangesAsync();

            try
            {
                byte[] key = Encoding.UTF8.GetBytes("C4rl0sS3cr3tK3y1234567890ABCDEFG");

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

                encryptedCsvDataSet.CreateNewVault(Vault.Path, passwords);


            }
            catch (System.Exception)
            {
                
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
