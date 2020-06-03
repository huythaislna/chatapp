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

namespace ChatApp
{
    public partial class ChatWindow : Form
    {

        //new stream and tcpClient
        TcpClient client = null;
        NetworkStream stream = null;


        //declare for setup
        public const string serverIpAddress = "127.0.0.1";
        public const int serverPort = 8080;

        public ChatWindow()
        {
            InitializeComponent();
            connect();
        }

        private void connect()
        {
            Thread stThread = new Thread(Setup);
            stThread.IsBackground = true;
            stThread.Start();
        }
        private void Setup()
        {
            try
            {
                CheckForIllegalCrossThreadCalls = false;
                client = new TcpClient();
                client.Connect(serverIpAddress, serverPort);
                stream = client.GetStream();
                Thread listen = new Thread(listenToServer);
                listen.Start();
            }
            catch
            {
                MessageBox.Show("Can't connect to server");
                this.Close();
            }
        }

        private void listenToServer()
        {
            while (true)
            {
                try
                {
                    var bufferSize = client.ReceiveBufferSize;
                    byte[] instream = new byte[bufferSize];
                    stream.Read(instream, 0, bufferSize);
                    string message = Encoding.UTF8.GetString(instream);
                    stream.Flush();

                    //process message

                    message = message.Substring(0, message.IndexOf("\0"));
                    //update members in room
                    if (message.StartsWith(updateMemberHeader))
                    {
                        member_lv.Items.Clear();
                        string[] member = message.Remove(0, updateMemberHeader.Length).Split('\n');
                        foreach (string m in member)
                        {
                            ListViewItem it = new ListViewItem(m);
                            member_lv.Items.Add(it);
                        }

                    }


                    //out a chat session
                    else if (message.StartsWith(outSuccessHeader))
                    {
                        this.Close();
                    }

                    //signout
                    else if (message.StartsWith(signOutHeader))
                    {
                        SendData(outRoomHeader);
                    }
                    //message chat incoming
                    else if (message.StartsWith(adminHeader)) 
                    {
                        chat_lw.Items.Add(message.Replace(adminHeader, ""));
                        int visibleItems = chat_lw.ClientSize.Height / chat_lw.ItemHeight;
                        chat_lw.TopIndex = Math.Max(chat_lw.Items.Count - visibleItems + 1, 0);
                    }
                     else if (message.StartsWith(chatHeader))
                    {
                        
                        message = message.Replace(chatHeader, "");
                        for (int i = 0; i < message.Length; i += 45)
                        {
                            int numberCharInLine = 45;
                            string partialMessage;
                            try
                            {
                                while (message[i + numberCharInLine] != ' ')
                                {
                                    numberCharInLine--;
                                }
                                partialMessage = message.Substring(i, numberCharInLine);
                            }
                            catch
                            {
                                partialMessage = message.Substring(i, message.Length - i);
                            }
                            chat_lw.Items.Add(partialMessage);
                            int visibleItems = chat_lw.ClientSize.Height / chat_lw.ItemHeight;
                            chat_lw.TopIndex = Math.Max(chat_lw.Items.Count - visibleItems + 1, 0);
                        }
                    }

                }
                catch
                {
                    MessageBox.Show("Get an unexpected error! Try again later");
                    this.Close();
                }
            }
        }
        private void SendData(string message)
        {
            try
            {
                byte[] outstream = Encoding.UTF8.GetBytes(message);
                stream.Write(outstream, 0, outstream.Length);
            }
            catch
            {
                MessageBox.Show("Get an unexpected error! Try again later");
                this.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (message_tb.Text != "")
            {
                SendData(chatHeader + "|" + message_tb.Text);
                message_tb.Text = "";

            }
        }

        private void ChatWindow_Load(object sender, EventArgs e)
        {
            SendData(startChatSession + "|" + Client.username + "|" + Client.room_id);
            //group_name_gb.Text = Client.room_name.ToUpper() + " - ID: " + Client.room_id;
            group_name_gb.Text = Client.room_name.ToUpper() + " - ID: " + Client.room_id;
        }
        private void message_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (message_tb.Text != "")
                {
                    SendData(chatHeader + "|" + message_tb.Text);
                    message_tb.Text = "";

                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            SendData(outRoomHeader);
        }
    }
}
