using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecurityLibrary.AES
{
    public class AES
    {
        public string Encrypt(string plainText, string key)
        {
            byte[] plainBytes = HexaToByteArray(plainText);
            byte[] keyBytes = HexaToByteArray(key);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                ICryptoTransform encryptor = aes.CreateEncryptor();

                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                return ByteArrayToHexa(cipherBytes);
            }
        }

        public string Decrypt(string cipherText, string key)
        {
            byte[] cipherBytes = HexaToByteArray(cipherText);
            byte[] keyBytes = HexaToByteArray(key);

            using (Aes aes = Aes.Create())
            {
                //each block of plaintext is encrypted independently of other blocks
                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                ICryptoTransform decryptor = aes.CreateDecryptor();

                byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length); // transform the ciphertext byte array into the plaintext byte array

                return ByteArrayToHexa(plainBytes);
            }
        }

        private static byte[] HexaToByteArray(string hexa)
        {
            //Removes the 0x
            hexa = hexa.Replace("0x", "");
            //each byte is represented by two hexadecimal digits
            byte[] bytes = new byte[hexa.Length / 2];
            for (int i = 0; i < hexa.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexa.Substring(i, 2), 16);
            }
            return bytes;
        }

        private static string ByteArrayToHexa(byte[] bytes)
        {
            return "0x" + BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}