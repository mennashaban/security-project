using System;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid
    {
        /// <summary>
        /// Calculates the multiplicative inverse of a number modulo baseN.
        /// </summary>
        /// <param name="number">The number to find the inverse of.</param>
        /// <param name="baseN">The base modulo value.</param>
        /// <returns>The multiplicative inverse, or -1 if no inverse exists.</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {

            if (baseN == 1)
                return 0;
            if (number == 0)
                return -1;
            if (number == 1)
                return 1;


            int Q = 0;
            int A1 = 1;
            int A2 = 0;
            int A3 = baseN;
            int B1 = 0;
            int B2 = 1;
            int B3 = number;


            if (B3 == 0)
            {
                return -1;
            }
            if (B3 == 1)
            {
                return B2;
            }




            switch (B3)
            {
                case 0:
                    return -1;
                    break;

                case 1:
                    return B2;
                    break;

                default:
                    int OLDA1;
                    int OLDA2;
                    int OLDA3;
                    while (B3 != 1)
                    {
                        Q = A3 / B3;
                        OLDA1 = A1;
                        OLDA2 = A2;
                        OLDA3 = A3;
                        A1 = B1;
                        A2 = B2;
                        A3 = B3;
                        B1 = OLDA1 - (Q * B1);
                        B2 = OLDA2 - (Q * B2);
                        B3 = OLDA3 - (Q * B3);


                        if (B3 == 0)
                            return -1;

                    }
                    break;

            }

            int GCD;
            int INVERSE = 0;

            GCD = B3;
            if (GCD != 1)
            {
                return -1;
            }

            INVERSE = B2;


            if (INVERSE < 0)
            {
                INVERSE = INVERSE + baseN;
            }

            return INVERSE;

        }
    }
}