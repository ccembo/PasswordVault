using System.Text;
using System.Data;
using PasswordVault.core;
using Xunit;
using System.Security.Cryptography;

namespace PasswordVault.test;

public class SecretsStorageTest
{
    [Fact]
    public void SecretStoreWithDataTables()
    {
        // Generate or obtain a 32-byte encryption key
        byte[] key = Encoding.UTF8.GetBytes("C4rl0sS3cr3tK3y1234567890ABCDEFG");

        // Ensure the key is 32 bytes
        if (key.Length != 32)
        {
            throw new Exception("The encryption key must be 32 bytes long.");
        }
        DataSet dataSet = new DataSet();
        DataTable table = new DataTable("Passwords");   
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Username", typeof(string));
        table.Columns.Add("Password", typeof(string));
        table.Columns.Add("URL", typeof(string));
        table.Columns.Add("Notes", typeof(string));

        table.Rows.Add(1, "admin", "password123", "http://example.com", "This is a test password");
        table.Rows.Add(2, "user", "letmein", "http://example.org", "This is another test password");

        dataSet.Tables.Add(table);

        SecretsStore encryptedCsvDataSet = new SecretsStore(key);

        encryptedCsvDataSet.CreateNewVault("encryptedData.csv", dataSet);

        // Load data from encrypted file
        encryptedCsvDataSet.LoadFromFile("encryptedData.csv");

        // Access the DataSet
        DataSet data = encryptedCsvDataSet.Data;

        // Modify the DataSet as needed
        // ...

        // Save the DataSet back to an encrypted file
        //encryptedCsvDataSet.SaveToFile("encryptedData.csv");

        string p1 = data.Tables[0].Rows[0]["Password"]?.ToString() ?? string.Empty;

        string p2 = dataSet.Tables[0].Rows[0]["Password"].ToString() ?? string.Empty;


        Assert.Equal(p1,p2);
    }

    class Secret
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public  string? Url { get; set; }
        public string? Notes { get; set; }
        
    }

    [Fact]
    public void SecretStoreWithListOFObjects()
    {
        // Generate or obtain a 32-byte encryption key
        byte[] key = Encoding.UTF8.GetBytes("C4rl0sS3cr3tK3y1234567890ABCDEFG");

        // Ensure the key is 32 bytes
        if (key.Length != 32)
        {
            throw new Exception("The encryption key must be 32 bytes long.");
        }

        List<Secret> passwords = new List<Secret>();

        Secret p1 = new Secret {Id=1, Username = "admin", Password = "password123", Url = "http://example.com", Notes = "This is a test password"};
        Secret p2 = new Secret {Id=2, Username = "user", Password = "letmein", Url = "http://example.org", Notes = "This is another test password"};
        

        passwords.Add(p1);
        passwords.Add(p2);

        SecretsStore encryptedCsvDataSet = new SecretsStore(key);

        encryptedCsvDataSet.CreateNewVault("encryptedList.csv", passwords);

        // Load data from encrypted file
        List<Secret> passwords_restored;
        
        passwords_restored = encryptedCsvDataSet.LoadFromFileToList<Secret>("encryptedList.csv");

        // Access the DataSet
        

        // Modify the DataSet as needed
        // ...

        // Save the DataSet back to an encrypted file
        //encryptedCsvDataSet.SaveToFile("encryptedData.csv");

        string p1_txt = passwords[0].Password ?? string.Empty;

        string p2_txt = passwords_restored[0].Password ?? string.Empty;


        Assert.Equal(p1_txt,p2_txt);
    }


       [Fact]
    public void SecretStoreWithListOFObjectsBase46Key()
    {
        //Generate a 32-byte key
        byte[] key = new byte[32]; // 256 bits
        
        string passcode = "Password123456";
        
        using (SHA256 sha256Hash = SHA256.Create())
        {
            key = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(passcode));
        }
        

        //Convert the key to Base64  
        string keyBase64 = Convert.ToBase64String(key);

        // Ensure the key is 32 bytes
        if (key.Length != 32)
        {
            throw new Exception("The encryption key must be 32 bytes long.");
        }

        List<Secret> passwords = new List<Secret>();

        Secret p1 = new Secret {Id=1, Username = "admin", Password = "password123", Url = "http://example.com", Notes = "This is a test password"};
        Secret p2 = new Secret {Id=2, Username = "user", Password = "letmein", Url = "http://example.org", Notes = "This is another test password"};
        

        passwords.Add(p1);
        passwords.Add(p2);

        SecretsStore encryptedCsvDataSet = new SecretsStore(key);

        encryptedCsvDataSet.CreateNewVault("encryptedList2.csv", passwords);

        // Load data from encrypted file
        List<Secret> passwords_restored;
        
        //Restore the key from a Base64 string
        byte[] keyFromBase64 = Convert.FromBase64String(keyBase64);

        SecretsStore encryptedCsvDataSet_new = new SecretsStore(keyFromBase64);
        passwords_restored = encryptedCsvDataSet_new.LoadFromFileToList<Secret>("encryptedList2.csv");

        // Access the DataSet
        

        // Modify the DataSet as needed
        // ...

        // Save the DataSet back to an encrypted file
        //encryptedCsvDataSet.SaveToFile("encryptedData.csv");

        string p1_txt = passwords[0].Password ?? string.Empty;

        string p2_txt = passwords_restored[0].Password ?? string.Empty;


        Assert.Equal(p1_txt,p2_txt);
    }
}