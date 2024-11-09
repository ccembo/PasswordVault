import time
from Crypto.Cipher import AES
from hashlib import sha256
import binascii
import sys

def encrypt_AES_GCM(msg, secretKey):
    aesCipher = AES.new(secretKey, AES.MODE_GCM)
    ciphertext, authTag = aesCipher.encrypt_and_digest(msg)
    return (ciphertext, aesCipher.nonce, authTag)

def decrypt_AES_GCM(encryptedMsg, secretKey):
    (ciphertext, nonce, authTag) = encryptedMsg
    aesCipher = AES.new(secretKey, AES.MODE_GCM, nonce)
    try:
        plaintext = aesCipher.decrypt_and_verify(ciphertext, authTag)
        return plaintext
    except (ValueError, KeyError):
        return None  # Return None if decryption fails

# Read the encrypted file path from an input argument

if len(sys.argv) != 2:
    print("Usage: python pyCracker.py <encrypted_file>")
    sys.exit(1)
encrypted_file = sys.argv[1]


# Read the encrypted message from a file 
with open(encrypted_file, "rb") as file:
        file_content = file.read()
        aesIV = file_content[:12]
        authTag = file_content[-16:]
        ciphertext = file_content[12:-16]

# Update the encryptedMsg tuple with the values read from the file
encryptedMsg = (ciphertext, aesIV, authTag)

# Attempt decryption with each passcode in rockyou.txt
with open("rockyou.txt", "r", encoding="utf-8", errors="ignore") as file:
    passcodes = file.readlines()
starttime = time.time()
for passcode in passcodes:
    passcode = passcode.strip()  # Remove any whitespace or newline characters
    # Hash the passcode with SHA-256 to derive a 256-bit key
    key = sha256(passcode.encode()).digest()
    
    # Attempt to decrypt with the derived key
    decryptedMsg = decrypt_AES_GCM(encryptedMsg, key)
    if decryptedMsg:
        print("Decryption successful with passcode:", passcode)
        print("Decrypted Message:", decryptedMsg)
        print("Key found in " , time.time() - starttime, " seconds")
        break
else:
    print("No valid passcode found in rockyou.txt")
    print("Total time " , time.time() - starttime, " seconds")
