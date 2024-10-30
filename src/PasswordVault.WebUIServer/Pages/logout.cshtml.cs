using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PasswordVault.core;
using PasswordVault.core.Model;

namespace PasswordVault.WebUIServer.Pages;

public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;
    private readonly PVDB.Data.PVDBContext _context;
    
    [BindProperty]
    public PasswordVault.core.Model.User user { get; set; } = default!;
    [BindProperty]
    public string ErrorMessage { get; set; } = default!;
    
    public LogoutModel(ILogger<LogoutModel> logger, PVDB.Data.PVDBContext context)
    {
        _context = context;
        _logger = logger;
    }

    public void OnGet()
    {
        _logger.LogInformation("User: {0} Logout", user);
       
        

        this.HttpContext.SignOutAsync();

       
    }
    public async Task<IActionResult> OnPostAsync()
    {
        // if (!ModelState.IsValid)
        // {
        //     return Page();
        // }
         return RedirectToPage("/Index");
        
    }
}

