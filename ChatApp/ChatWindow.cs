﻿using System;
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
using static SERVER.RSA;

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

                    int length = Int32.Parse(message.Substring(0, 10));
                    message = message.Substring(10, length);
                    Console.WriteLine("Client-received:" + message);
                    if (!message.StartsWith(keyExchangeHeader)) message = Decrypt(message, privateKeyString);
                    Console.WriteLine("Client-decrypt: " + message);

                    //update members in room
                    if (message.StartsWith(keyExchangeHeader))
                    {
                        serverPublicKey = message.Split('|')[1];
                        SendData(keyExchangeHeader + "|" + publicKeyString);
                    } 
                    else if (message.StartsWith(updateMemberHeader))
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
                        //chat_lw.Items.Add(message.Replace(adminHeader, ""));
                        //int visibleItems = chat_lw.ClientSize.Height / chat_lw.ItemHeight;
                        //chat_lw.TopIndex = Math.Max(chat_lw.Items.Count - visibleItems + 1, 0);
                        chat_lw.Text += (message.Replace(adminHeader, ""));
                    }
                     else if (message.StartsWith(chatHeader))
                    {
                        
                        message = message.Replace(chatHeader, "");
                        print(message);
                        
                        //while (message.Length != 0)
                        //{
                        //    int i = break_pos(message);
                        //    if (i != message.Length)
                        //    {
                        //        print(message.Remove(i + 1));
                        //        message = message.Remove(0, i);
                        //    }
                        //    else
                        //    {
                        //        print(message);
                        //        message = "";
                        //    }
                        //}
                    }

                }
                catch
                {
                    MessageBox.Show("Get an unexpected error! Try again later");
                    client.Close();
                    stream.Close();
                    this.Close();
                    return;
                }
            }
        }

        private void SendData(string message)
        {
            try
            {
                if (!message.StartsWith(keyExchangeHeader))
                {
                    message = Encrypt(message, serverPublicKey);
                }
                byte[] length = Encoding.UTF8.GetBytes(message.Length.ToString());
                byte[] lengthHeader = new byte[10];
                length.CopyTo(lengthHeader, 0);
                byte[] noti = Encoding.UTF8.GetBytes(message);
                //stream.Write(noti, 0, noti.Length);
                byte[] sentData = new byte[10 + noti.Length];
                lengthHeader.CopyTo(sentData, 0);
                noti.CopyTo(sentData, 10);
                stream.Write(sentData, 0, sentData.Length);
            }
            catch
            {
                MessageBox.Show("Get an unexpected error! Try again later");
                client.Close();
                stream.Close();
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
            SendData(startChatSession + "|" + Client.username + "|" + Client.room_id);;
            group_name_gb.Text = Client.room_name.ToUpper() + " - ID: " + Client.room_id;
        }
        int shift = 0;
        private void message_tb_KeyDown(object sender, KeyEventArgs e)
        {
            //giống k? k
            //if (e.Shift)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        return;
            //    }
            //}
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (message_tb.Text != "\r\n" && message_tb.Text != "")
            //    {
            //        SendData(chatHeader + "|" + message_tb.Text);
            //    }
            //    message_tb.Text = "";
            //}
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
        private int break_pos(string mess)
        {
            
            if (mess.Length > 42)
            {
                for (int i = 42; i > 0; i--)
                {
                    if (mess[i] == ' ')
                    {
                        return i;
                    }
                }
                return 42;
            }
            return mess.Length;


        }
        private void print(string m)
        {
            //ListViewItem it = new ListViewItem(m);
            //chat_lw.Items.Add(m);
            //int visibleItems = chat_lw.ClientSize.Height / chat_lw.ItemHeight;
            //chat_lw.TopIndex = Math.Max(chat_lw.Items.Count - visibleItems + 1, 0);
            chat_lw.Text += "\r\n" + m;
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
                if (numberOfLine < 15)
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
