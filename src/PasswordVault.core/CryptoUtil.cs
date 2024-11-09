using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PasswordVault.core
{
    public class CryptoUtil
    {
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256 instance
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string of hex characters
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

    public static class AsymmetricEncryption
    {  
        public static byte[] EncryptMessage(RSA publicKey, string message)
        {
            using (RSA rsa = RSA.Create())
            {
                //rsa.ImportParameters(publicKey);
                return publicKey.Encrypt(Encoding.UTF8.GetBytes(message), RSAEncryptionPadding.OaepSHA256);
            }
        }

        public static string DecryptMessage(RSA privateKey, byte[] encryptedMessage)
        {
            using (RSA rsa = RSA.Create())
            {                
                //rsa.ImportParameters(privateKey);
                byte[] decryptedBytes = privateKey.Decrypt(encryptedMessage, RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

    public class X509CertificateUtil
    {
        //Create a self-signed certificate using RSA key pair and PKCS#1 padding
        public static void CreateSelfSignedCertificate(string subjectName, string pfxPath, string password)
        {
            // Generate RSA Key Pair
            using (RSA rsa = RSA.Create(2048)) // 2048-bit RSA key
            {
                // Create certificate request
                var certRequest = new CertificateRequest(
                    subjectName,
                    rsa,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                // Add extensions (optional)
                certRequest.CertificateExtensions.Add(
                    new X509BasicConstraintsExtension(false, false, 0, false));
                certRequest.CertificateExtensions.Add(
                    new X509SubjectKeyIdentifierExtension(certRequest.PublicKey, false));

                // Create the certificate and sign it
                using (X509Certificate2 cert = certRequest.CreateSelfSigned(
                    DateTimeOffset.Now,
                    DateTimeOffset.Now.AddYears(5))) // Certificate valid for 5 years
                {
                    // Export private key and public key (.pfx format with a password)
                    byte[] pfxData = cert.Export(X509ContentType.Pfx, password);
                    System.IO.File.WriteAllBytes(pfxPath, pfxData);

                    Console.WriteLine($"Certificates saved: {pfxPath}");
                }
            }
        }

        public static X509Certificate2 LoadX509Certificate(string pfxPath, string password)
        {
            X509Certificate2 cert = new X509Certificate2(pfxPath, password, X509KeyStorageFlags.MachineKeySet);
                      
                // RSA privatekey= cert.GetRSAPrivateKey();      
                // RSA publickey = cert.GetRSAPublicKey();      
            return cert;
            
        }
    }

    

}

