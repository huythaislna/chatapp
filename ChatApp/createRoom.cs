using System;
using System.Text;
using System.Windows.Forms;
using static ChatApp.Client;
using static SERVER.Cipher;
using static SERVER.Header;
namespace ChatApp
{

    public partial class createRoom : Form
    {

        public createRoom()
        {
            InitializeComponent();
        }
        private void SendData(string message)
        {
            string length = message.Length.ToString();
            message = String.Format("{0, -10}", length) + message;
            Console.WriteLine(message);
            message = XORCipher(message);

            byte[] noti = Encoding.UTF8.GetBytes(message);
            stream.Write(noti, 0, noti.Length);
        }
        private void ReceiveMessage()
        {

            try
            {
                var bufferSize = client.ReceiveBufferSize;
                byte[] instream = new byte[bufferSize];
                stream.Read(instream, 0, bufferSize);
                var message = Encoding.UTF8.GetString(instream);

                int length = Int32.Parse(XORCipher(message.Substring(0, 10)));
                message = XORCipher(message.Substring(0, length + 10));
                message = message.Substring(10, length);

                Console.WriteLine("Client-decrypt: " + message);


                if (message.StartsWith(createRoomSuccess))
                {                    
                    room_id_lb.Text = message.Remove(0, createRoomSuccess.Length);
                    id_pb.Visible = true;
                    room_id_lb.Visible = true;
                    id_lb.Visible = true;
                    copy_btn.Visible = true;
                    copy_btn.Enabled = true;
                    copy_lb.Visible = false;
                }
                else 
                {
                    MessageBox.Show("Create Room Unsuccessfuly!");
                }
            }
            catch
            {
                
                stream.Close();
                client.Close();
                return;
            }

        }

        private void create_btn_Click(object sender, EventArgs e)
        {
            if (room_name_tb.Text != "")
            {
                SendData(createRoomHeader + "|" + room_name_tb.Text + "|");
                ReceiveMessage();
            }
            else
                errror_create_lb.Visible = true;
        }

        private void room_name_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (room_name_tb.Text != "")
                {
                    SendData(createRoomHeader + "|" + room_name_tb.Text + "|");
                    ReceiveMessage();

                }
                else
                    errror_create_lb.Visible = true;
            }
        }

        private void copy_btn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(room_id_lb.Text);
            copy_btn.Enabled = false;
            copy_lb.Visible = true;

        }

        private void exit_pb_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void room_name_tb_Enter(object sender, EventArgs e)
        {
            errror_create_lb.Visible = false;
        }

        private void room_name_tb_TextChanged(object sender, EventArgs e)
        {
            errror_create_lb.Visible = false;
        }


        //move form
        bool flag = false;
        int movX = 0;
        int movY = 0;
        private void createRoom_MouseDown(object sender, MouseEventArgs e)
        {
            flag = true;
            movX = e.X;
            movY = e.Y;
        }

        private void createRoom_MouseMove(object sender, MouseEventArgs e)
        {
            if (flag == true)
            {
                //this.Location = Cursor.Position;
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void createRoom_MouseUp(object sender, MouseEventArgs e)
        {
            flag = false;
        }
    }
}
