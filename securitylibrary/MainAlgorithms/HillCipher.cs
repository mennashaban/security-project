using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace SecurityLibrary
{

    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            List<double> c = cipherText.ConvertAll(x => (double)x);
            //List<double> p = plainText.ConvertAll(x => (double)x);
            int matrixSize = Convert.ToInt32(Math.Sqrt(c.Count));

            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            List<int> mayBeKey = new List<int> { i, j, k, l };
                            List<int> encrypted = Encrypt(plainText, mayBeKey);

                            if (encrypted.SequenceEqual(cipherText))
                            {
                                return mayBeKey;
                            }
                        }
                    }
                }
            }

            throw new InvalidAnlysisException();
        }

        public string Analyse(string plainText, string cipherText)
        {
            // Convert strings to character arrays
            char[] plainChars = plainText.ToUpper().ToCharArray();
            char[] cipherChars = cipherText.ToUpper().ToCharArray();

            // Define the alphabet
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Loop through all possible key combinations
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            // Construct the current key
                            string key = $"{alphabet[i]}{alphabet[j]}{alphabet[k]}{alphabet[l]}";

                            // Encrypt the plain text with the current key
                            string encrypted = Encrypt(plainText, key);

                            // Check if the encrypted text matches the cipher text
                            if (encrypted.Equals(cipherText, StringComparison.OrdinalIgnoreCase))
                            {
                                return key;
                            }
                        }
                    }
                }
            }

            // If no key is found, throw an exception
            throw new InvalidAnlysisException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int key_number = key.Count();
            for (int i = 0; i < key_number; i++)
            {
                if (key[i] > 26 || key[i] < 0)
                    throw new Exception();
            }
            int matrix_size = (int)Math.Sqrt(key_number);
            double[,] key_matrix = new double[matrix_size, matrix_size];

            int matrix_index = 0;
            for (int i = 0; i < matrix_size; i++)
            {
                for (int j = 0; j < matrix_size; j++)
                {
                    key_matrix[j, i] = key[matrix_index];
                    matrix_index++;
                }
            }
            double[,] inverse_key = InverseMatrix(key_matrix);
            int number_of_sub_plain = cipherText.Count() / matrix_size;
            List<List<double>> ciphertextcutt = new List<List<double>>();
            List<int> result = new List<int>();
            int counter = 0;
            for (int i = 0; i < number_of_sub_plain; i++)
            {
                List<double> temp = new List<double>();
                ciphertextcutt.Add(temp);
                for (int j = 0; j < matrix_size; j++)
                {
                    ciphertextcutt[i].Add(cipherText[counter]);
                    counter++;
                }
            }
            for (int i = 0; i < number_of_sub_plain; i++)
            {
                for (int j = 0; j < matrix_size; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < matrix_size; k++)
                    {
                        sum += ciphertextcutt[i][k] * inverse_key[k, j];
                    }
                    bool flag = true;
                    while (flag)
                    {
                        if (sum < 0)
                        {
                            sum += 26;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    result.Add((int)(sum % 26));
                }
            }
            return result;

            double[,] InverseMatrix(double[,] matrix)
            {
                int n = matrix.GetLength(0);
                double[,] inverse = new double[n, n];

                if (n == 1)
                {
                    inverse[0, 0] = 1 / matrix[0, 0];
                    return inverse;
                }
                else if (n == 2)
                {
                    double det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                    if (det == 0)
                    {
                        throw new Exception();
                    }
                    double c = gcd((int)det, 26);
                    if (gcd((int)det, 26) != 1 && gcd((int)det, 26) != -1)
                    {
                        throw new Exception();
                    }
                    inverse[0, 0] = matrix[1, 1] / det;
                    inverse[0, 1] = -matrix[0, 1] / det;
                    inverse[1, 0] = -matrix[1, 0] / det;
                    inverse[1, 1] = matrix[0, 0] / det;
                    return inverse;
                }
                else
                {
                    double det = Determinant(matrix);
                    if (det == 0)
                    {
                        throw new Exception();
                    }
                    if (gcd((int)det, 26) != 1 && gcd((int)det, 26) != -1)
                    {
                        throw new Exception();
                    }
                    for (int i = 2; i < 26; i++)
                    {
                        if ((det * i) % 26 == 1)
                        {
                            det = i;
                            break;
                        }
                    }
                    double[,] adjugate = Adjugate(matrix);
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            inverse[i, j] = (adjugate[i, j] * det) % 26;
                        }
                    }
                    return inverse;
                }
            }

            double Determinant(double[,] matrix)
            {
                int n = matrix.GetLength(0);
                double det = 0;
                if (n == 1)
                {
                    det = matrix[0, 0];
                }
                else if (n == 2)
                {
                    det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        double[,] submatrix = new double[n - 1, n - 1];
                        for (int j = 1; j < n; j++)
                        {
                            for (int k = 0; k < n; k++)
                            {
                                if (k < i)
                                {
                                    submatrix[j - 1, k] = matrix[j, k];
                                }
                                else if (k > i)
                                {
                                    submatrix[j - 1, k - 1] = matrix[j, k];
                                }
                            }
                        }
                        double cofactor = Math.Pow(-1, i) * Determinant(submatrix);
                        det += matrix[0, i] * cofactor;
                    }
                }

                return det % 26;
            }

            double gcd(int num1, int num2)
            {
                if (num1 < num2)
                {
                    int temp = num1;
                    num1 = num2;
                    num2 = temp;
                }

                int remainder = num1 % num2;

                while (remainder != 0)
                {
                    num1 = num2;
                    num2 = remainder;
                    remainder = num1 % num2;
                }
                int gccd = num2;
                return gccd;
            }

            double[,] Adjugate(double[,] matrix)
            {
                int n = matrix.GetLength(0);
                double[,] adjugate = new double[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        double[,] submatrix = new double[n - 1, n - 1];
                        for (int k = 0; k < n; k++)
                        {
                            for (int l = 0; l < n; l++)
                            {
                                if (k < i && l < j)
                                {
                                    submatrix[k, l] = matrix[k, l];
                                }
                                else if (k < i && l > j)
                                {
                                    submatrix[k, l - 1] = matrix[k, l];
                                }
                                else if (k > i && l < j)
                                {
                                    submatrix[k - 1, l] = matrix[k, l];
                                }
                                else if (k > i && l > j)
                                {
                                    submatrix[k - 1, l - 1] = matrix[k, l];
                                }
                            }
                        }
                        double cofactor = Math.Pow(-1, i + j) * Determinant(submatrix);
                        adjugate[j, i] = cofactor;
                    }
                }
                return adjugate;
            }
        }
        public string Decrypt(string cipherText, string key)
        {
            // Convert the key to uppercase
            key = key.ToUpper();

            // Convert the cipher text to uppercase
            cipherText = cipherText.ToUpper();

            // Calculate the key matrix size
            int keySize = (int)Math.Sqrt(key.Length);

            // Create a StringBuilder to store the decrypted text
            StringBuilder decryptedText = new StringBuilder();

            // Create a matrix to store the key
            int[,] keyMatrix = new int[keySize, keySize];

            // Populate the key matrix using the key string
            int index = 0;
            for (int i = 0; i < keySize; i++)
            {
                for (int j = 0; j < keySize; j++)
                {
                    keyMatrix[i, j] = key[index] - 'A';
                    index++;
                }
            }

            // Calculate the modular inverse of the determinant of the key matrix
            int determinant = GetDeterminant(keyMatrix);
            int modInverse = GetModularInverse(determinant, 26);

            // Calculate the adjugate matrix
            int[,] adjugateMatrix = GetAdjugateMatrix(keyMatrix);

            // Calculate the inverse key matrix
            int[,] inverseKeyMatrix = MultiplyMatrixScalar(adjugateMatrix, modInverse);

            // Calculate the size of the blocks in the cipher text
            int blockSize = keySize;

            // Iterate through the cipher text in blocks of size blockSize
            for (int i = 0; i < cipherText.Length; i += blockSize)
            {
                // Extract the current block of cipher text
                string block = cipherText.Substring(i, Math.Min(blockSize, cipherText.Length - i));

                // Convert the block to a list of integers representing characters
                List<int> blockInts = new List<int>();
                foreach (char c in block)
                {
                    blockInts.Add(c - 'A');
                }

                // Decrypt the block using the inverse key matrix
                List<int> decryptedBlockInts = DecryptBlock(blockInts, inverseKeyMatrix);

                // Convert the decrypted block back to a string
                string decryptedBlock = string.Join("", decryptedBlockInts.Select(x => (char)(x + 'A')));

                // Append the decrypted block to the decrypted text
                decryptedText.Append(decryptedBlock);
            }

            // Return the decrypted text
            return decryptedText.ToString();
        }

        private List<int> DecryptBlock(List<int> block, int[,] inverseKeyMatrix)
        {
            int keySize = (int)Math.Sqrt(inverseKeyMatrix.Length);
            List<int> result = new List<int>();

            for (int i = 0; i < keySize; i++)
            {
                int sum = 0;
                for (int j = 0; j < keySize; j++)
                {
                    sum += inverseKeyMatrix[i, j] * block[j];
                }
                result.Add((sum % 26 + 26) % 26); // Ensure the result is positive modulo 26
            }

            return result;
        }

        private int GetDeterminant(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n == 1)
            {
                return matrix[0, 0];
            }
            else if (n == 2)
            {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            else
            {
                int det = 0;
                for (int j = 0; j < n; j++)
                {
                    det += matrix[0, j] * (int)Math.Pow(-1, 0 + j) * GetDeterminant(GetSubMatrix(matrix, 0, j));
                }
                return det;
            }
        }

        private int[,] GetSubMatrix(int[,] matrix, int excludeRow, int excludeCol)
        {
            int n = matrix.GetLength(0);
            int[,] subMatrix = new int[n - 1, n - 1];
            int rowIndex = 0, colIndex = 0;
            for (int i = 0; i < n; i++)
            {
                if (i == excludeRow)
                    continue;

                colIndex = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == excludeCol)
                        continue;

                    subMatrix[rowIndex, colIndex] = matrix[i, j];
                    colIndex++;
                }
                rowIndex++;
            }
            return subMatrix;
        }

        private int GetModularInverse(int a, int m)
        {
            a = (a % m + m) % m;

            for (int b = 1; b < m; b++)
            {
                if ((a * b) % m == 1)
                {
                    return b;
                }
            }

            throw new ArgumentException("Modular inverse does not exist.");
        }

        private int[,] GetAdjugateMatrix(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int[,] adjugate = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int[,] subMatrix = GetSubMatrix(matrix, i, j);
                    int cofactor = (int)Math.Pow(-1, i + j) * GetDeterminant(subMatrix);
                    adjugate[j, i] = cofactor;
                }
            }

            return adjugate;
        }

        private int[,] MultiplyMatrixScalar(int[,] matrix, int scalar)
        {
            int n = matrix.GetLength(0);
            int[,] result = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = (matrix[i, j] * scalar) % 26;
                }
            }

            return result;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> c = new List<int>();
            int keySize = (int)Math.Sqrt(key.Count);
            int plainTextLength = plainText.Count;

            for (int i = 0; i < plainTextLength; i += keySize)
            {
                List<int> chunk = plainText.GetRange(i, Math.Min(keySize, plainTextLength - i));
                List<int> encryptedChunk = new List<int>();

                for (int j = 0; j < keySize; j += keySize)
                {
                    int[] chunkSlice = chunk.Skip(j).Take(keySize).ToArray();
                    for (int k = 0; k < keySize; k++)
                    {
                        int sum = 0;
                        for (int l = 0; l < keySize; l++)
                        {
                            sum += key[k * keySize + l] * chunkSlice[l];
                        }
                        encryptedChunk.Add(sum % 26); // Assuming a-z are mapped to 0-25
                    }
                }

                c.AddRange(encryptedChunk);
            }

            return c;
        }



        public string Encrypt(string plainText, string key)
        {
            // Convert plaintext and key to lowercase
            plainText = plainText.ToLower();
            key = key.ToLower();

            // Convert plaintext and key to lists of integers representing characters
            List<int> plainTextInts = plainText.Select(c => c - 'a').ToList();
            List<int> keyInts = key.Select(c => c - 'a').ToList();

            // Encrypt the plaintext using the key
            List<int> encryptedInts = Encrypt(plainTextInts, keyInts);

            // Convert the encrypted integers back to characters
            string encryptedText = string.Join("", encryptedInts.Select(i => (char)(i + 'a')));

            return encryptedText;
        }



        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            List<double> cipher = cipherText.ConvertAll(x => (double)x);

            List<double> Plain = plainText.ConvertAll(x => (double)x);

            int matrix = Convert.ToInt32(Math.Sqrt((cipher.Count)));

            Matrix<double> cipher_matrix = DenseMatrix.OfColumnMajor(matrix, cipherText.Count / matrix, cipher.AsEnumerable());

            Matrix<double> plain_matrix = DenseMatrix.OfColumnMajor(matrix, plainText.Count / matrix, Plain.AsEnumerable());

            List<int> k = new List<int>();

            Matrix<double> key_matrix = DenseMatrix.Create(3, 3, 0);

            plain_matrix = Mod(plain_matrix.Transpose(), Det(plain_matrix));

            key_matrix = (cipher_matrix * plain_matrix);

            k = key_matrix.Transpose().Enumerate().ToList().Select(i => (int)i % 26).ToList();


            return k;
        }
        public int Det(Matrix<double> matrix)
        {
            double A = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) -
                       matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
                       matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);

            int AI = (int)(A % 26);
            AI = (AI >= 0) ? AI : AI + 26;

            int i = 0;
            do
            {
                if ((AI * i) % 26 == 1)
                {
                    return i;
                }
                i++;
            } while (i < 26);

            return -1;
        }

        public Matrix<double> Mod(Matrix<double> matrix, int A)
        {
            Matrix<double> res = Matrix<double>.Build.Dense(3, 3);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int x;
                    if (i == 0)
                        x = 1;
                    else
                        x = 0;

                    int y = (j == 0) ? 1 : 0;
                    int x1 = (i == 2) ? 1 : 2;

                    int y1;
                    if (j == 2)
                        y1 = 1;
                    else
                        y1 = 2;
                    double r = ((matrix[x, y] * matrix[x1, y1] - matrix[x, y1] * matrix[x1, y]) * Math.Pow(-1, i + j) * A) % 26;

                    if (r >= 0)
                        res[i, j] = r;
                    else
                        res[i, j] = r + 26;
                }
            }

            return res;
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            plain3 = plain3.ToUpper();
            List<char> plain_char = plain3.ToList();

            List<int> plain_list = new List<int>();

            for (int i = 0; i < plain_char.Count; i++)
            {
                plain_list.Add((int)plain_char[i] - 65);
            }

            cipher3 = cipher3.ToUpper();
            List<char> cipher_char = cipher3.ToList();

            List<int> cipher_list = new List<int>();

            for (int i = 0; i < cipher_char.Count; i++)
            {
                cipher_list.Add((int)cipher_char[i] - 65);
            }

            List<int> key_list = Analyse3By3Key(plain_list, cipher_list);
            List<char> key_char = new List<char>();

            for (int i = 0; i < key_list.Count; i++)
            {
                key_char.Add((char)(key_list[i] + 65));
            }

            string key = new string(key_char.ToArray());
            return key;
        }


    }
}