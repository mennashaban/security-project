using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            // Convert both plainText and cipherText to lowercase
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            // Initialize the key list
            List<int> key = new List<int>();

            // Iterate over possible keys
            for (int temp = 1; temp <= plainText.Length; temp++)
            {
                key.Add(temp);

                // Generate permutations for the current key
                var permutations = GeneratePermutations(key);

                // Iterate over permutations
                foreach (var permutation in permutations)
                {
                    // Initialize a new Columnar object
                    Columnar columnar = new Columnar();

                    // Check if encryption or decryption with the current key matches the cipherText or plainText respectively
                    if (columnar.Encrypt(plainText, permutation) == cipherText || columnar.Decrypt(cipherText, permutation) == plainText)
                    {
                        return permutation; // Return the current key if found
                    }
                }
            }

            // Throw an exception if the key is not found
            throw new Exception("Key not found!");
        }

        //  generate all possible permutations
        private IEnumerable<List<int>> GeneratePermutations(List<int> key)
        {
            if (key.Count <= 1)
            {
                yield return key;
            }
            else
            {
                for (int i = 0; i < key.Count; i++)
                {
                    var subKey = new List<int>(key);
                    subKey.RemoveAt(i);
                    foreach (var permutation in GeneratePermutations(subKey))
                    {
                        permutation.Insert(0, key[i]);
                        yield return permutation;
                    }
                }
            }
        }




        public string Decrypt(string cipherText, List<int> key)
        {
            // Create the table of desired height
            int nRows = (int)Math.Ceiling((double)cipherText.Length / key.Count);
            List<List<char>> table = new List<List<char>>();
            for (int i = 0; i < key.Count; i++)
                table.Add(new List<char>());

            // Fill the table column-wise with-respect-to key indices
            int ctIndex = 0;
            for (int i = 0; i < table.Count; i++)
            {
                for (int j = 0; j < nRows; j++, ctIndex++)
                {
                    if (ctIndex < cipherText.Length)
                        table[key.IndexOf(i + 1)].Add(cipherText[ctIndex]);
                    else
                        table[key.IndexOf(i + 1)].Add('-');
                }
            }

            // Read the table row-wise
            string plain = "";
            for (int j = 0; j < nRows; j++)
            {
                for (int i = 0; i < table.Count; i++)
                {
                    if (table[i][j] != '-')
                        plain += table[i][j];
                }
            }

            return plain;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            
            int numRows = (int)Math.Ceiling((double)plainText.Length / key.Count);  //lab ex. 15/5 =3 rows
            List<List<char>> table = new List<List<char>>();

            // Initialize the table with '-' characters for empty spaces
            for (int i = 0; i < numRows; i++)
            {
                table.Add(new List<char>());
                for (int j = 0; j < key.Count; j++)
                {
                    table[i].Add('-');
                }
            }

            // Fill the table row-wise
            int ptIndex = 0;
            foreach (char c in plainText)
            {
                int rowIndex = ptIndex / key.Count;
                int colIndex = ptIndex % key.Count;
                table[rowIndex][colIndex] = c;
                ptIndex++;
            }

            // Read the table column-wise with respect to key indices
            string cipherText = "";
            for (int col = 0; col < key.Count; col++)
            {
                int keyIndex = key.IndexOf(col + 1);
                for (int row = 0; row < numRows; row++)
                {
                    if (table[row][keyIndex] != '-')
                    {
                        cipherText += table[row][keyIndex];
                    }
                }
            }

            return cipherText;
        }

    }
}

