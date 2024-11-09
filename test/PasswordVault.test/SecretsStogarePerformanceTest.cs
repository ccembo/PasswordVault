using System.Text;
using System.Data;
using PasswordVault.core;
using Xunit;
using System.Security.Cryptography;
using PasswordVault.core.Model;

namespace PasswordVault.test;

public class SecretsStogarePerformanceTest
{
    [Fact]
    public void CreateNewVaultWith100kSecrets()
    {
        // Generate or obtain a 32-byte encryption key
        byte[] key = Encoding.UTF8.GetBytes("C4rl0sS3cr3tK3y1234567890ABCDEFG");

        // Ensure the key is 32 bytes
        if (key.Length != 32)
        {
            throw new Exception("The encryption key must be 32 bytes long.");
        }

        List<Secret> passwords = new List<Secret>();

        for (int i = 0; i < 100000; i++)
        {
            Secret p = new Secret {Id=i, Username = "user"+i, Password = "letmein"+i, URL = "http://example.org", Notes = "This is another test password"};
            passwords.Add(p);
        }
        

        SecretsStore encryptedCsvDataSet = new SecretsStore(key);

        encryptedCsvDataSet.CreateNewVault("encryptedList100K.csv", passwords);

        // Load data from encrypted file
        List<Secret> passwords_restored;
        
        passwords_restored = encryptedCsvDataSet.LoadFromFileToList<Secret>("encryptedList100K.csv");

        // Access the DataSet
        

        // Modify the DataSet as needed
        // ...

        // Access the middle of the both lists

        string p1_txt = passwords[50000].Password ?? string.Empty;

        string p2_txt = passwords_restored[50000].Password ?? string.Empty;


        Assert.Equal(p1_txt,p2_txt);
    }
}