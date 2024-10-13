using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PVDB.Data;
using PasswordVault.core.Model;

namespace pv.Pages_User
{
    public class DeleteModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        public DeleteModel(PVDB.Data.PVDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User user { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                this.user = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                this.user = user;
                _context.User.Remove(this.user);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
