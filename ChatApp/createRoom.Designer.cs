namespace ChatApp
{
    partial class createRoom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(createRoom));
            this.create_btn = new System.Windows.Forms.Button();
            this.room_name = new System.Windows.Forms.Label();
            this.room_name_tb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.id_pb = new System.Windows.Forms.PictureBox();
            this.id_lb = new System.Windows.Forms.Label();
            this.copy_btn = new System.Windows.Forms.Button();
            this.room_id_lb = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.id_pb)).BeginInit();
            this.SuspendLayout();
            // 
            // create_btn
            // 
            this.create_btn.BackgroundImage = global::ChatApp.Properties.Resources._100790964_686014118855549_1343039108137615360_n;
            this.create_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.create_btn.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.create_btn.Location = new System.Drawing.Point(90, 225);
            this.create_btn.Name = "create_btn";
            this.create_btn.Size = new System.Drawing.Size(192, 41);
            this.create_btn.TabIndex = 7;
            this.create_btn.Text = "CREATE";
            this.create_btn.UseVisualStyleBackColor = true;
            this.create_btn.Click += new System.EventHandler(this.create_btn_Click);
            // 
            // room_name
            // 
            this.room_name.AutoSize = true;
            this.room_name.BackColor = System.Drawing.Color.Transparent;
            this.room_name.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.room_name.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.room_name.Location = new System.Drawing.Point(20, 176);
            this.room_name.Name = "room_name";
            this.room_name.Size = new System.Drawing.Size(123, 23);
            this.room_name.TabIndex = 14;
            this.room_name.Text = "ROOM NAME";
            // 
            // room_name_tb
            // 
            this.room_name_tb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.room_name_tb.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.room_name_tb.Location = new System.Drawing.Point(185, 176);
            this.room_name_tb.Name = "room_name_tb";
            this.room_name_tb.Size = new System.Drawing.Size(149, 23);
            this.room_name_tb.TabIndex = 13;
            this.room_name_tb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.room_name_tb_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Black", 13F);
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(140, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 30);
            this.label2.TabIndex = 17;
            this.label2.Text = "HT Chat";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::ChatApp.Properties.Resources.Untitled_1;
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(20, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(330, 111);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::ChatApp.Properties.Resources._100558368_246432556645783_1444366019534520320_n;
            this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox2.Location = new System.Drawing.Point(164, 170);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(190, 36);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 18;
            this.pictureBox2.TabStop = false;
            // 
            // id_pb
            // 
            this.id_pb.BackColor = System.Drawing.Color.Transparent;
            this.id_pb.Image = global::ChatApp.Properties.Resources._100558368_246432556645783_1444366019534520320_n;
            this.id_pb.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.id_pb.Location = new System.Drawing.Point(122, 354);
            this.id_pb.Name = "id_pb";
            this.id_pb.Size = new System.Drawing.Size(157, 36);
            this.id_pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.id_pb.TabIndex = 18;
            this.id_pb.TabStop = false;
            this.id_pb.Visible = false;
            // 
            // id_lb
            // 
            this.id_lb.AutoSize = true;
            this.id_lb.BackColor = System.Drawing.Color.Transparent;
            this.id_lb.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.id_lb.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.id_lb.Location = new System.Drawing.Point(20, 359);
            this.id_lb.Name = "id_lb";
            this.id_lb.Size = new System.Drawing.Size(89, 23);
            this.id_lb.TabIndex = 14;
            this.id_lb.Text = "ROOM ID";
            this.id_lb.Visible = false;
            // 
            // copy_btn
            // 
            this.copy_btn.AutoSize = true;
            this.copy_btn.BackgroundImage = global::ChatApp.Properties.Resources._100790964_686014118855549_1343039108137615360_n;
            this.copy_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.copy_btn.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copy_btn.Location = new System.Drawing.Point(287, 350);
            this.copy_btn.Name = "copy_btn";
            this.copy_btn.Size = new System.Drawing.Size(67, 41);
            this.copy_btn.TabIndex = 7;
            this.copy_btn.Text = "COPY";
            this.copy_btn.UseVisualStyleBackColor = true;
            this.copy_btn.Visible = false;
            this.copy_btn.Click += new System.EventHandler(this.copy_btn_Click);
            // 
            // room_id_lb
            // 
            this.room_id_lb.AutoSize = true;
            this.room_id_lb.BackColor = System.Drawing.Color.White;
            this.room_id_lb.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.room_id_lb.Location = new System.Drawing.Point(165, 359);
            this.room_id_lb.Name = "room_id_lb";
            this.room_id_lb.Size = new System.Drawing.Size(0, 23);
            this.room_id_lb.TabIndex = 19;
            // 
            // createRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImage = global::ChatApp.Properties.Resources._400;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(382, 453);
            this.Controls.Add(this.room_id_lb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.id_lb);
            this.Controls.Add(this.room_name);
            this.Controls.Add(this.room_name_tb);
            this.Controls.Add(this.copy_btn);
            this.Controls.Add(this.create_btn);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.id_pb);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "createRoom";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "createRoom";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.id_pb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button create_btn;
        private System.Windows.Forms.Label room_name;
        private System.Windows.Forms.TextBox room_name_tb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox id_pb;
        private System.Windows.Forms.Label id_lb;
        private System.Windows.Forms.Button copy_btn;
        private System.Windows.Forms.Label room_id_lb;
    }
}