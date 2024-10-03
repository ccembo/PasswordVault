using System.Text;
using System.Data;
using PasswordVault.core;
using Xunit;
using System.Security.Cryptography;

namespace PasswordVault.test;

public class UnitTest1
{
    [Fact]
    public void Test1()
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
}