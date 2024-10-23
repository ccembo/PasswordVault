using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PasswordVault.core;
using PasswordVault.core.Model;

namespace PasswordVault.WebUIServer.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly PVDB.Data.PVDBContext _context;
    
    [BindProperty]
    public PasswordVault.core.Model.User user { get; set; } = default!;
    [BindProperty]
    public string ErrorMessage { get; set; } = default!;
    
    public LoginModel(ILogger<LoginModel> logger, PVDB.Data.PVDBContext context)
    {
        _context = context;
        _logger = logger;
    }

    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPostAsync()
    {
        // if (!ModelState.IsValid)
        // {
        //     return Page();
        // }
        _logger.LogInformation("User: {0}", user);
       
        var user_in_db = await _context.User.FirstAsync(m => m.Name == user.Name);
        if (user_in_db == null)
        {
            return NotFound();
        }

        //Hash the password first
        user.Password = CryptoUtil.ComputeSha256Hash(user.Password);
        
        if (user_in_db.Password != user.Password)
        {
            return Page();
        }

        // Make sure the user's role is valid
        var role = _context.Role.FirstOrDefault(r => r.Name == user_in_db.Role);
        if (role == null)
        {
            // Handle case where the user's role is missing or invalid
            ErrorMessage = "User role is invalid!";
            return Page();
        }

        var claims = new[] {
            new Claim(ClaimTypes.Name, user_in_db.Name),
            new Claim(ClaimTypes.Role, user_in_db.Role),
            new Claim(ClaimTypes.Email, user_in_db.Email)
        };

        var identity = new ClaimsIdentity(claims, "PVScheme");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("PVScheme", principal);

        return RedirectToPage("/Index");

    }
}

