using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PVDB.Data;
using PasswordVault.core.Model;

namespace pv.Pages_VaultSecrets
{
    public class DetailsModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        public DetailsModel(PVDB.Data.PVDBContext context)
        {
            _context = context;
        }

        public Secret Secret { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secret = await _context.Secret.FirstOrDefaultAsync(m => m.Id == id);
            if (secret == null)
            {
                return NotFound();
            }
            else
            {
                Secret = secret;
            }
            return Page();
        }
    }
}
