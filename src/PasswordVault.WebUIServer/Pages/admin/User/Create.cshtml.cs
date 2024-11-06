using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PVDB.Data;
using PasswordVault.core;
using PasswordVault.core.Model;
using Microsoft.AspNetCore.Authorization;

namespace pv.Pages_User
{[Authorize(AuthenticationSchemes = "PVScheme", Roles = "Admin")]
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
        public User user { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Hash the password first
            user.Password = CryptoUtil.ComputeSha256Hash(user.Password);

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
