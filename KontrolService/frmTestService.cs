using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace KontrolService
{
    public partial class frmTestService : Form
    {
        public frmTestService()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            ServiceController sc = new ServiceController();
            sc.ServiceName = "SQLSERVERAGENT";

            this.textBox1.Text = sc.Status.ToString();

            sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(long.MaxValue));
            this.textBox1.Text = "Running";
            */
            RunManagementEventWatcherForWindowsServices();
            

        }

        private  void RunManagementEventWatcherForWindowsServices()
        {
            EventQuery eventQuery = new EventQuery();
            eventQuery.QueryString = "SELECT * FROM __InstanceModificationEvent within 2 WHERE targetinstance isa 'Win32_Service'";
            ManagementEventWatcher demoWatcher = new ManagementEventWatcher(eventQuery);
            demoWatcher.Options.Timeout = new TimeSpan(1, 0, 0);
          //  Console.WriteLine("Perform the appropriate change in a Windows service according to your query");
            ManagementBaseObject nextEvent = demoWatcher.WaitForNextEvent();
            ManagementBaseObject targetInstance = ((ManagementBaseObject)nextEvent["targetinstance"]);
            PropertyDataCollection props = targetInstance.Properties;
            StringBuilder strB = new StringBuilder();
            foreach (PropertyData prop in props)
            {
               // Console.WriteLine("Property name: {0}, property value: {1}", prop.Name, prop.Value);
               strB.Append ( prop.Name).Append ( prop.Value).Append (Environment.NewLine );


            }
            this.textBox1.Text = strB.ToString();

            demoWatcher.Stop();
        }
    }
}
