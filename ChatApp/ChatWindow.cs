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
using static SERVER.Header;
using static SERVER.Cipher;
using static ChatApp.Client;

namespace ChatApp
{
    public partial class ChatWindow : Form
    {

        //new stream and tcpClient


        //declare for setup
        public string chatIpServer = "192.168.1.138";
        public int port = 9999;

        public ChatWindow()
        {
            InitializeComponent();
           // connect();
        }

        //private void connect()
        //{
        //    Thread stThread = new Thread(Setup);
        //    stThread.IsBackground = true;
        //    stThread.Start();
        //}
        //private void Setup()
        //{
        //    try
        //    {
        //        CheckForIllegalCrossThreadCalls = false;
        //        client = new TcpClient();
        //        client.Connect(chatIpServer, port);
        //        stream = client.GetStream();
        //        Thread listen = new Thread(listenToServer);
        //        listen.IsBackground = true;
        //        listen.Start();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Can't connect to server");
        //        this.Close();
        //    }
        //}

        private void listen()
        {

            while (true)
            {
                //try
                //{
                    var bufferSize = client.ReceiveBufferSize;
                    byte[] instream = new byte[bufferSize];
                    Client.client.GetStream().Read(instream, 0, bufferSize);
                    string message = Encoding.UTF8.GetString(instream);
                //stream.Flush();

                //process message
                //decrypt incoming message
                int length = Int32.Parse(XORCipher(message.Substring(0, 10)));
                message = XORCipher(message.Substring(0, length + 10));
                message = message.Substring(10, length);
                    Console.WriteLine("windows chat: " + message);

                    string[] data = message.Split('|');
                    switch(data[0])
                    {

                        case "UPDATE_MEMBER":
                            printMember(data[1]);
                            break;

                        case "OUT_ROOM_SUCCESS":
                            this.Close();
                            return;
                            break;
                        
                        case "SIGN_OUT":
                            SendData(outRoomHeader);
                            break;

                        case "CHAT":
                            print(message.Replace(chatHeader + '|', ""));
                            break;
                        
                        //case "REDIRECT":
                        //    client.Close();
                        //    stream.Close();
                        //    chatIpServer = data[1];
                        //    Console.WriteLine("ip: " + data[1]);
                        //    port = Int32.Parse(data[2]);
                        //    Console.WriteLine("port: " + data[2]);
                        //    Setup();
                        //    break;
                }
                //}
                //catch
                //{
                //    MessageBox.Show("Get an unexpected error! Try again later");
                //    client.Close();
                //    stream.Close();
                //    this.Close();
                //    return;
                //}
            }
        }
        private void printMember(string data)
        {
            foreach (string m in data.Split('\n'))
            {
                
                foreach (var i in member_lv.Items)
                {
                    if(i.ToString() != m)
                    {
                        ListViewItem it = new ListViewItem(m);
                        member_lv.Items.Add(it);

                    }
                }

                
            }
        }
        private void SendData(string message)
        {
            try
            {
                if (message.Length > 2500)
                {
                    MessageBox.Show("Message is two long!");
                    return;
                }
                string length = message.Length.ToString();
                message = String.Format("{0, -10}", length) + message;
                Console.WriteLine(message);
                message = XORCipher(message);

                byte[] noti = Encoding.UTF8.GetBytes(message);
                Client.client.GetStream().Write(noti, 0, noti.Length);
            }
            catch
            {
                MessageBox.Show("Get an unexpected error! Try again later");
                client.Close();
                stream.Close();
                this.Close();
            }
        }

        private void ChatWindow_Load(object sender, EventArgs e)
        {
            print("Admin: Welcome to room " + Client.room_name);
            Thread t1 = new Thread(listen);
            t1.IsBackground = true;
            t1.Start();
            SendData(startChatSession + "|" + Client.username + "|" + Client.room_id);
            group_name_gb.Text = Client.room_name.ToUpper() + " - ID: " + Client.room_id;
        }

        //shift enter
        int shift = 0;
        private void message_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                shift = 1;
                Console.WriteLine("Shift");
            }
            if (e.KeyCode == Keys.Enter && shift == 0)
            {
                e.SuppressKeyPress = true;
                if (message_tb.Text != "")
                {
                    SendData(chatHeader + "|" + message_tb.Text);
                    message_tb.Text = "";

                }
            }
            else { shift = 0; }
        }

        private void print(string m)
        {
            chat_lw.Text += m + "\r\n";
        }
        private void sendBt_Click(object sender, EventArgs e)
        {
            if (message_tb.Text != "")
            {
                if (message_tb.Text != "")
                {
                    SendData(chatHeader + "|" + message_tb.Text);
                    message_tb.Text = "";

                }
            }

        }

        private void exit_bt_Click(object sender, EventArgs e)
        {
            SendData(outRoomHeader);
        }

        private void chat_lw_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            chat_lw.SelectionStart = chat_lw.Text.Length;
            // scroll it automatically
            chat_lw.ScrollToCaret();
        }
        

        private void message_tb_TextChanged(object sender, EventArgs e)
        {
            int heightInit = 22;
            Point locationInit = new Point(171, 327);
            int numberOfLine = (message_tb.Text.Split('\n').Length + 1);
            int a = message_tb.Height;
            Point location = message_tb.Location;
            if (message_tb.Text == "" || message_tb.Text == "\r\n")
            {
                message_tb.Height = heightInit;
                message_tb.Location = locationInit;
            }
            else
            {
                if (numberOfLine < 8)
                {
                    message_tb.ScrollBars = ScrollBars.None;
                    if (numberOfLine == 2)
                    {
                        message_tb.Height = heightInit;
                    }
                    else
                        message_tb.Height = heightInit + (numberOfLine - 2) * message_tb.Font.Height;
                    //message_tb.Height =  (numberOfLine ) * message_tb.Font.Height;
                    if (a < message_tb.Height)
                        message_tb.Location = new Point(message_tb.Location.X, message_tb.Location.Y - message_tb.Font.Height);
                    else if (a > message_tb.Height)
                        message_tb.Location = new Point(message_tb.Location.X, message_tb.Location.Y + message_tb.Font.Height);

                }
                else
                {
                    message_tb.ScrollBars = ScrollBars.Both;
                }
            }
        }
    }
}
