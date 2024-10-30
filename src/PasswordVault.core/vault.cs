namespace PasswordVault.core.Model;
public class Vault
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Path { get; set; }
    public string? noce { get; set; }
    public string? tag { get; set; }
    public ICollection<UserVault>? VaultsAllocated { get; set; }

}