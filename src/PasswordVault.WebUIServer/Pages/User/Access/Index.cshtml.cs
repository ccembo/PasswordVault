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
    public class IndexModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        public IndexModel(PVDB.Data.PVDBContext context)
        {
            _context = context;
        }

        public IList<UserVault> UserVault { get;set; } = default!;

        public async Task OnGetAsync()
        {
            UserVault = await _context.UserVault
                .Include(u => u.User)
                .Include(u => u.Vault).ToListAsync();
        }
    }
}
