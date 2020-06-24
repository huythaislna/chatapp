using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;       

namespace SERVER
{
    class Header
    {
        public static string registerHeader = "registerABCDEF*#!#1234239ik17886381761282121";
        public static string loginHeader = "login#DQ@FGW!122122121";
        public static string loginSuccessHeader = "success328u321!!!!@#$#$";
        public static string loginFailHeader = "a328yidafhkdsjhfas.kjfhda;s";
        public static string errorHeader = "error";
        public static string createRoomHeader = "createabc##";
        public static string chatHeader = "chat#!#AC";
        public static string joinHeader = "joinabc##";
        public static string joinSuccessHeader = "joinsuccessljl3#";
        public static string getGroupNameHeader = "getgroupnameajsi1i2u123123dfjska";
        public static string updateMemberHeader = "updatememberkjdfii123123jfks";
        public static string outRoomHeader = "outroomkajskfdjj123okaf";
        public static string outSuccessHeader = "outsuccess1j2oijfls";
        public static string createRoomSuccess = "creakjkjtiejkjkj12jifasjfdk123123j";
        public static string startChatSession = "start#!@#$";
        public static string adminHeader = "admin3@##@##@a1";
        public static string signOutHeader = "Signout32k32lj32!!";
        public static string signOutSuccess = "Signoutsuccess328i28332!";
        public static string keyExchangeHeader = "key1234523@@@@a!!!!";
    }

    class KeyExchange
    {
        //encrypt
        public static string secretKey = "key";
        public static string EncryptMessage(string message, string key)
        {
            return message;
        }
        public static string DecryptMessage(string message, string key)
        {
            return message;
        }

        //Tham so truyen vao:
        //p = generatePrimeNumber(10);
        //g = findPrimitive(p);
        //privateKey = generatePrivateKey(p)
        //publicKey la cai nhan tu ben kia
        public static int GenerateSecretKey(int p, int g, int privateKey, int publicKey)
        {

            int K = mod_define(publicKey, privateKey, p);
            return K;
        }

        static public int generatePrivateKey(int p)
        {
            Random rnd = new Random();
            int a = rnd.Next(p - 1);
            return a;
        }
        static public int generatePublicKey(int p, int g, int a)
        {
            int A = mod_define(g, a, p);
            return A;

        }
        static public int generatePrimeNumber(int size)
        {
            while (true)
            {
                Random rnd = new Random();
                int num = rnd.Next(Convert.ToInt32(Math.Pow(2, size - 1)), Convert.ToInt32(Math.Pow(2, size)));
                if (isPrime(num))
                    return num;
            }

        }
        static int mod_define(int x, int y, int p)
        {
            int res = 1;
            while (y > 0)
            {
                if (y % 2 != 0)
                    res = (res * x) % p;
                y = y / 2;
                x = (x * x) % p;

            }
            return res;
        }

        //Iterative Function to calculate(x^n)%p in O(logy)
        static int power(int x, int y, int p)
        {
            int res = 1;     // Initialize result 

            x = x % p; // Update x if it is more than or 
                       // equal to p 

            while (y > 0)
            {
                // If y is odd, multiply x with result 
                if (y % 2 == 1)
                {
                    res = (res * x) % p;
                }

                // y must be even now 
                y = y >> 1; // y = y/2 
                x = (x * x) % p;
            }
            return res;
        }

        // Utility function to store prime factors of a number 
        static void findPrimefactors(HashSet<int> s, int n)
        {
            // Print the number of 2s that divide n 
            while (n % 2 == 0)
            {
                s.Add(2);
                n = n / 2;
            }

            // n must be odd at this point. So we can skip 
            // one element (Note i = i +2) 
            for (int i = 3; i <= Math.Sqrt(n); i = i + 2)
            {
                // While i divides n, print i and divide n 
                while (n % i == 0)
                {
                    s.Add(i);
                    n = n / i;
                }
            }

            // This condition is to handle the case when 
            // n is a prime number greater than 2 
            if (n > 2)
            {
                s.Add(n);
            }
        }

        // Function to find smallest primitive root of n 
        static int findPrimitive(int n)
        {
            HashSet<int> s = new HashSet<int>();

            // Check if n is prime or not 
            if (isPrime(n) == false)
            {
                return -1;
            }

            // Find value of Euler Totient function of n 
            // Since n is a prime number, the value of Euler 
            // Totient function is n-1 as there are n-1 
            // relatively prime numbers. 
            int phi = n - 1;

            // Find prime factors of phi and store in a set 
            findPrimefactors(s, phi);

            // Check for every number from 2 to phi 
            for (int r = 2; r <= phi; r++)
            {
                // Iterate through all prime factors of phi. 
                // and check if we found a power with value 1 
                bool flag = false;
                foreach (int a in s)
                {

                    // Check if r^((phi)/primefactors) mod n 
                    // is 1 or not 
                    if (power(r, phi / (a), n) == 1)
                    {
                        flag = true;
                        break;
                    }
                }

                // If there was no power with value 1. 
                if (flag == false)
                {
                    return r;
                }
            }

            // If no primitive root found 
            return -1;
        }

        // Driver code 
        static bool isPrime(int n)
        {

            // Corner cases 
            if (n <= 1 || n == 4)
                return false;
            if (n <= 3)
                return true;

            // Find r such that n = 2^d * r + 1  
            // for some r >= 1 
            int d = n - 1;

            while (d % 2 == 0)
                d /= 2;

            // Iterate given nber of 'k' times 
            for (int i = 0; i < 4; i++)
                if (miillerTest(d, n) == false)
                    return false;

            return true;
        }
        static bool miillerTest(int d, int n)
        {

            // Pick a random number in [2..n-2] 
            // Corner cases make sure that n > 4 
            Random r = new Random();
            int a = 2 + (int)(r.Next() % (n - 4));

            // Compute a^d % n 
            int x = power(a, d, n);

            if (x == 1 || x == n - 1)
                return true;

            // Keep squaring x while one of the 
            // following doesn't happen 
            // (i) d does not reach n-1 
            // (ii) (x^2) % n is not 1 
            // (iii) (x^2) % n is not n-1 
            while (d != n - 1)
            {
                x = (x * x) % n;
                d *= 2;

                if (x == 1)
                    return false;
                if (x == n - 1)
                    return true;
            }

            // Return composite 
            return false;
        }

    }
}
