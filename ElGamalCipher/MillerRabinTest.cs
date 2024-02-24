using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElGamalCipher
{
    public class MillerRabinTest
    {
        public bool Test(BigInteger number, int k)
        {
            if (number == 2 || number == 3) { return true; }
            if (number < 2 || number % 2 == 0) { return false; }


            BigInteger t = number - 1;
            int s = 0;
            while(t % 2 == 0)
            {
                t /= 2;
                s++;
            }
            for(int i = 0; i < k; i++)
            {
                BigInteger a;
                a = random(number);
                BigInteger x = BigInteger.ModPow(a, t, number);
                if (x == 1 || x == number - 1)
                    continue;
                for(int j = 0; j < s; j++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if(x == 1)
                    {
                        return false;
                    }
                    if(x == number - 1) { break; }
                }
                if (x != number - 1) { return false; }
            }
            return true;
        }
        public BigInteger random(BigInteger number)
        {
            BigInteger a;
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] randomNumber = new byte[number.ToByteArray().LongLength]; // Например, 32 байта
                do
                {
                    rng.GetBytes(randomNumber);
                    a = new BigInteger(randomNumber);
                } while (a < 2 || a > number - 2);

            }
            return a;
        }
    }
}
