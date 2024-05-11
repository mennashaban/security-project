using System;
using System.Security.Cryptography;
using System.Text;

namespace SecurityLibrary.DES
{
    public class DES
    {
        public string Encrypt(string plainText, string key)
        {
            byte[] plainBytes = HexaToByteArray(plainText);
            byte[] keyBytes = HexaToByteArray(key);

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = keyBytes;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.Zeros;

                ICryptoTransform encryptor = des.CreateEncryptor();

                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                return ByteArrayToHexa(cipherBytes);
            }
        }

        public string Decrypt(string cipherText, string key)
        {
            byte[] cipherBytes = HexaToByteArray(cipherText);
            byte[] keyBytes = HexaToByteArray(key);

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = keyBytes;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.Zeros;

                ICryptoTransform decryptor = des.CreateDecryptor();

                byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                return ByteArrayToHexa(plainBytes);
            }
        }

        private static byte[] HexaToByteArray(string hex)
        {
            hex = hex.Replace("0x", "");
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        private static string ByteArrayToHexa(byte[] bytes)
        {
            return "0x" + BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}