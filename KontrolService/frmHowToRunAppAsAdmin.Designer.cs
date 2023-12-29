namespace KontrolService
{
    partial class frmHowToRunAppAsAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHowToRunAppAsAdmin));
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.picBoxNeedtoRunVSAsAdmin = new System.Windows.Forms.PictureBox();
            this.lblNeedtoRunVSAsAdmin = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxNeedtoRunVSAsAdmin)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(385, 40);
            this.label2.TabIndex = 2;
            this.label2.Text = "You are not running this application as Administrator, \r\nIf you face the problem " +
    "when Start of Stop service ";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(13, 206);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(424, 533);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 132);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(830, 60);
            this.label1.TabIndex = 4;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(13, 84);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(127, 20);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Please click here";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(794, 704);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 43);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picBoxNeedtoRunVSAsAdmin
            // 
            this.picBoxNeedtoRunVSAsAdmin.Image = ((System.Drawing.Image)(resources.GetObject("picBoxNeedtoRunVSAsAdmin.Image")));
            this.picBoxNeedtoRunVSAsAdmin.Location = new System.Drawing.Point(466, 616);
            this.picBoxNeedtoRunVSAsAdmin.Name = "picBoxNeedtoRunVSAsAdmin";
            this.picBoxNeedtoRunVSAsAdmin.Size = new System.Drawing.Size(535, 65);
            this.picBoxNeedtoRunVSAsAdmin.TabIndex = 7;
            this.picBoxNeedtoRunVSAsAdmin.TabStop = false;
            // 
            // lblNeedtoRunVSAsAdmin
            // 
            this.lblNeedtoRunVSAsAdmin.AutoSize = true;
            this.lblNeedtoRunVSAsAdmin.Location = new System.Drawing.Point(462, 593);
            this.lblNeedtoRunVSAsAdmin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNeedtoRunVSAsAdmin.Name = "lblNeedtoRunVSAsAdmin";
            this.lblNeedtoRunVSAsAdmin.Size = new System.Drawing.Size(372, 20);
            this.lblNeedtoRunVSAsAdmin.TabIndex = 8;
            this.lblNeedtoRunVSAsAdmin.Text = "You also need to run Visual Studio as Administrator";
            // 
            // frmHowToRunAppAsAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 759);
            this.Controls.Add(this.lblNeedtoRunVSAsAdmin);
            this.Controls.Add(this.picBoxNeedtoRunVSAsAdmin);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmHowToRunAppAsAdmin";
            this.Text = "How To Run App As Admin";
            this.Load += new System.EventHandler(this.frmHowToRunAppAsAdmin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxNeedtoRunVSAsAdmin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox picBoxNeedtoRunVSAsAdmin;
        private System.Windows.Forms.Label lblNeedtoRunVSAsAdmin;
    }
}