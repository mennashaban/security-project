using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {



        public string Encrypt(string plainText, int key)
        {
            StringBuilder cipher = new StringBuilder();

            for (int i = 0; i < plainText.Length; i++)
            {
                char index = (char)(((plainText[i] - 'a' + key) % 26) + 'a');
                cipher.Append(index);
            }

            return cipher.ToString();
        }


        public string Decrypt(string cipherText, int key)
        {
            StringBuilder plain = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i++)
            {
                char encryptedChar = cipherText[i];

                // Decrypt uppercase letters
                if (char.IsUpper(encryptedChar))
                {
                    char decryptedChar = (char)((encryptedChar - 'A' - key + 26) % 26 + 'A');
                    plain.Append(decryptedChar);
                }
                // Decrypt lowercase letters
                else if (char.IsLower(encryptedChar))
                {
                    char decryptedChar = (char)((encryptedChar - 'a' - key + 26) % 26 + 'a');
                    plain.Append(decryptedChar);
                }
                // Preserve non-alphabetic characters
                else
                {
                    plain.Append(encryptedChar);
                }
            }

            return plain.ToString();
        }



        public int Analyse(string plainText, string cipherText)
        {
            // Assuming the input strings are uppercase
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();

            // Loop through each possible shift (0 to 25)
            for (int i = 0; i < 26; i++)
            {
                // Attempt to decrypt the cipher text using the current shift
                string decryptedText = Decrypt(cipherText, i);

                // Check if the decrypted text matches the given plain text
                if (decryptedText == plainText)
                {
                    // Return the found shift (which corresponds to the key)
                    return i;
                }
            }

            // If no key is found, return -1 or handle appropriately
            return -1;
        }
    }
}