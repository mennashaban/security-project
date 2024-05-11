using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographicTechnique<string, string>
    {
        /// <summary>
        /// The most common diagrams in english (sorted): TH, HE, AN, IN, ER, ON, RE, ED, ND, HA, AT, EN, ES, OF, NT, EA, TI, TO, IO, LE, IS, OU, AR, AS, DE, RT, VE
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Analyse(string plainText)
        {
            throw new NotImplementedException();
        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            if (cipherText == null || key == null)
                return "Enter cipherText and Key  again";

            key = key.ToLower();

            char[,] playfair_matrix = new char[5, 5];
            string combinedKey = key + "abcdefghijklmnopqrstuvwxyz";
            string filtered_Key = new string(combinedKey.Distinct().ToArray());
            int playfair_matrix_index = 0;


            int row_of_I = 0;
            int column_of_I = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (filtered_Key[playfair_matrix_index] == 'i' || filtered_Key[playfair_matrix_index] == 'j')
                    {
                        if (playfair_matrix[row_of_I, column_of_I] == 'i' || playfair_matrix[row_of_I, column_of_I] == 'j')
                        {
                            playfair_matrix_index++;
                            playfair_matrix[i, j] = filtered_Key[playfair_matrix_index++];
                        }
                        else
                        {
                            playfair_matrix[i, j] = 'i';

                            row_of_I = i;
                            column_of_I = j;
                            playfair_matrix_index++;
                        }
                    }
                    else
                    {
                        playfair_matrix[i, j] = filtered_Key[playfair_matrix_index++];
                    }
                }
            }




            cipherText = cipherText.Replace(" ", "").ToLower();

            string[] cipherText_matrix = new string[cipherText.Length / 2];
            int cipherText_lenght = cipherText.Length / 2;


            string two_letter;
            char first;
            char second;

            char x = 'j';

            int m = 0;
            bool dup = false;

            for (int i = 0; i < cipherText_lenght; i++)
            {
                first = cipherText[m];
                second = cipherText[m + 1];

                if (first == 'j')
                {
                    first = 'i';
                }
                if (second == 'j')
                {
                    second = 'i';
                }
                m += 2;
                two_letter = first.ToString() + second;
                cipherText_matrix[i] = two_letter;
            }



            string plainText = "";
            string first_and_second;
            char first_letter;
            char second_letter;
            int row_of_firstletter = -1;
            int column_of_firstletter = -1;
            int row_of_secondletter = -1;
            int column_of_secondletter = -1;
            bool I = false;


            for (int k = 0; k < cipherText_lenght; k++)
            {
                first_and_second = cipherText_matrix[k];
                first_letter = first_and_second[0];
                second_letter = first_and_second[1];

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (first_letter == playfair_matrix[i, j])
                        {
                            row_of_firstletter = i;
                            column_of_firstletter = j;
                        }

                        if (second_letter == playfair_matrix[i, j])
                        {
                            row_of_secondletter = i;
                            column_of_secondletter = j;
                        }
                    }

                }
                if (row_of_firstletter == row_of_secondletter)
                {
                    if (column_of_firstletter == 0)
                    {
                        column_of_firstletter = 5;
                    }
                    if (column_of_secondletter == 0)
                    {
                        column_of_secondletter = 5;
                    }
                    plainText += playfair_matrix[row_of_firstletter, column_of_firstletter - 1].ToString() + playfair_matrix[row_of_secondletter, column_of_secondletter - 1];
                }
                else if (column_of_firstletter == column_of_secondletter)
                {
                    if (row_of_firstletter == 0)
                    {
                        row_of_firstletter = 5;
                    }
                    if (row_of_secondletter == 0)
                    {
                        row_of_secondletter = 5;
                    }

                    plainText += playfair_matrix[row_of_firstletter - 1, column_of_firstletter].ToString() + playfair_matrix[row_of_secondletter - 1, column_of_secondletter];
                }
                else
                {
                    plainText += playfair_matrix[row_of_firstletter, column_of_secondletter].ToString() + playfair_matrix[row_of_secondletter, column_of_firstletter];
                }

            }

            if (plainText[plainText.Length - 1] == 'x')
            {
                plainText = plainText.Remove(plainText.Length - 1);
            }
            for (int i = 0; i < plainText.Length - 2; i++)
            {
                if ((plainText[i] == plainText[i + 2]) && (plainText[i + 1] == 'x') && (i + 1) % 2 != 0)
                {
                    char[] charArray = plainText.ToCharArray();
                    charArray[i + 1] = '*';
                    plainText = new string(charArray);
                }
            }

            plainText = plainText.Replace("*", "");

            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            if (plainText == null || key == null)
                return "Enter plainText and Key  again";

            key = key.ToLower();

            char[,] playfair_matrix = new char[5, 5];
            string combinedKey = key + "abcdefghijklmnopqrstuvwxyz";
            string filtered_Key = new string(combinedKey.Distinct().ToArray());
            int playfair_matrix_index = 0;


            int row_of_I = 0;
            int column_of_I = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (filtered_Key[playfair_matrix_index] == 'i' || filtered_Key[playfair_matrix_index] == 'j')
                    {
                        if (playfair_matrix[row_of_I, column_of_I] == 'i' || playfair_matrix[row_of_I, column_of_I] == 'j')
                        {
                            playfair_matrix_index++;
                            playfair_matrix[i, j] = filtered_Key[playfair_matrix_index++];
                        }
                        else
                        {
                            playfair_matrix[i, j] = 'i';

                            row_of_I = i;
                            column_of_I = j;
                            playfair_matrix_index++;
                        }
                    }
                    else
                    {
                        playfair_matrix[i, j] = filtered_Key[playfair_matrix_index++];
                    }
                }
            }



            plainText = plainText.Replace(" ", "").ToLower();

            for (int i = 0; i < plainText.Length - 1; i++)
            {
                if (plainText[i] == plainText[i + 1] && (i + 1) % 2 != 0)
                {
                    plainText = plainText.Insert(i + 1, "x");
                }
            }

            if (plainText.Length % 2 != 0)
            {
                plainText += 'x';
            }

            string[] plain_Text_matrix = new string[plainText.Length / 2];
            int plainText_lenght = plainText.Length / 2;


            string two_letter;
            char first;
            char second;

            char x = 'j';

            int m = 0;
            bool dup = false;

            for (int i = 0; i < plainText_lenght; i++)
            {
                first = plainText[m];
                second = plainText[m + 1];

                if (first == 'j')
                {
                    first = 'i';
                }
                if (second == 'j')
                {
                    second = 'i';
                }


                m += 2;

                two_letter = first.ToString() + second;
                plain_Text_matrix[i] = two_letter;
            }





            string cipherText = "";
            string first_and_second;
            char first_letter;
            char second_letter;
            int row_of_firstletter = -1;
            int column_of_firstletter = -1;
            int row_of_secondletter = -1;
            int column_of_secondletter = -1;
            bool I = false;
            string cipherText2 = "";

            for (int k = 0; k < plainText_lenght; k++)
            {
                first_and_second = plain_Text_matrix[k];
                first_letter = first_and_second[0];
                second_letter = first_and_second[1];

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (first_letter == playfair_matrix[i, j])
                        {
                            row_of_firstletter = i;
                            column_of_firstletter = j;
                        }

                        if (second_letter == playfair_matrix[i, j])
                        {
                            row_of_secondletter = i;
                            column_of_secondletter = j;
                        }
                    }

                }
                if (row_of_firstletter == row_of_secondletter)
                {
                    cipherText += playfair_matrix[row_of_firstletter, (column_of_firstletter + 1) % 5].ToString() + playfair_matrix[row_of_secondletter, (column_of_secondletter + 1) % 5];
                }
                else if (column_of_firstletter == column_of_secondletter)
                {
                    cipherText += playfair_matrix[(row_of_firstletter + 1) % 5, column_of_firstletter].ToString() + playfair_matrix[(row_of_secondletter + 1) % 5, column_of_secondletter];
                }
                else
                {
                    cipherText += playfair_matrix[row_of_firstletter, column_of_secondletter].ToString() + playfair_matrix[row_of_secondletter, column_of_firstletter];
                }

            }
            return cipherText;
        }
    }
}
