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
    public class IndexModel : PageModel
    {
        private readonly PVDB.Data.PVDBContext _context;

        public IndexModel(PVDB.Data.PVDBContext context)
        {
            _context = context;
        }

        public IList<User> Index_user { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Index_user = await _context.User.ToListAsync();
        }
    }
}
