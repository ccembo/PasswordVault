using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PasswordVault.core.Model;
using System.Text;
using PasswordVault.core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using PasswordVault.WebUIServer.Configuration;
using Microsoft.EntityFrameworkCore;

namespace pv.Pages_UserVaultSecrets
{
    [Authorize(AuthenticationSchemes = "PVScheme", Roles = "User,Admin")]
    public class CreateModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;
        private readonly ServerConfiguration _myConfig;

        public CreateModel(PVDB.Data.PVDBContext context,IOptions<ServerConfiguration> config)
        {
            _context = context;
            _myConfig = config.Value;
        }

        public IActionResult OnGet(int? id)
        {
            return Page();
        }

        [BindProperty]
        public Secret Secret { get; set; } = default!;
        public Vault Vault { get;set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

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

            //Get User Id
            User userId = _context.User.FirstOrDefault(u => u.Name == this.HttpContext.User.Identity.Name);

            UserVault userVault = _context.UserVault.FirstOrDefault(uv => uv.UserId == userId.Id && uv.VaultId == vault.Id);

            byte[] key = Convert.FromBase64String(userVault.VaultKey);

            // Ensure the key is 32 bytes
            if (key.Length != 32)
            {
                throw new Exception("The encryption key must be 32 bytes long.");
            }

            string vaultPath = _myConfig.VaultStoragePath + "\\" + vault.Path;

            // Create a new instance of the SecretsStore class
            SecretsStore encryptedCsvDataSet = new SecretsStore(key);

            // Load the encrypted data from the file        
            var secrets = encryptedCsvDataSet.LoadFromFileToList<Secret>(vaultPath);

            int max_id = secrets.Max(s => s.Id);

            Secret.Id = max_id + 1;

            secrets.Add(Secret);

            encryptedCsvDataSet.SaveToFile<Secret>(vaultPath, secrets );

            return RedirectToPage("./Secrets", new { id = id });
        }
    }
}
