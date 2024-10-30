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

namespace pv.Pages_VaultSecrets
{
    public class EditModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        public EditModel(PVDB.Data.PVDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Secret Secret { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secret =  await _context.Secret.FirstOrDefaultAsync(m => m.Id == id);
            if (secret == null)
            {
                return NotFound();
            }
            Secret = secret;
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

            _context.Attach(Secret).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecretExists(Secret.Id))
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

        private bool SecretExists(int id)
        {
            return _context.Secret.Any(e => e.Id == id);
        }
    }
}
