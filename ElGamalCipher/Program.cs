using ElGamalCipher;
using System;
using System.Numerics;
using System.Text;



WorkingWithText text = new WorkingWithText();
ElGamalSheme elGamalSheme = new ElGamalSheme();

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
        elGamalSheme.GenerationKey();
        interfaceUser();
    }
    else if (number == 2)
    {
        elGamalSheme.EncryptingText();
        interfaceUser();
    }
    else if(number == 3)
    {
        elGamalSheme.DecryptionText();
        interfaceUser();
    }
    else if (number == 0)
    {
        Console.WriteLine("Exit");
    }
    else
    {
        Console.WriteLine("Error");
        interfaceUser();
    }

}







