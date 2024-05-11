using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        private DES desAlgorithm;

        public TripleDES()
        {
            desAlgorithm = new DES();
        }

        public string Decrypt(string cipherText, List<string> keys)
        {
            string outputDEcrypt = desAlgorithm.Decrypt(cipherText, keys[0]);
            string outputENcrypt = desAlgorithm.Encrypt(outputDEcrypt, keys[1]);
            string outputDesTripleDecrypt = desAlgorithm.Decrypt(outputENcrypt, keys[0]);
            return outputDesTripleDecrypt;
        }

        public string Encrypt(string plainText, List<string> keys)
        {
            string outputDEcrypt = desAlgorithm.Encrypt(plainText, keys[0]);
            string outputENcrypt = desAlgorithm.Decrypt(outputDEcrypt, keys[1]);
            string outputDesTripleEnecrypt = desAlgorithm.Encrypt(outputENcrypt, keys[0]);
            return outputDesTripleEnecrypt;
        }

        public List<string> Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }
    }
}