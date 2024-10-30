namespace PasswordVault.core.Model;
public class UserVault
{
    public int UserId { get; set; }
    public int VaultId { get; set; }
    public required string Role { get; set; }
    public required string VaultKey { get; set; }

    public User? User { get; set; }
    public Vault? Vault { get; set; }

}