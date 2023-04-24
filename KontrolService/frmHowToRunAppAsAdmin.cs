using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KontrolService
{
    public partial class frmHowToRunAppAsAdmin : Form
    {
        public frmHowToRunAppAsAdmin()
        {
            InitializeComponent();
        }
        private string ExePath()
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            return strWorkPath;
            
        }
        private Boolean IsRunByVisualStudio()
        {
            return System.Diagnostics.Debugger.IsAttached;
        }
        private void frmHowToRunAppAsAdmin_Load(object sender, EventArgs e)
        {
            this.picBoxNeedtoRunVSAsAdmin.Visible = false;
            this.lblNeedtoRunVSAsAdmin.Visible = false;
            if (IsRunByVisualStudio ())
            {
                this.picBoxNeedtoRunVSAsAdmin.Visible = true;
                this.lblNeedtoRunVSAsAdmin.Visible = true;
            }
            this.linkLabel1.Tag = ExePath();
            this.linkLabel1.Text = "Please click here to go to " + this.linkLabel1.Tag.ToString();
            this.linkLabel1.Click += LinkLabel1_Click;

        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            String ExPath = ((LinkLabel)sender).Tag.ToString();
            
            Process.Start(ExPath );


        }

        private void StartProcess(string path)
        {
            ProcessStartInfo StartInformation = new ProcessStartInfo();

            StartInformation.FileName = path;

            Process process = Process.Start(StartInformation);

            process.EnableRaisingEvents = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
