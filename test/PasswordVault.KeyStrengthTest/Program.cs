using System.Text;
using System.Data;
using PasswordVault.core;
using PasswordVault.core.Model;
using System.Security.Cryptography;
using System;
using System.IO;

namespace PasswordVault.KeyStrengthTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: PasswordVault.KeyStrengthTest <outputFileName> <password> [userKeyFileName]");
                return;
            }

            string outputFileName = args[0];
            string password = args[1];
            string userKeyFileName = args.Length > 2 ? args[2] : null;

            // Your logic here

            Console.WriteLine($"Output File: {outputFileName}");
            Console.WriteLine($"Password: {password}");
            if (userKeyFileName != null)
            {
                Console.WriteLine($"User Key File: {userKeyFileName}");
            }

            //Read the content of the file and convert it to a byte array
            string userKey;
            string passcode;

            //if the user key file is provided, read the content of the file and convert it to a byte array
            if (userKeyFileName != null){
             
                userKey = File.ReadAllText(userKeyFileName);
                
                passcode = password + userKey;

            }else{
                passcode = password;
            }

            //Generate a 32-byte key
            byte[] key = new byte[32]; // 256 bits

            using (SHA256 sha256Hash = SHA256.Create())
            {
                key = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(passcode));
            }
            


            // Ensure the key is 32 bytes
            if (key.Length != 32)
            {
                throw new Exception("The encryption key must be 32 bytes long.");
            }

            List<Secret> passwords = new List<Secret>();

            Secret p1 = new Secret {Id=1, Username = "admin", Password = "password123", URL = "http://example.com", Notes = "This is a test password"};
            Secret p2 = new Secret {Id=2, Username = "user", Password = "letmein", URL = "http://example.org", Notes = "This is another test password"};
            

            passwords.Add(p1);
            passwords.Add(p2);

            SecretsStore encryptedCsvDataSet = new SecretsStore(key);

            encryptedCsvDataSet.CreateNewVault(outputFileName, passwords);
        }
    }
}