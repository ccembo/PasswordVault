using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Collections;

namespace PasswordVault.core;
public class SecretsStore
{
    private readonly byte[] key;
    public DataSet Data { get; private set; }
        
    public SecretsStore(byte[] key)
    {
        if (key == null || key.Length != 32) // AES-256 requires a 32-byte key
        {
            throw new ArgumentException("Key must be 32 bytes (256 bits) in length.");
        }
        this.key = key;
        Data = new DataSet();
    }

    public void CreateNewVault(string encryptedFilePath, DataSet dataSet)
    {
        Data = dataSet;
        SaveToFile(encryptedFilePath);

    }
    public void CreateNewVault<T>(string encryptedFilePath, List<T> secrets)
    {       
        SaveToFile(encryptedFilePath, secrets);

    }
    public void LoadFromFile(string encryptedFilePath)
    {
        // Read the encrypted file
        byte[] encryptedFileBytes = File.ReadAllBytes(encryptedFilePath);

        // Decrypt the data
        byte[] decryptedBytes = Decrypt(encryptedFileBytes);

        // Convert bytes to string
        string csvContent = Encoding.UTF8.GetString(decryptedBytes);

        // Load CSV into DataSet
        Data = CsvToDataSet(csvContent);
    }

    public List<T> LoadFromFileToList<T>(string encryptedFilePath)
    {
        // Read the encrypted file
        byte[] encryptedFileBytes = File.ReadAllBytes(encryptedFilePath);

        // Decrypt the data
        byte[] decryptedBytes = Decrypt(encryptedFileBytes);

        // Convert bytes to string
        string csvContent = Encoding.UTF8.GetString(decryptedBytes);

        // Load CSV into DataSet
        return CsvToList<T>(csvContent);
    }

    public void SaveToFile(string encryptedFilePath)
    {
        // Convert DataSet to CSV string
        string csvContent = DataSetToCsv(Data);

        // Convert string to bytes
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(csvContent);

        // Encrypt the data
        byte[] encryptedBytes = Encrypt(plaintextBytes);

        // Write to file
        File.WriteAllBytes(encryptedFilePath, encryptedBytes);
    }
    public void SaveToFile<T>(string encryptedFilePath, List<T> secrets)
    {
        // Convert DataSet to CSV string
        string csvContent = ListToCsv<T>(secrets);

        // Convert string to bytes
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(csvContent);

        // Encrypt the data
        byte[] encryptedBytes = Encrypt(plaintextBytes);

        // Write to file
        File.WriteAllBytes(encryptedFilePath, encryptedBytes);
    }

    private byte[] Encrypt(byte[] plaintext)
    {
        using (AesGcm aesGcm = new AesGcm(key,16))
        {
            // Generate a random nonce
            byte[] nonce = new byte[12]; // 96 bits
            RandomNumberGenerator.Fill(nonce);

            // Prepare buffers
            byte[] ciphertext = new byte[plaintext.Length];
            byte[] tag = new byte[16]; // 128 bits

            // Encrypt
            aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);

            // Output format: nonce + ciphertext + tag
            byte[] encryptedData = new byte[nonce.Length + ciphertext.Length + tag.Length];
            Buffer.BlockCopy(nonce, 0, encryptedData, 0, nonce.Length);
            Buffer.BlockCopy(ciphertext, 0, encryptedData, nonce.Length, ciphertext.Length);
            Buffer.BlockCopy(tag, 0, encryptedData, nonce.Length + ciphertext.Length, tag.Length);

            return encryptedData;
        }
    }

    private byte[] Decrypt(byte[] encryptedData)
    {
        using (AesGcm aesGcm = new AesGcm(key,16))
        {
            // Extract nonce, ciphertext, and tag
            byte[] nonce = new byte[12]; // 96 bits
            byte[] tag = new byte[16]; // 128 bits
            int ciphertextLength = encryptedData.Length - nonce.Length - tag.Length;
            byte[] ciphertext = new byte[ciphertextLength];

            Buffer.BlockCopy(encryptedData, 0, nonce, 0, nonce.Length);
            Buffer.BlockCopy(encryptedData, nonce.Length, ciphertext, 0, ciphertextLength);
            Buffer.BlockCopy(encryptedData, nonce.Length + ciphertextLength, tag, 0, tag.Length);

            byte[] plaintext = new byte[ciphertextLength];

            // Decrypt
            aesGcm.Decrypt(nonce, ciphertext, tag, plaintext);

            return plaintext;
        }
    }

    private DataSet CsvToDataSet(string csvContent)
    {
        DataSet dataSet = new DataSet();
        DataTable dataTable = new DataTable("Data");

        using (StringReader reader = new StringReader(csvContent))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            using (var dr = new CsvDataReader(csv))
            {
                dataTable.Load(dr);
            }
        }

        dataSet.Tables.Add(dataTable);
        return dataSet;
    }

    private List<T> CsvToList<T>(string csvContent)
    {
        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
        config.HasHeaderRecord = true;
        config.HeaderValidated = null;

        using (StringReader reader = new StringReader(csvContent))
        using (var csv = new CsvReader(reader, config))
        {
            var dr = csv.GetRecords<T>();
            return dr.ToList();
        }
        
    }

    private string DataSetToCsv(DataSet dataSet)
    {
        StringBuilder csvContent = new StringBuilder();

        if (dataSet.Tables.Count > 0)
        {
            DataTable table = dataSet.Tables[0];

            using (StringWriter writer = new StringWriter(csvContent))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Write headers
                foreach (DataColumn column in table.Columns)
                {
                    csv.WriteField(column.ColumnName);
                }
                csv.NextRecord();

                // Write rows
                foreach (DataRow row in table.Rows)
                {
                    foreach (var field in row.ItemArray)
                    {
                        csv.WriteField(field);
                    }
                    csv.NextRecord();
                }
            }
        }

        return csvContent.ToString();
    }

    private string ListToCsv<T>(List<T> secrets)
    {
        StringBuilder csvContent = new StringBuilder();

        if (secrets != null)
        {      
            using (StringWriter writer = new StringWriter(csvContent))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Write rows
                csv.WriteRecords(secrets);
                
            }
        }

        return csvContent.ToString();
    }
}
