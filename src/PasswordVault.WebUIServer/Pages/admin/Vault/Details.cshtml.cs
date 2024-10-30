using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PVDB.Data;
using PasswordVault.core.Model;

namespace pv.Pages_Vault
{
    public class DetailsModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        public DetailsModel(PVDB.Data.PVDBContext context)
        {
            _context = context;
        }

        public Vault Vault { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vault = await _context.Vault.FirstOrDefaultAsync(m => m.Id == id);
            if (vault == null)
            {
                return NotFound();
            }
            else
            {
                Vault = vault;
            }
            return Page();
        }
    }
}
