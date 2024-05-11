using System;
using System.Text;

namespace SecurityLibrary.RC4
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal, not a string.
    /// </summary>
    public class RC4 : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            bool isHexadecimal = cipherText.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
                                key.StartsWith("0x", StringComparison.OrdinalIgnoreCase);

            if (isHexadecimal)
            {
                cipherText = HexaDecimalToString(cipherText);
                key = HexaDecimalToString(key);
            }

            int[] s = new int[256];
            for (int i = 0; i < 256; i++)
            {
                s[i] = i;
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + s[i] + key[i % key.Length]) % 256;
                Swap(ref s[i], ref s[j]);
            }

            int x = 0;
            j = 0;
            char[] plainText = new char[cipherText.Length];
            for (int i = 0; i < cipherText.Length; i++)
            {
                x = (x + 1) % 256;
                j = (j + s[x]) % 256;
                Swap(ref s[x], ref s[j]);
                plainText[i] = (char)(cipherText[i] ^ s[(s[x] + s[j]) % 256]);
            }

            string result = new string(plainText);

            if (isHexadecimal)
            {
                result = StringToHexaDecimal(result);
            }

            return result;
        }

        public override string Encrypt(string plainText, string key)
        {
            bool isHexadecimal = plainText.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
                                key.StartsWith("0x", StringComparison.OrdinalIgnoreCase);

            if (isHexadecimal)
            {
                plainText = HexaDecimalToString(plainText);
                key = HexaDecimalToString(key);
            }

            int[] s = new int[256];
            for (int i = 0; i < 256; i++)
            {
                s[i] = i;
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + s[i] + key[i % key.Length]) % 256;
                Swap(ref s[i], ref s[j]);
            }

            int x = 0;
            j = 0;
            char[] cipherText = new char[plainText.Length];
            for (int i = 0; i < plainText.Length; i++)
            {
                x = (x + 1) % 256;
                j = (j + s[x]) % 256;
                Swap(ref s[x], ref s[j]);
                cipherText[i] = (char)(plainText[i] ^ s[(s[x] + s[j]) % 256]);
            }

            string result = new string(cipherText);

            if (isHexadecimal)
            {
                result = StringToHexaDecimal(result);
            }

            return result;
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public static string HexaDecimalToString(string hex)
        {
            StringBuilder sb = new StringBuilder();

            if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                hex = hex.Substring(2);
            }

            for (int i = 0; i < hex.Length; i += 2)
            {
                string hexPair = hex.Substring(i, 2);
                int decimalValue = Convert.ToInt32(hexPair, 16);
                sb.Append((char)decimalValue);
            }

            return sb.ToString();
        }

        public static string StringToHexaDecimal(string text)
        {
            StringBuilder hex = new StringBuilder();
            foreach (char c in text)
            {
                hex.Append(((int)c).ToString("X2"));
            }
            return "0x" + hex.ToString();
        }
    }
}
