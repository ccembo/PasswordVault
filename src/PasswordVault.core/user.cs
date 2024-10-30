namespace PasswordVault.core.Model;
public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
    public string? Token { get; set; }

    public ICollection<UserVault>? VaultsAllocated { get; set; }
}