namespace PasswordVault.core.Model;
public class Secret
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public  string? URL { get; set; }
    public string? Notes { get; set; }


}