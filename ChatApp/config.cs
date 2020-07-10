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
        public static string keyExchangeHeader = "KEY_EXCHANGE";
        public static string registerHeader = "REGISTER";
        public static string loginHeader = "LOGIN";
        public static string loginSuccessHeader = "LOGIN_SUCCESS";
        public static string loginFailHeader = "LOGIN_FAIL";

        public static string errorHeader = "ERROR";
        public static string createRoomHeader = "CREATE_ROOM";
        public static string createRoomSuccess = "CREATE_ROOM_SUCCESS";
        public static string outRoomHeader = "OUT_ROOM";
        public static string outSuccessHeader = "OUT_ROOM_SUCCESS";
        public static string joinHeader = "JOIN";
        public static string joinSuccessHeader = "JOIN_SUCCESS";
        public static string getGroupNameHeader = "GET_ROOM_NAME";
        public static string updateMemberHeader = "UPDATE_MEMBER";
        public static string redirectHeader = "REDIRECT";


        public static string chatHeader = "CHAT";
        public static string startChatSession = "START_CHAT_SESSION";
        public static string adminHeader = "ADMIN";
        public static string signOutHeader = "SIGN_OUT";
        public static string signOutSuccess = "SIGN_OUT_SUCCESS";
    }

    class RSA
    {
        public static RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(1024); 
        public static RSAParameters privateKey = cryptoServiceProvider.ExportParameters(true);
        public static RSAParameters publicKey = cryptoServiceProvider.ExportParameters(false);
        public static string publicKeyString = GetKeyString(publicKey);
        public static string privateKeyString = GetKeyString(privateKey);
        public static string serverPublicKey = "";

        public static string Encrypt(string textToEncrypt, string publicKeyString)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);

            using (var rsa = new RSACryptoServiceProvider(1024))
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

            using (var rsa = new RSACryptoServiceProvider(1024))
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
