using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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

    class Cipher
    {
        public static string XORCipher(string data)
        {
            string key = "thaihieu";
            int dataLen = data.Length;
            int keyLen = key.Length;
            char[] output = new char[dataLen];

            for (int i = 0; i < dataLen; ++i)
            {
                output[i] = (char)(data[i] ^ key[i % keyLen]);
            }

            return new string(output);
        }
    }
}
