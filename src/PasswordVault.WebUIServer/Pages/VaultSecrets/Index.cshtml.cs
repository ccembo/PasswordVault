using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PVDB.Data;
using PasswordVault.core.Model;
using System.Text;
using PasswordVault.core;

namespace pv.Pages_VaultSecrets
{
    public class IndexModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        public IndexModel(PVDB.Data.PVDBContext context)
        {
            _context = context;
        }

        public IList<Secret> Secret { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //Find the Vault first
            if (id == null)
            {
                return NotFound();
            }

            var vault =  await _context.Vault.FirstOrDefaultAsync(m => m.Id == id);
            if (vault == null)
            {
                return NotFound();
            }
         

            byte[] key = Encoding.UTF8.GetBytes("C4rl0sS3cr3tK3y1234567890ABCDEFG");

            // Ensure the key is 32 bytes
            if (key.Length != 32)
            {
                throw new Exception("The encryption key must be 32 bytes long.");
            }

            // Create a new instance of the SecretsStore class
            SecretsStore encryptedCsvDataSet = new SecretsStore(key);

            // Load the encrypted data from the file        
            Secret = encryptedCsvDataSet.LoadFromFileToList<Secret>(vault.Path);
            return Page();
        }
    }
}
