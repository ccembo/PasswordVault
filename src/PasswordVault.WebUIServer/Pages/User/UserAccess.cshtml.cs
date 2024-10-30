using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PVDB.Data;
using PasswordVault.core.Model;
using System.Text;
using PasswordVault.core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using PasswordVault.WebUIServer.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PasswordVault.WebUIServer.Pages;

[Authorize(AuthenticationSchemes = "PVScheme", Roles = "User, Read-Only User")]
public class UserAccessModel : PageModel
{
    private readonly ILogger<UserAccessModel> _logger;
    private readonly PVDB.Data.PVDBContext _context;
    public IList<Vault> Vault { get;set; } = default!;
    public IList<Role> Roles { get;set; } = default!;

    public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> RoleSelectListItems { get; set; }

    [BindProperty]
    public IList<UserVault> UserVault { get;set; } = default!;

    public UserAccessModel(ILogger<UserAccessModel> logger, PVDB.Data.PVDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task OnGetAsync(int? id)
    {
        //Get User Id
        var userName = HttpContext.User.Identity?.Name;
        if (userName == null)
        {
            Vault = new List<Vault>();
            return;
        }
        PasswordVault.core.Model.User? userId = _context.User.FirstOrDefault(u => u.Name == userName);

        if (userId == null)
        {
            Vault = new List<Vault>();
            return;
        }

        UserVault = await _context.UserVault.Where(uv => uv.UserId == userId.Id && uv.VaultId == id).ToListAsync();

        Roles = await _context.Role.ToListAsync();
        RoleSelectListItems = Roles.Select(r => new SelectListItem(r.Name, r.Name));
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
         //Get User Id
        PasswordVault.core.Model.User userId = _context.User.FirstOrDefault(u => u.Name == this.HttpContext.User.Identity.Name);

        var uservault = await _context.UserVault.Where(m => m.VaultId == id && m.UserId == userId.Id).ToListAsync();
        if (uservault == null || !uservault.Any())
        {
            return NotFound();
        }
        
        //UserVault = uservault;
        
        return RedirectToPage("./Index");
    }
}
