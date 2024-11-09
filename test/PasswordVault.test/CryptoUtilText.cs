using System.Text;
using System.Data;
using PasswordVault.core;
using Xunit;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PasswordVault.test;

public class CryptoUtilTest
{
    [Fact]
    public void CalculateSHA256()
    {
        //https://emn178.github.io/online-tools/sha256.html
        string hash_of_hello_txt = "185f8db32271fe25f561a6fc938b2e264306ec304eda518007d1764826381969";
        string hash_calculated_of_hello_txt;
        hash_calculated_of_hello_txt = CryptoUtil.ComputeSha256Hash("Hello");

        Assert.Equal(hash_of_hello_txt,hash_calculated_of_hello_txt);
    }

    [Fact]
    public void RSAEncryptionDecryption()
    {
        string originalMessage = "This is a secret message";
        string decryptedMessage;

        using (RSA rsa = RSA.Create(2048))
        {
            // Export the public and private keys
            RSAParameters publicKey = rsa.ExportParameters(false);
            RSAParameters privateKey = rsa.ExportParameters(true);

            
            Console.WriteLine($"Original Message: {originalMessage}");

            // Encrypt the message using the public key
            byte[] encryptedMessage = AsymmetricEncryption.EncryptMessage(rsa, originalMessage);
            Console.WriteLine($"Encrypted Message: {Convert.ToBase64String(encryptedMessage)}");

            // Decrypt the message using the private key
            decryptedMessage = AsymmetricEncryption.DecryptMessage(rsa, encryptedMessage);
            Console.WriteLine($"Decrypted Message: {decryptedMessage}");
        }

        Assert.Equal(originalMessage,decryptedMessage);
    }

    [Fact]
    public void CertEncryptionDecryption()
    {
        string originalMessage = "This is a secret message";
        string decryptedMessage;

        //Create a self-signed certificate using RSA key pair and PKCS#1 padding
        X509CertificateUtil.CreateSelfSignedCertificate("CN=TestCert", "TestCert.pfx", "password");

        //Load the certificate from the file
        using (X509Certificate2 cert = X509CertificateUtil.LoadX509Certificate("TestCert.pfx", "password"))
        {
            // Export the public and private keys
            
            RSA RSAprivateKey = cert.GetRSAPrivateKey();

            RSA RSApublicKey = cert.GetRSAPublicKey();

            Console.WriteLine($"Original Message: {originalMessage}");

            // Encrypt the message using the public key
            byte[] encryptedMessage = AsymmetricEncryption.EncryptMessage(RSApublicKey, originalMessage);
            Console.WriteLine($"Encrypted Message: {Convert.ToBase64String(encryptedMessage)}");

            // Decrypt the message using the private key
            decryptedMessage = AsymmetricEncryption.DecryptMessage(RSAprivateKey, encryptedMessage);
            Console.WriteLine($"Decrypted Message: {decryptedMessage}");
        }

        Assert.Equal(originalMessage,decryptedMessage);
    }
}