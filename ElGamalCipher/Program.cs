using ElGamalCipher;
using System;
using System.Numerics;
using System.Text;

string pathAlph = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\inputAlp.txt";
string pathPublicKey = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\publicKey.txt";
string pathPrivateKey = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\privateKey.txt";
string pathMessage = "C:\\Документы\\8Sem\\Kriptografia\\ElGamalCipher\\message.txt";

Dictionary<char, int> alphavit = inputAlphavitAsync(pathAlph).Result;

interfaceUser();

void interfaceUser()
{
    Console.WriteLine($"\n****************************************");
    Console.WriteLine("Введите 1 - если хотите сгенерировать ключи");
    Console.WriteLine("2 - если хотите зашифровать текст");
    Console.WriteLine("3 - если хотите расшифровать текст");
    Console.WriteLine("0 - выход");
    Console.WriteLine($"****************************************");
    int number = Convert.ToInt32(Console.ReadLine());

    if (number == 1)
    {
        GenerationKey();
        interfaceUser();
    }
    else if (number == 2)
    {
        EncryptingText();
    }
    else if (number == 0)
    {
        Console.WriteLine("Exit");
    }
    else
    {
        Console.WriteLine("Error");
    }

}

////////////////////////////////////////////////////////////////////
void GenerationKey()
{
    //82589933  997
    Console.Write("Введите простое число p = ");
    BigInteger p = 0;
    MillerRabinTest millerRabinTest = new MillerRabinTest();
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
    outputText(pathPublicKey, publicKey);
    Console.WriteLine($"--------------");
    Console.WriteLine($"Открытый ключ:\n{publicKey}");

    var privateKey = x.ToString();
    outputText(pathPrivateKey, privateKey);
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

//////////////////////////////////////////////////////////////////////
void EncryptingText()
{
    var plainText = inputText(pathMessage);
    Console.WriteLine($"--------------");
    Console.WriteLine($"Начальное сообщение:\n{plainText}");

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
    var plainTextNumber = ConvertTextToNumber(plainText, alphavit);

}
List<int> ConvertTextToNumber(string text, Dictionary<char, int> alphavit)
{
    List<int> textNumber = new List<int>();
    foreach (var item in text)
    {
        textNumber.Add(alphavit[item]);
    }
    return textNumber;
}

async Task<Dictionary<char, int>> inputAlphavitAsync(string path)
{
    Dictionary<char, int> alphaMap = new Dictionary<char, int>();
    using (FileStream fstream = File.OpenRead(path))
    {
        Random rand = new Random();
        byte[] buffer = new byte[fstream.Length];
        await fstream.ReadAsync(buffer, 0, buffer.Length);
        string textFromFile = Encoding.Default.GetString(buffer);
        int i = 0;
        foreach (var item in textFromFile)
        {
            char curentLetter = item;
            int randomNumber = rand.Next(100, 1000);
            alphaMap[curentLetter] = randomNumber;
            i++;
        }
        alphaMap['\r'] = rand.Next(100, 1000);
        alphaMap['\n'] = rand.Next(100, 1000);
    }
    return alphaMap;
}
string inputText(string path)
{
    string textFromFile = File.ReadAllText(path);
    return textFromFile;
}
void outputText(string path, string Text)
{
    File.WriteAllText(path, Text);
}