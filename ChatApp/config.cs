using System;
using System.Collections.Generic;
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
        public static string keyExchangeHeader = "keyexchange123#######";
    }

    class RSA
    {
        public static RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(2048); //2048 - Długość klucza
        public static RSAParameters privateKey = cryptoServiceProvider.ExportParameters(true); //Generowanie klucza prywatnego
        public static RSAParameters publicKey = cryptoServiceProvider.ExportParameters(false);
        public static string publicKeyString = GetKeyString(publicKey);
        public static string privateKeyString = GetKeyString(privateKey);
        public static string serverPublicKey = "";

        public static string Encrypt(string textToEncrypt, string publicKeyString)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    rsa.FromXmlString(publicKeyString);
                    var encryptedData = rsa.Encrypt(bytesToEncrypt, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static string GetKeyString(RSAParameters publicKey)
        {
            var stringWriter = new System.IO.StringWriter();
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(stringWriter, publicKey);
            return stringWriter.ToString();
        }

        public static string Decrypt(string textToDecrypt, string privateKeyString)
        {
            var bytesToDescrypt = Encoding.UTF8.GetBytes(textToDecrypt);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {

                    // server decrypting data with private key                    
                    rsa.FromXmlString(privateKeyString);

                    var resultBytes = Convert.FromBase64String(textToDecrypt);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData.ToString();
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}
