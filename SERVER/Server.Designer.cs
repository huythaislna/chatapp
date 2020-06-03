namespace SERVER
{
    partial class Server
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
            this.start_btn = new System.Windows.Forms.Button();
            this.data = new System.Windows.Forms.Label();
            this.power_lb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // start_btn
            // 
            this.start_btn.Location = new System.Drawing.Point(271, 244);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(75, 23);
            this.start_btn.TabIndex = 0;
            this.start_btn.Text = "START";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.start_btn_Click);
            // 
            // data
            // 
            this.data.AutoSize = true;
            this.data.Location = new System.Drawing.Point(257, 109);
            this.data.Name = "data";
            this.data.Size = new System.Drawing.Size(46, 17);
            this.data.TabIndex = 1;
            this.data.Text = "label1";
            // 
            // power_lb
            // 
            this.power_lb.AutoSize = true;
            this.power_lb.Location = new System.Drawing.Point(473, 244);
            this.power_lb.Name = "power_lb";
            this.power_lb.Size = new System.Drawing.Size(46, 17);
            this.power_lb.TabIndex = 2;
            this.power_lb.Text = "label1";
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.power_lb);
            this.Controls.Add(this.data);
            this.Controls.Add(this.start_btn);
            this.Name = "Server";
            this.Text = "SERVER";
            this.Load += new System.EventHandler(this.Server_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start_btn;
        private System.Windows.Forms.Label data;
        private System.Windows.Forms.Label power_lb;
    }
}

