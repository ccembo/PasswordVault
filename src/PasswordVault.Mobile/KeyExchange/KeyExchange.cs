using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace PasswordVault.Mobile.KeyExchange
{
    internal class KeyExchange
    {
        public static void test()
        {
            using (ECDiffieHellman alice = ECDiffieHellman.Create())
            using (ECDiffieHellman bob = ECDiffieHellman.Create())
            {
                // Alice generates her public key and shares with Bob
                byte[] alicePublicKey = alice.PublicKey.ExportSubjectPublicKeyInfo();

                //System.Security.Cryptography.ECDiffieHellmanPublicKey.
                //ECDiffieHellmanPublicKey.

                // Bob generates a shared secret using Alice's public key
                bob.ImportSubjectPublicKeyInfo(alicePublicKey, out _);
                byte[] bobSharedKey = bob.DeriveKeyMaterial(alice.PublicKey);

                // Alice generates a shared secret using Bob's public key
                byte[] aliceSharedKey = alice.DeriveKeyMaterial(bob.PublicKey);

                Console.WriteLine($"Are keys equal: {Convert.ToBase64String(aliceSharedKey) == Convert.ToBase64String(bobSharedKey)}");

                // Encrypt and Decrypt example can follow based on symmetric encryption using the shared key
            }
        }
    }
}

