using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman
    {
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            //q-> mode prime
            //alpha -> generator
            //xa-> private key of a
            //xb -> private key of b

            //calcualte public key of a and b
            int ya = ModPow(alpha, xa, q);
            int yb = ModPow(alpha, xb, q);

            //calcualte the secret key of a and b
            int secretA = ModPow(yb, xa, q);
            int secretB = ModPow(ya, xb, q);

            return new List<int> { secretA, secretB };
        }

        private int ModPow(int value, int exponent, int modulus)
        {
            // ensures that the method returns an integer result within the range specified
            BigInteger result = BigInteger.ModPow(value, exponent, modulus);
            return (int)(result % modulus);
        }
    }
}