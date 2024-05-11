using System;
using System.Numerics;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int Encrypt(int p, int q, int M, int e)
        {
            // Calculate n (modulus)
            int n = p * q;

            // Calculate the encrypted ciphertext (C)
            int C = ModularPow(M, e, n);

            return C;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            // Calculate n (modulus)
            int n = p * q;

            // Calculate phi(n)
            int phiN = (p - 1) * (q - 1);

            // Calculate the modular inverse of e (d)
            int d = ModularInverse(e, phiN);

            // Calculate the decrypted plaintext (M)
            int M = ModularPow(C, d, n);

            return M;
        }

        private int ModularPow(int a, int b, int n)
        {
            BigInteger result = 1;

            BigInteger baseValue = a;
            BigInteger exponent = b;
            BigInteger modulus = n;

            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                {
                    result = (result * baseValue) % modulus;
                }

                baseValue = (baseValue * baseValue) % modulus;
                exponent /= 2;
            }

            return (int)result;
        }

        private int ModularInverse(int a, int n)
        {
            int t = 0, newT = 1;
            int r = n, newR = a;

            while (newR != 0)
            {
                int quotient = r / newR;

                (t, newT) = (newT, t - quotient * newT);
                (r, newR) = (newR, r - quotient * newR);
            }

            if (r > 1)
            {
                throw new Exception("The given number has no modular inverse.");
            }

            if (t < 0)
            {
                t += n;
            }

            return t;
        }
    }
}