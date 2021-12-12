using System.IO;
using System.Security.Cryptography;

namespace GM
{
    public static class AES
    {
        static byte[] key = {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        };

        public static void Encrypt(string path, string text)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;

                    fileStream.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter encryptWriter = new StreamWriter(cryptoStream))
                        {
                            encryptWriter.WriteLine(text);
                        }
                    }
                }
            }
        }

        public static string Encrypt(string text)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;

                    stream.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter encryptWriter = new StreamWriter(cryptoStream))
                        {
                            encryptWriter.WriteLine(text);

                            return stream.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string Decrypt(string path)
        {
            string output = string.Empty;

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] iv = new byte[aes.IV.Length];

                    fileStream.Read(iv, 0, aes.IV.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                    {
                        using (StreamReader decryptReader = new StreamReader(cryptoStream))
                        {
                            output = decryptReader.ReadToEnd();
                        }
                    }
                }
            }

            return output;
        }
    }
}
