using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using PVDB.Data;
using PasswordVault.core.Model;
using Microsoft.EntityFrameworkCore;

namespace PasswordVault.WebUIServer.Pages;

[Authorize(AuthenticationSchemes = "PVScheme", Roles = "User, Read-Only User")]
public class User_IndexModel : PageModel
{
    private readonly ILogger<User_IndexModel> _logger;
    private readonly PVDB.Data.PVDBContext _context;
    public IList<Vault> Vault { get;set; } = default!;

    public User_IndexModel(ILogger<User_IndexModel> logger, PVDB.Data.PVDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task OnGetAsync()
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

        Vault = await _context.UserVault.AsQueryable()
            .Where(uv => uv.User == userId)
            .Select(uv => uv.Vault)
            .ToListAsync();
    }
}
