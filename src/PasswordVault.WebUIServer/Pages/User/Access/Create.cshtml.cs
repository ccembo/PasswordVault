using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PVDB.Data;
using PasswordVault.core.Model;

namespace PasswordVault.WebUIServer.Pages_User_Access
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
        ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
        ViewData["VaultId"] = new SelectList(_context.Vault, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public UserVault UserVault { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.UserVault.Add(UserVault);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
