using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using BCrypt.Net;
using MongoDB.Driver.Core.Authentication;
using System.Text.RegularExpressions;
using static SERVER.Header;
using static SERVER.KeyExchange;

namespace SERVER
{

    public partial class Server : Form
    {
        public class User
        {
            public string Username { get; set; }
            public TcpClient UserConnection { get; set; }
            public string Display_name { get; set; }
            public string Room_id { get; set; }

            public string Room_name { get; set; }

        }


        IPAddress ipAddress;
        Int32 port = 8080;
        TcpListener TcpServer;
        public MongoClient dbClient;
        public IMongoDatabase database;

        static readonly Dictionary<string, TcpClient> signedInUsers = new Dictionary<string, TcpClient>(); //username - tcpclient.
        static readonly List<User> usersInRoom = new List<User>();


        public Server()
        {
            InitializeComponent();
        }

        //Setup port, ip,.... and start server
        public string get_display_name(string username)
        {
            var usersCollection = database.GetCollection<BsonDocument>("users");
            var firstDocument = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", username)).FirstOrDefault();
            if (firstDocument != null) return firstDocument.GetElement("name").Value.ToString();
            return "";
        }

        public User getUser(TcpClient client)
        {
            foreach (var user in usersInRoom)
            {
                if (user.UserConnection == client)
                {
                    return user;
                }
            }
            return null;
        }
        private void Setup()
        {
            CheckForIllegalCrossThreadCalls = false;
            dbClient = new MongoClient("mongodb://127.0.0.1:27017/");
            database = dbClient.GetDatabase("chatapp");
            ipAddress = IPAddress.Parse("127.0.0.1");
            TcpServer = new TcpListener(IPAddress.Any, port);
            TcpServer.Start();
            //status.Text = "Waiting for a connection...";
            Thread serverThread = new Thread(waitForClient);
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        //waiting for client and accept connection
        private void waitForClient()
        {
            while (true)
            {
                TcpClient client;
                client = TcpServer.AcceptTcpClient();
                var t = new Thread(new ParameterizedThreadStart(Listen));
                t.IsBackground = true;
                t.Start(client);
            }
        }

        //listen to client for sending and receiving
        private void Listen(object Client)
        {
            TcpClient client = (TcpClient)Client;
            NetworkStream stream = client.GetStream();
            SendData(EncryptMessage("Connected to server!", secretKey), client);
            while (true)
            {
                if (stream.CanRead != true || stream.CanWrite != true) break;
                try
                {
                    string message = ReceiveData(stream, client);
                    message = message.Substring(0, message.IndexOf('\0'));

                    //prcessing
                    string[] data = message.Split('|');

                    //key exchange
                    if (message.StartsWith(keyExchangeHeader))
                    {
                        secretKey = GenerateSecretKey(1, 2, 3, 4).ToString();
                    }
                    //sign up
                    if (message.StartsWith(registerHeader))
                    {
                        register(data[1], data[2], data[3], client);
                    }
                    //login
                    else if (message.StartsWith(loginHeader))
                    {
                        bool isUsed = false;
                        foreach (var user in signedInUsers)
                        {
                            if (user.Key == data[1])
                            {
                                SendData(loginFailHeader + "This account have been being used", client);
                                isUsed = true;
                                break;
                            }
                        }
                        if (authenticate(data[1], data[2]) == true && isUsed == false)
                        {
                            SendData(loginSuccessHeader, client);
                            signedInUsers.Add(data[1], client);
                        }
                        else SendData(loginFailHeader + "Username or Password was wrong", client);
                    }
                    //signout
                    else if (message.StartsWith(signOutHeader))
                    {
                        signedInUsers.Remove(data[1]);
                        foreach(var user in usersInRoom)
                        {
                            if (user.Username == data[1])
                            {
                                SendData(signOutHeader, user.UserConnection);
                            }
                        }
                        SendData(signOutSuccess, client);
                        client.Close();
                        stream.Close();
                    }

                    //createroom
                    else if (message.StartsWith(createRoomHeader))
                    {
                        string room_id = createRoomId();
                        var usersCollection = database.GetCollection<BsonDocument>("rooms");
                        usersCollection.InsertOne(
                            new BsonDocument {
                                { "id",  room_id},
                                { "name",  data[1]},
                        });
                        SendData(createRoomSuccess + room_id, client);
                    }
                    //start chat - initial user 
                    else if (message.StartsWith(startChatSession))
                    {
                        User user = new User
                        {
                            UserConnection = client,
                            Username = data[1],
                            Display_name = get_display_name(data[1]),
                            Room_id = data[2],
                            Room_name = getRoomName(data[2])
                        };
                        sendToRoom(adminHeader + "Admin: " + user.Display_name + " joined !!", user.Room_id);
                        SendData(adminHeader + "Admin: Welcome! " + user.Display_name, client);
                        usersInRoom.Add(user);
                        updateMember(client);
                    }

                    //chat message
                    else if (message.StartsWith(chatHeader))
                    {
                        sendToRoom(chatHeader + getUser(client).Display_name + ": " + data[1], getUser(client).Room_id);
                    }


                    //join room
                    else if (message.StartsWith(joinHeader))
                    {
                        if (IsExistedRoom(data[1]) == true)
                        {
                            if (checkUserInRoom(data[1], data[2]) == false)
                            {
                                SendData(joinSuccessHeader + getRoomName(data[1]), client);
                            } else
                            {
                                SendData(errorHeader + "You have already been in room", client);
                            }
                        }
                        else
                        {
                            SendData(errorHeader + "Room is not existed", client);
                        }
                    }
                    //update members
                    else if (message.StartsWith(updateMemberHeader))
                    {
                        updateMember(client);
                    }

                    //out room
                    else if (message.StartsWith(outRoomHeader))
                    {
                        try
                        {
                            SendData(outSuccessHeader, client);
                            var user = getUser(client);
                            usersInRoom.Remove(user);
                            sendToRoom(adminHeader + "Admin: " + user.Display_name + " left!!", user.Room_id);

                            string listMember = "";
                            foreach (User member in usersInRoom)
                            {
                                if (user != member)
                                    if (user.Room_id == member.Room_id)
                                        listMember += member.Display_name + '\n';
                            }
                            sendToRoom(updateMemberHeader + listMember, user.Room_id);
                        }
                        catch { }
                    }
                }
                catch
                {
                    client.Close();
                    stream.Close();
                    return;
                }
            }

        }

        private string getRoomName(string roomId)
        {
            var roomsCollection = database.GetCollection<BsonDocument>("rooms");
            var firstDocument = roomsCollection.Find(Builders<BsonDocument>.Filter.Eq("id", roomId)).FirstOrDefault();
            if (firstDocument == null) { return ""; }
            return firstDocument.GetValue("name").ToString();
        }
        private bool checkUserInRoom(string room_id, string username)
        {
            foreach(var user in usersInRoom)
            {
                if (user.Room_id == room_id && username == user.Username)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsExistedRoom(string roomId)
        {
            var roomsCollection = database.GetCollection<BsonDocument>("rooms");
            var firstDocument = roomsCollection.Find(Builders<BsonDocument>.Filter.Eq("id", roomId)).FirstOrDefault();
            if (firstDocument != null) { return true; }
            else return false;
        }
        private void sendToRoom(string message, string room_id)
        {
            foreach (var user in usersInRoom)
            {
                if (user.Room_id == room_id)
                {
                    SendData(message, user.UserConnection);
                }
            }
        }

        private void updateMember(TcpClient client)
        {
            string listMember = "";
            foreach (User member in usersInRoom)
            {
                if (getUser(client).Room_id == member.Room_id)
                    listMember += member.Display_name + '\n';
            }
            sendToRoom(updateMemberHeader + listMember, getUser(client).Room_id);
        }
        private string createRoomId()
        {
            string room_id;
            while (true)
            {
                room_id = Guid.NewGuid().ToString("N").Substring(0, 6);
                var roomsCollection = database.GetCollection<BsonDocument>("rooms");
                var firstDocument = roomsCollection.Find(Builders<BsonDocument>.Filter.Eq("id", room_id)).FirstOrDefault();
                if (firstDocument == null) break;
            }
            return room_id;
        }

        private bool authenticate(string username, string pwd)
        {
            //return true;
            var usersCollection = database.GetCollection<BsonDocument>("users");
            var firstDocument = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", username)).FirstOrDefault();
            if (firstDocument == null) return false;
            var isMatchPwd = BCrypt.Net.BCrypt.Verify(pwd, firstDocument.GetValue("password").ToString());
            return isMatchPwd;
        }

        private bool register(string name, string username, string pwd, TcpClient client)
        {
            // return true
            var usernameRegex = new Regex(@"[a-z\d]{6,15}");
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{6,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var usersCollection = database.GetCollection<BsonDocument>("users");
            var firstDocument = usersCollection.Find(Builders<BsonDocument>.Filter.Eq("username", username)).FirstOrDefault();
            if (firstDocument != null)
            {
                SendData("Username is existed!", client);
                return false;
            }
            if (usernameRegex.IsMatch(username) == false || 
                hasUpperChar.IsMatch(username) == true)
            {
                SendData("Username is invalid!", client);
                return false;
            }
            if ((hasNumber.IsMatch(pwd) && hasUpperChar.IsMatch(pwd) && 
                hasMiniMaxChars.IsMatch(pwd) && hasLowerChar.IsMatch(pwd)) == false) 
            {
                SendData("Password is invalid!", client);
                return false;
            }
            usersCollection.InsertOne(
                new BsonDocument {
                    { "name",  name},
                    { "username", username },
                    { "password", BCrypt.Net.BCrypt.HashPassword(pwd)}
                }
            );
            SendData("Sign up successful!", client);
            return true;
        }

        //send a message
        private void SendData(string message, object clientObj)
        {
            Console.WriteLine("Server: " + message);
            TcpClient client = clientObj as TcpClient;
            NetworkStream stream = client.GetStream();
            if (!message.StartsWith(keyExchangeHeader))
                message = EncryptMessage(message, secretKey);
            byte[] noti = Encoding.UTF8.GetBytes(message);
            stream.Write(noti, 0, noti.Length);
        }

        //get message from client
        private string ReceiveData(NetworkStream str, TcpClient client)
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            str.Read(buffer, 0, buffer.Length);

            string mess = Encoding.UTF8.GetString(buffer);
            if (!mess.StartsWith(keyExchangeHeader)) {
                mess = DecryptMessage(mess, secretKey);
            }
            return mess;

        }
        private void start_btn_Click(object sender, EventArgs e)
        {
            start_btn.Enabled = false;
            start_btn.BackColor = System.Drawing.Color.Gray;
            Setup();
            power_lb.Text = "ON";
            power_lb.ForeColor = System.Drawing.Color.Green;
        }

        private void Server_Load(object sender, EventArgs e)
        {
            //Setup();
        }
    }
}