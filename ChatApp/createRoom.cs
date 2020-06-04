using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ChatApp.Client;
namespace ChatApp
{

    public partial class createRoom : Form
    {
        const string createHeader = "createabc##";
        const string createRoomSuccess = "creakjkjtiejkjkj12jifasjfdk123123j";

        public createRoom()
        {
            InitializeComponent();
        }
        private void SendData(string message)
        {
            byte[] outstream = Encoding.UTF8.GetBytes(message);
            if (stream != null)
            {
                stream.Write(outstream, 0, outstream.Length);
                stream.Flush();
            }
        }
        private void ReceiveMessage()
        {

            try
            {
                var bufferSize = client.ReceiveBufferSize;
                byte[] instream = new byte[bufferSize];
                stream.Read(instream, 0, bufferSize);
                var message = Encoding.UTF8.GetString(instream);
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
                SendData(createHeader + "|" + room_name_tb.Text + "|");
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
                    SendData(createHeader + "|" + room_name_tb.Text + "|");
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
    }
}
