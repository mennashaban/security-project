using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            int cipherLength = cipherText.Length;

            // Iterate through possible keys
            for (int key = 1; key <= cipherLength; key++)
            {
                // Decrypt the ciphertext using the current key
                string decrypted = Decrypt(cipherText, key);
                string encryoted = Encrypt(plainText, key);
                // Check if the decrypted result matches the original plaintext
                if (decrypted == plainText || encryoted == cipherText)
                {
                    return key; // Return the key if decryption is successful
                }
            }

            // If no suitable key is found, return 0
            return 0;
        }


        public string Decrypt(string cipherText, int key)
        {
            int length = cipherText.Length;
            int rows = key;
            int cols = (int)Math.Ceiling((double)length / key);

            // Initialize the decrypted plaintext
            StringBuilder decryptedText = new StringBuilder();

            // Iterate through the rail fence pattern and pick characters from the ciphertext
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    // Calculate the index of the current character in the rail fence pattern
                    int index = i + j * cols;

                    // Check if the index is within the bounds of the ciphertext length
                    if (index < length)
                    {
                        // Append the character to the decrypted plaintext
                        decryptedText.Append(cipherText[index]);
                    }
                }
            }

            // Convert the decrypted plaintext to lowercase and return
            return decryptedText.ToString().ToLower();
        }

        public string Encrypt(string plainText, int key)
        {
            int length = plainText.Length;
            int rows = key;
            int cols = (int)Math.Ceiling((double)length / key);
            char[,] railFence = new char[rows, cols];
            int counter = 0;
            int j = 0;
            while (counter < key)
            {
                j = counter;
                while (j < length)
                {
                    railFence[counter, j / key] = plainText[j];
                    j += key;
                }
                counter++;
            }
            for (int w = 0; w < rows; w++)
            {

            }
            string ret = "";
            int i = 0;
            while (i < rows)
            {
                int k = 0;
                while (k < cols)
                {
                    if (railFence[i, k] != '\0')
                        ret += railFence[i, k];
                    k++;
                }
                i++;
            }
            return ret.ToUpper();

        }
    }
}