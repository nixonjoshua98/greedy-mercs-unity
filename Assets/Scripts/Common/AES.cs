using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SRC
{
    public static class AES
    {
        private static readonly byte[] key = Encoding.UTF8.GetBytes("1234567895645451");

        public static string Encrypt(string plainText)
        {
            using (Aes algo = Aes.Create())
            {
                algo.Key = key;

                // Create a decrytor to perform the stream transform.
                var encryptor = algo.CreateEncryptor(algo.Key, algo.IV);

                // Create the streams used for encryption.
                using (var ms = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write IV first
                            ms.Write(algo.IV, 0, algo.IV.Length);

                            // Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (Aes algo = Aes.Create())
            {
                algo.Key = key;

                // Get bytes from input string
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Create the streams used for decryption.
                using (MemoryStream ms = new MemoryStream(cipherBytes))
                {
                    // Read IV first
                    byte[] IV = new byte[16];
                    ms.Read(IV, 0, IV.Length);

                    // Assign IV to an algorithm
                    algo.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    var decryptor = algo.CreateDecryptor(algo.Key, algo.IV);

                    using (var csDecrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
