using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;
using PVDB.Data;
using PasswordVault.core.Model;
using Microsoft.EntityFrameworkCore;
using SignalRKeyExchange.Hubs;

namespace PasswordVault.WebUIServer.Pages;

[Authorize(AuthenticationSchemes = "PVScheme", Roles = "User, Read-Only User")]
public class User_IndexModel : PageModel
{
    public IList<Vault> Vault { get;set; } = default!;
    public bool isKeyPresent { get; set; } = false;
    private readonly ILogger<User_IndexModel> _logger;
    private readonly PVDB.Data.PVDBContext _context;
    private readonly IKeyRepository _keyRepository;


    public User_IndexModel(ILogger<User_IndexModel> logger, PVDB.Data.PVDBContext context, IKeyRepository keyRepository)
    {
        _logger = logger;
        _context = context;
        _keyRepository = keyRepository;
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

        //KeyExchange 
        //Session userKey presence check
        string session_userKey = HttpContext.Session.GetString("Key");
        if (session_userKey != null)
        {
            isKeyPresent = true;
                       
        }else
        {
            string userKey = _keyRepository.Get(userName);

            if(userKey != null)
            {
                //Do something with the key
                HttpContext.Session.SetString("Key", userKey);
                isKeyPresent = true;

            }
        }
        

        
    }
}
