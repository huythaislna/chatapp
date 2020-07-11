using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SERVER.Header;
using static SERVER.Cipher;
using SERVER;

namespace ChatApp
{
    public partial class Client : Form
    {
        //header

        //stream and tcpClient for room
        public static TcpClient client = null;
        public static NetworkStream stream = null;
        public static string username = null;
        public static string room_id = null;
        public static string room_name = null;

        public static string chatIpServer = null;
        //declare for setup
        public const string serverIpAddress = "127.0.0.1";
        public const int serverPort = 8080;

        public Client()
        {
            InitializeComponent();
        }


        //send a message
        private void SendData(string message)
        {
            try
            {
                string length = message.Length.ToString();
                message = String.Format("{0, -10}", length) + message;
                Console.WriteLine(message);
                message = XORCipher(message);

                byte[] noti = Encoding.UTF8.GetBytes(message);
                stream.Write(noti, 0, noti.Length);
            }
            catch
            {
                MessageBox.Show("Get an unexpected error! Try again later");
                this.Close();
            }
        }
        //Setup port, ip,.... and start client
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
            try
            {
                var bufferSize = client.ReceiveBufferSize;
                byte[] instream = new byte[bufferSize];
                stream.Read(instream, 0, bufferSize);
                var message = Encoding.UTF8.GetString(instream);

                //process message

                int length = Int32.Parse(XORCipher(message.Substring(0, 10)));
                message = XORCipher(message.Substring(0, length + 10));
                message = message.Substring(10, length);

                string[] data = message.Split('|');
                switch(data[0])
                {
                    case "LOGIN_SUCCESS":
                        username = user_tb.Text;
                        error_lb.Text = "";
                        client_control.SelectedIndex = 1;
                        break;

                    case "LOGIN_FAIL":
                        error_lb.Text = "⚠ " + data[1];
                        break;

                    case "Username is existed!":
                        username_error_lb.Text = "⚠ " + data[0];
                        signup_btn.Enabled = true;
                        break;

                    case "ERROR":
                        error_id_lb.Text = "⚠ " + data[1];
                        break;

                    case "SIGN_OUT_SUCCESS":
                        client_control.SelectedIndex = 0;
                        Setup();
                        break;

                    case "JOIN_SUCCESS":
                        room_id = Join_tb.Text;
                        room_name = data[1];
                        chatIpServer = data[2].Replace(redirectHeader, "");
                        if (chatIpServer == null)
                            chatIpServer = serverIpAddress;
                        Join_tb.Text = "";
                        error_id_lb.Text = "";
                        ChatWindow cw = new ChatWindow();
                        cw.Show();
                        break;

                    case "Sign up successful!":
                        MessageBox.Show(data[0]);
                        client_control.SelectedIndex = 0;
                        break;

                    case "Password is invalid!":
                        pwd_error_lb.Text = "⚠ " + data[0];
                        break;

                    case "Username is invalid!":
                        username_error_lb.Text = "⚠ " + data[0];
                        break;


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
        private void signup_btn_Click(object sender, EventArgs e)
        {
            client_control.SelectedIndex = 2;
        }

        private void connect()
        {
            Thread stThread = new Thread(Setup);
            stThread.IsBackground = true;
            stThread.Start();
        }

        private void signin_btn_Click(object sender, EventArgs e)
        {
            if (check())
            {
                SendData(loginHeader + "|" + user_tb.Text + "|" + pw_tb.Text + '|');
                listenToServer();
            }
        }

        private void Showpass_CheckedChanged(object sender, EventArgs e)
        {
            if (Showpass.Checked == true)
                pw_tb.UseSystemPasswordChar = false;
            else
                pw_tb.UseSystemPasswordChar = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //SIGNUP signup = new SIGNUP();
            //signup.ShowDialog();
            client_control.SelectedIndex = 2;
        }

        private void pw_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && signin_btn.Enabled == true)
            {
                if (check())
                {
                    SendData(loginHeader + "|" + user_tb.Text + "|" + pw_tb.Text + '|');
                    listenToServer();
                }
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {
            connect();
        }
        private bool check()
        {

            if (user_tb.Text == "")
            {
                error_lb.Text = "⚠ Please enter Username";
                return false;
            }
            else if (pw_tb.Text == "")
            {
                error_lb.Text = "⚠ Please enter Password";
                return false;
            }
            return true;
        }
        private void join_btn_Click(object sender, EventArgs e)
        {
            if (Join_tb.Text == "")
            {
                error_id_lb.Text = "Please enter Room ID!";
                return;
            }
            SendData(joinHeader + "|" + Join_tb.Text + "|" + username + "|");
            listenToServer();
        }

        private void Create_group_btn_Click_1(object sender, EventArgs e)
        {
            createRoom cr = new createRoom();
            cr.ShowDialog();
        }

        private void Join_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Join_tb.Text == "")
                {
                    error_id_lb.Text = "Please enter Room ID!";
                }
                SendData(joinHeader + "|" + Join_tb.Text + "|" + username + "|");
                listenToServer();
            }
        }

        




