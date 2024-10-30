using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PVDB.Data;
using PasswordVault.core.Model;

namespace PasswordVault.WebUIServer.Pages_User_Access
{
    public class DeleteModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        public DeleteModel(PVDB.Data.PVDBContext context)
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

            var uservault = await _context.UserVault.FirstOrDefaultAsync(m => m.UserId == id);

            if (uservault == null)
            {
                return NotFound();
            }
            else
            {
                UserVault = uservault;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uservault = await _context.UserVault.FindAsync(id);
            if (uservault != null)
            {
                UserVault = uservault;
                _context.UserVault.Remove(UserVault);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
