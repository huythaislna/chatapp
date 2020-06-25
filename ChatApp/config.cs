using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public static int secretKey = 0;
        public static int privateKey = 0;
        public static int publicKey = 0;
        public static string XORCipher(string data, string key)
        {
            int dataLen = data.Length;
            int keyLen = key.Length;
            char[] output = new char[dataLen];

            for (int i = 0; i < dataLen; ++i)
            {
                output[i] = (char)(data[i] ^ key[i % keyLen]);
            }

            Console.WriteLine("Xor-Client: " + new string(output));
            return new string(output);
        }
        public static string EncryptMessage(string plainText, int key)
        {
            return XORCipher(plainText, key.ToString());
            //return plainText;
        }
        //
        public static string DecryptMessage(string plainText, int key)
        {
            return XORCipher(plainText, key.ToString());
            //return message;
        }
        public static int GenerateSecretKey(int p, int privateKey, int publicKey)
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
        static public int generatePublicKey(int p, int g, int a)
        {
            int A = mod_define(g, a, p);
            return A;
        }
    }
}
