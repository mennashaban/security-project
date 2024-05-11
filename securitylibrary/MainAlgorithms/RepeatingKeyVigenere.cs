using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            char[] characters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            int matrixSize = 26;
            char[,] matrix = new char[matrixSize, matrixSize];

            // Populate the matrix
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    matrix[i, j] = characters[(i + j) % matrixSize];
                }
            }

            string key = "";

            // Iterate through the characters in the plaintext and ciphertext
            for (int i = 0; i < plainText.Length; i++)
            {
                // Find possible keys for the current pair of characters
                List<char> possibleKeys = GetPossibleKeys(matrix, plainText[i], cipherText[i], characters);

                // Check if there is a unique possible key
                if (possibleKeys.Count == 1)
                {
                    key += possibleKeys.First();
                }
                else
                {
                    return "";
                }

                // Check if the current key successfully decrypts the ciphertext
                if (Encrypt(plainText, key).ToLower() == cipherText)
                {
                    return key;
                }
            }

            return key;
        }

        private List<char> GetPossibleKeys(char[,] matrix, char plainChar, char cipherChar, char[] alphabet)
        {
            List<char> possibleKeys = new List<char>();

            // Find possible keys by checking the matrix
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if (matrix[j, plainChar - 'a'] == cipherChar)
                {
                    possibleKeys.Add(alphabet[j]);
                }
            }

            return possibleKeys;
        }




        public string Decrypt(string cipherText, string key)
        {
            char[] characters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            StringBuilder decryptedText = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i++)
            {
                char cipherTextChar = char.ToLower(cipherText[i]);
                char keyChar = char.ToLower(key[i % key.Length]);

                if (char.IsLetter(cipherTextChar))
                {
                    int cipherTextIndex = Array.IndexOf(characters, cipherTextChar);
                    int keyIndex = Array.IndexOf(characters, keyChar);

                    // Ensure positive result by adding 26 and then taking modulo 26
                    int decryptedIndex = ((cipherTextIndex - keyIndex + 26) % 26);
                    char decryptedChar = characters[decryptedIndex];

                    // Preserve the original case
                    if (char.IsUpper(cipherText[i]))
                    {
                        decryptedChar = char.ToUpper(decryptedChar);
                    }

                    decryptedText.Append(decryptedChar);
                }
                else
                {
                    // If the character is not a letter, leave it unchanged
                    decryptedText.Append(cipherText[i]);
                }
            }

            return decryptedText.ToString();
        }




        public string Encrypt(string plainText, string key)
        {
            char[] characters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            StringBuilder encryptedText = new StringBuilder();

            for (int i = 0; i < plainText.Length; i++)
            {
                char plainTextChar = char.ToLower(plainText[i]);
                char keyChar = char.ToLower(key[i % key.Length]);

                if (char.IsLetter(plainTextChar))
                {
                    int plainTextIndex = Array.IndexOf(characters, plainTextChar);
                    int keyIndex = Array.IndexOf(characters, keyChar);

                    int encryptedIndex = (plainTextIndex + keyIndex) % 26;
                    char encryptedChar = characters[encryptedIndex];

                    // Preserve the original case
                    if (char.IsUpper(plainText[i]))
                    {
                        encryptedChar = char.ToUpper(encryptedChar);
                    }

                    encryptedText.Append(encryptedChar);
                }
                else
                {

                    encryptedText.Append(plainText[i]);
                }
            }

            return encryptedText.ToString();
        }

    }
}




