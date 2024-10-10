using System.Text;
using System.Data;
using PasswordVault.core;
using Xunit;
using System.Security.Cryptography;

namespace PasswordVault.test;

public class UnitTest1
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
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string url { get; set; }
        public string notes { get; set; }
        
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

        Secret p1 = new Secret {id=1, username = "admin", password = "password123", url = "http://example.com", notes = "This is a test password"};
        Secret p2 = new Secret {id=2, username = "user", password = "letmein", url = "http://example.org", notes = "This is another test password"};
        

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

        string p1_txt = passwords[0].password ?? string.Empty;

        string p2_txt = passwords_restored[0].password ?? string.Empty;


        Assert.Equal(p1_txt,p2_txt);
    }
}