        //sign up
        private void SIGNUP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (check())
                {
                    try
                    {
                        SendData(registerHeader + "|" + name_tb.Text + "|" + username_tb.Text + "|" + pwd_tb.Text + "|"); ;
                        listenToServer();
                    }
                    catch { }
                }
            }
        }
        private bool signup_check()
        {
            int count = 0;
            if (name_tb.Text == "")
            {
                name_error_lb.Text = "⚠ Please enter display name";
                count++;
            }
            if (username_tb.Text == "")
            {
                username_error_lb.Text = "⚠ Please enter username";
                count++;
            }

            if (pwd_tb.Text == "")
            {
                pwd_error_lb.Text = "⚠ Please enter Password";
                count++;
            }
            if (pwd_tb.Text != pw2_tb.Text)
            {
                pw2_error_lb.Text = ("⚠ Password is not matched");
                count++;
            }

            if (count != 0)
                return false;
            return true;
        }



        //enter event
        private void username_tb_Enter(object sender, EventArgs e)
        {
            username_error_lb.Text = "";
        }

        private void name_tb_Enter(object sender, EventArgs e)
        {
            name_error_lb.Text = "";
        }

        private void pwd_tb_Enter(object sender, EventArgs e)
        {
            pwd_error_lb.Text = "";
        }

        private void pw2_tb_Enter(object sender, EventArgs e)
        {
            pw2_error_lb.Text = "";
        }
       

        private void signup_btn_Click_1(object sender, EventArgs e)
        {

            if (signup_check())
            {
                SendData(registerHeader + "|" + name_tb.Text + "|" + username_tb.Text + "|" + pwd_tb.Text + "|"); ;
                listenToServer();
            }
            else
            {
                signup_btn.Enabled = true;
            }

        }

        private void exit_bt_Click(object sender, EventArgs e)
        {
            client_control.SelectedIndex = 0;
        }

        private void exit_client_tb_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //move without border
        bool flag = false;
        int movX = 0;
        int movY = 0;

        private void Form1_MouseDown(object sender, MouseEventArgs e)

        {

            flag = true;

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)

        {

            //Check if Flag is True ??? if so then make form position same

            //as Cursor position

            if (flag == true)

            {

                //this.Location = Cursor.Position;
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);

            }

        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)

        {

            flag = false;

        }

        private void exit_pb_Click(object sender, EventArgs e)
        {
            SendData(signOutHeader + "|" + username + "|");
            this.Close();
        }

        private void pw2_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (signup_check())
                {
                    SendData(registerHeader + "|" + name_tb.Text + "|" + username_tb.Text + "|" + pwd_tb.Text + "|"); ;
                    listenToServer();
                }
                else
                {
                    signup_btn.Enabled = true;
                }
            }
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            client_control.SelectedIndex = 0;
        }

        private void signout_pt_Click(object sender, EventArgs e)
        {
            SendData(signOutHeader + "|" + username + "|");
            listenToServer();
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            SendData(signOutHeader + "|" + username + "|");
        }
    }
}


