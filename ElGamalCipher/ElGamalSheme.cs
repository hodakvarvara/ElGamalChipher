using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ElGamalCipher
{
    public class ElGamalSheme
    {
        string pathPublicKey = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\publicKey.txt";
        string pathPrivateKey = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\privateKey.txt";
        string pathMessage = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\message.txt";
        string pathEncryptedMessage = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\encryptedMessage.txt";
        string pathDecryptedMessage = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\decryptedMessage.txt";
        string pathAlph = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\inputAlp.txt";
        string pathAlphNumber = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\inputAlphNumber.txt";

        WorkingWithText text = new WorkingWithText();
        MillerRabinTest millerRabinTest = new MillerRabinTest();

        public void GenerationKey()
        {
            //82589933  997
            Console.Write("Введите простое число p = ");
            BigInteger p = 0;
            while (true)
            {
                var input = Console.ReadLine();
                BigInteger.TryParse(input, out p);
                if (millerRabinTest.Test(p, 10))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Число не является простым, повторите попытку");
                }

            }

            var g = primitiveElementG(p);
            var x = millerRabinTest.random(p);
            var y = BigInteger.ModPow(g, x, p);

            var publicKey = p.ToString() + " " + g.ToString() + " " + y.ToString();
            text.outputText(pathPublicKey, publicKey);
            Console.WriteLine($"--------------");
            Console.WriteLine($"Открытый ключ:\n{publicKey}");

            var privateKey = x.ToString();
            text.outputText(pathPrivateKey, privateKey);
            Console.WriteLine($"--------------");
            Console.WriteLine($"Закрытый ключ:\n{privateKey}");
        }
        BigInteger primitiveElementG(BigInteger p)
        {
            BigInteger primitiveElement;
            for (primitiveElement = 2; primitiveElement < p; primitiveElement++)
            {
                if (IsPrimitiveRoot(primitiveElement, p))
                {
                    break;
                }
            }
            //while (true)
            //{
            //    primitiveElement = millerRabinTest.random(p);
            //    if (IsPrimitiveRoot(primitiveElement, p))
            //    {
            //        break;
            //    }
            //}
            return primitiveElement;
        }
        static bool IsPrimitiveRoot(BigInteger primitiveElement, BigInteger p)
        {
            HashSet<BigInteger> uniqueNumbers = new HashSet<BigInteger>();
            for (int i = 1; i < p - 1; i++)
            {
                BigInteger num = BigInteger.ModPow(primitiveElement, i, p);
                if (num == 1 || uniqueNumbers.Contains(num)) { return false; }
                else { uniqueNumbers.Add(num); }
            }
            return true;
        }

        public void EncryptingText()
        {
            Dictionary<char, int> alphavit = text.inputAlphavitAsync(pathAlph, pathAlphNumber).Result;

            var plainText = text.inputText(pathMessage);
            Console.WriteLine($"--------------");
            Console.WriteLine($"Начальное сообщение:\n{plainText}");

            plainText = text.RemoveCapitalLettersAndEnglishLetters(plainText, alphavit);
            string plainTextNumber = text.ConvertTextToNumber(plainText, alphavit);
            Console.WriteLine($"--------------");
            Console.WriteLine($"Начальное сообщение в цифрах:\n{plainTextNumber}");

            string str = text.inputText(pathPublicKey);
            string[] words = str.Split(' ');
            BigInteger p;
            BigInteger g;
            BigInteger y;
            BigInteger.TryParse(words[0], out p);
            BigInteger.TryParse(words[1], out g);
            BigInteger.TryParse(words[2], out y);

            List<BigInteger> M = new List<BigInteger>();
            string strNewTextNumber = new string(plainTextNumber);
            while (strNewTextNumber != "")
            {
                var i = 1;
                BigInteger l1;
                BigInteger l2 = 0;
                BigInteger.TryParse(strNewTextNumber.Substring(0, i), out l1);
                while(l1 < p)
                {
                    l2 = l1;
                    i++;
                    if(strNewTextNumber.Length >= i)
                    {
                        BigInteger.TryParse(strNewTextNumber.Substring(0, i), out l1);
                    }
                    else { break; }
                }
                M.Add(l2);
                strNewTextNumber = strNewTextNumber.Remove(0, i-1);
            }

            BigInteger k;
            while (true)
            {
                k = millerRabinTest.random(p);
                BigInteger gcd = BigInteger.GreatestCommonDivisor(k, p);
                if (gcd == 1)
                {
                    break;
                }
            }

            BigInteger A = BigInteger.ModPow(g, k, p);
            string encryptedText = A.ToString() + " ";

            List<BigInteger> B = new List<BigInteger>();
            foreach(var item in M)
            {
                var s = BigInteger.Remainder(BigInteger.Pow(y, (int)k) * item,p);
                B.Add(s);
                encryptedText += s.ToString() + " ";
            }

            text.outputText(pathEncryptedMessage, encryptedText);
            Console.WriteLine($"--------------");
            Console.WriteLine($"Криптограмма:\n{encryptedText}");
        }

        public void DecryptionText()
        {
            Dictionary<char, int> alphavit = text.inputAlphavitAsync(pathAlph, pathAlphNumber).Result;
            string M = "";
            List<BigInteger> B = new List<BigInteger>(); 
            string decryptedTextStr = "";

            string str = text.inputText(pathPublicKey);
            string[] words = str.Split(' ');
            BigInteger p;
            BigInteger g;
            BigInteger y;
            BigInteger.TryParse(words[0], out p);
            BigInteger.TryParse(words[1], out g);
            BigInteger.TryParse(words[2], out y);

            string str1 = text.inputText(pathPrivateKey);
            words = str1.Split(' ');
            BigInteger X;
            BigInteger.TryParse(words[0], out X);

            string AB = text.inputText(pathEncryptedMessage);
            words = AB.Split(' ');
            BigInteger A;
            BigInteger.TryParse(words[0], out A);
            for(int i = 1; i < words.Length - 1; i++)
            {
                BigInteger number;
                BigInteger.TryParse(words[i], out number);
                B.Add(number);
                var ttt = BigInteger.Remainder(number * BigInteger.Pow(A, (int)(p - 1 - X)), p).ToString(); 
                M += ttt;
                //Console.Write(ttt);
            }

            for (int i = 0; i < M.Length; i += 3)
            {
                try
                {
                    var tirimpirim = M.Substring(i, 3);
                    var value = Convert.ToInt32(tirimpirim);
                    decryptedTextStr += alphavit.FirstOrDefault(x => x.Value == value).Key;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            text.outputText(pathDecryptedMessage, decryptedTextStr);
            Console.WriteLine($"--------------");
            Console.WriteLine($"Расшифрованный текст:\n{decryptedTextStr}");

        }

    }
}
