using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        string alpha_char = "abcdefghijklmnopqrstuvwxyz";
        Dictionary<string, int> alpha_Dic = new Dictionary<string, int>();

        void alphabet_dic()
        {
            alpha_Dic.Clear(); // Clear the dictionary to avoid adding duplicate keys
            for (int i = 0; i < alpha_char.Length; i++)
            {
                string key = alpha_char[i].ToString();
                if (!alpha_Dic.ContainsKey(key)) // Check if the key already exists
                {
                    alpha_Dic.Add(key, i);
                }
            }
        }

        public string Analyse(string plainText, string cipherText)

        {
            alphabet_dic();

            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            string key = "";

            for (int i = 0; i < cipherText.Length; i++)
            {
                int cipher = alpha_Dic[cipherText[i].ToString()];
                int plain = alpha_Dic[plainText[i].ToString()];

                key += alpha_char[(cipher - plain + 26) % 26];
            }
            string original_key = "";
            string simlar = "";
            simlar = plainText.Substring(0, Math.Min(18, plainText.Length));

            int index = key.IndexOf(simlar);
            original_key = key.Substring(0, Math.Min(index, plainText.Length));


            return original_key;
        }

        public string Decrypt(string cipherText, string key)
        {

            alphabet_dic();

            cipherText = cipherText.ToLower();
            key = key.ToLower();
            string plain_text = "";

            for (int i = 0, keyIndex = 0; i < cipherText.Length; i++, keyIndex++)
            {
                int key_i = alpha_Dic[key[keyIndex].ToString()];
                int cipherIndex = alpha_Dic[cipherText[i].ToString()];
                plain_text += alpha_char[(cipherIndex - key_i + 26) % 26];
                if (key.Length < cipherText.Length)
                {
                    key += plain_text[i];
                }
            }

            return plain_text;
        }

        public string Encrypt(string plainText, string key)
        {
            string new_plain = plainText.ToLower();

            alphabet_dic();

            string cipher_text = "";

            if (key.Length != new_plain.Length)
            {
                for (int i = 0; i < plainText.Length; i++)
                {
                    key += plainText[i];
                    if (key.Length == plainText.Length) { break; }
                }
            }
            for (int i = 0; i < new_plain.Length; i++)
            {
                // Encrypt the current character
                int keyIndex = alpha_Dic[key[i].ToString()]; // Since the key and plaintext are the same length, we can use the current index
                int plainIndex = alpha_Dic[new_plain[i].ToString()]; // Get the index of the current character in the alphabet
                cipher_text += alpha_char[(plainIndex + keyIndex) % 26]; // Append the encrypted character to the ciphertext
            }

            return cipher_text;


        }


    }
}

