using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PVDB.Data;
using PasswordVault.core.Model;
using System.Text;
using PasswordVault.core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using PasswordVault.WebUIServer.Configuration;

namespace PasswordVault.WebUIServer.Pages_User_Access
{
    [Authorize(AuthenticationSchemes = "PVScheme", Roles = "User,Admin, Read-only User")]
    public class EditModel : PageModel
    {       
        private readonly PVDB.Data.PVDBContext _context;

        public EditModel(PVDB.Data.PVDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserVault UserVault { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

             //Get User Id
            User userId = _context.User.FirstOrDefault(u => u.Name == this.HttpContext.User.Identity.Name);

            var uservault =  await _context.UserVault.FirstOrDefaultAsync(m => m.VaultId == id && m.UserId == userId.Id);
            if (uservault == null)
            {
                return NotFound();
            }
            UserVault = uservault;
           ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
           ViewData["VaultId"] = new SelectList(_context.Vault, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserVault).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserVaultExists(UserVault.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserVaultExists(int id)
        {
            return _context.UserVault.Any(e => e.UserId == id);
        }
    }
}
