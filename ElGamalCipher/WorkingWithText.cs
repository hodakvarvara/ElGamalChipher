using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamalCipher
{
    public class WorkingWithText
    {
        public string ConvertTextToNumber(string text, Dictionary<char, int> alphavit)
        {
            string textNumber = "";
            foreach (var item in text)
            {
                textNumber += alphavit[item];
            }
            return textNumber;
        }

        public async Task<Dictionary<char, int>> inputAlphavitAsync(string path, string path2)
        {
            string inputNumberAlph = inputText(path2);
            Dictionary<char, int> alphaMap = new Dictionary<char, int>();
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);
                int i = 0;
                foreach (var item in textFromFile)
                {
                    char curentLetter = item;
                    alphaMap[curentLetter] = Convert.ToInt32(inputNumberAlph.Substring(i,3));
                    i += 3;
                }
                alphaMap['\r'] = Convert.ToInt32(inputNumberAlph.Substring(i, 3));
                i += 3;
                alphaMap['\n'] = Convert.ToInt32(inputNumberAlph.Substring(i, 3));
            }
            return alphaMap;
        }
        public string inputText(string path)
        {
            string textFromFile = File.ReadAllText(path);
            return textFromFile;
        }
        public void outputText(string path, string Text)
        {
            File.WriteAllText(path, Text);
        }

        public string RemoveCapitalLettersAndEnglishLetters(string plainText, Dictionary<char, int> alphavit)
        {
            plainText = plainText.ToLower();
            for (int i = 0; i < plainText.Length; i++)
            {
                var symbol = plainText[i];
                if (!alphavit.ContainsKey(symbol))
                {
                    plainText = plainText.Remove(i, 1);
                    i--;
                }
            }
            return plainText;
        }
    }
}
