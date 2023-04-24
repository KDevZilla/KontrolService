using KontrolService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolService
{
    public partial class FormControlService : Form
    {
        public FormControlService()
        {
            InitializeComponent();
        }
        Dictionary<string, cKontrolService> DicService = new Dictionary<string, cKontrolService>();

        const String ServiceStartUpNoInformation = "No information";
        const String ServiceStartUpAutomatic = "Automatic";
        const String ServiceStartUpDisabled = "Disabled";
        const String ServiceStartUpManual = "Manual";

        const int NameColOrder = 1;
        int DisplayNameColOrder = 2;
        int ServiceStatusColOrder = 3;
        int ServiceStatupColOrder = 4;

        delegate void UpdateListViewStatusCallBack(String strServiceName, String Status);



        private void button10_Click(object sender, EventArgs e)
        {
            frmChooseServices f = new frmChooseServices();
            f.ShowDialog();
            if (!f.HasChooseServiceName)
            {
                return;
            }
            Pro.LoadService(f.hshServiceSelected);


            LoadService(Pro);

            DisplayServiceV2();
        }

        private void DisplayServiceV2()
        {
            int i = 0;
            //this.checkedListBox1.Items.Clear();
            ServiceController[] services = ServiceController.GetServices();

            foreach (string strServiceName in DicService.Keys)
            {

                UpdateListViewStatus(strServiceName, DicService[strServiceName].service.Properties.Status.ToString());

            }

        }
        private void UpdateListViewStatus(String strServiceName, String Status)
        {
            if (this.listView1.InvokeRequired)
            {
                UpdateListViewStatusCallBack l = new UpdateListViewStatusCallBack(UpdateListViewStatus);

                this.Invoke(l, new object[] { strServiceName, Status });
            }
            else
            {
                ListViewItem lv = this.listView1.Items[strServiceName];
                lv.SubItems[ServiceStatusColOrder].Text = Status;
                switch (Status)
                {
                    case "Starting":
                    case "Stopping":
                        lv.UseItemStyleForSubItems = false;
                        lv.SubItems[ServiceStatusColOrder].BackColor = Color.White;
                        if (IsShowServiceStartUpType)
                        {
                            lv.SubItems[ServiceStatupColOrder].BackColor = Color.White;
                        }
                        //lv.SubItems[3].BackColor = Color.White;
                        break;
                    case "Stopped":
                        //lv.BackColor = Color.Yellow;
                        lv.UseItemStyleForSubItems = false;
                        lv.SubItems[ServiceStatusColOrder].BackColor = Color.Yellow;
                        if (IsShowServiceStartUpType)
                        {
                            lv.SubItems[ServiceStatupColOrder].BackColor = Color.White;
                        }
                        //lv.SubItems[3].BackColor = Color.Yellow ;
                        break;
                    case "Running":
                        //lv.BackColor = Color.LightGreen;
                        lv.UseItemStyleForSubItems = false;
                        lv.SubItems[ServiceStatusColOrder].BackColor = Color.LightGreen;
                        if (IsShowServiceStartUpType)
                        {
                            lv.SubItems[ServiceStatupColOrder].BackColor = Color.LightGreen;
                        }
                        //lv.SubItems[3].BackColor = Color.LightGreen;
                        break;

                }
                if (IsShowServiceStartUpType)
                {
                    if (lv.SubItems[ServiceStatupColOrder].Text == ServiceStartUpDisabled)
                    {
                        //lv.BackColor = Color.Crimson;
                        lv.UseItemStyleForSubItems = false;
                        lv.SubItems[NameColOrder].BackColor = Color.Crimson;
                        lv.SubItems[DisplayNameColOrder].BackColor = Color.Crimson;
                        lv.SubItems[ServiceStatusColOrder].BackColor = Color.Crimson;
                        lv.SubItems[ServiceStatupColOrder].BackColor = Color.Crimson;
                    }
                }


                //this.listView1.Items[strServiceName].SubItems[2].Text = Status;

            }
        }
        Boolean IsShowServiceStartUpType = false;
        private Project Pro = new Project("Untitled");

        private void AddNewListItem(String serviceName,
           String serviceDisplayName,
           String Status,
           String StartupType)
        {
            ListViewItem lstViewItem = new ListViewItem();
            lstViewItem.Name = serviceName;
            lstViewItem.SubItems.Add(serviceName);
            lstViewItem.SubItems.Add(serviceDisplayName);
            lstViewItem.SubItems.Add(Status);

            if (IsShowServiceStartUpType)
            {
                lstViewItem.SubItems.Add(StartupType);
            }
            this.listView1.Items.Add(lstViewItem);

        }
        private void LoadService(Project pPro)
        {
            ServiceController[] services = ServiceController.GetServices();



            try
            {
                bool IsUsingMock = false;
                if (!IsUsingMock)
                {
                    foreach (ServiceController service in ServiceController.GetServices())
                    {
                        string serviceName = service.ServiceName;
                        string serviceDisplayName = service.DisplayName;
                        string serviceType = service.ServiceType.ToString();
                        string status = service.Status.ToString();


                        if (!pPro.HshServiceName.Contains(serviceName))
                        {
                            continue;
                        }


                        cKontrolService c = new cKontrolService(service);

                        DicService.Add(serviceName, c);
                        string startupmode = "";
                        if (IsShowServiceStartUpType)
                        {
                            enStartMode e = ServiceHelper.GetServiceStartMode(serviceName); //GetServiceStartMode(serviceName);
                            startupmode = ServiceStartUpNoInformation;
                            switch (e)
                            {
                                case enStartMode.Automatic:
                                    startupmode = ServiceStartUpAutomatic;
                                    break;
                                case enStartMode.Disabled:
                                    startupmode = ServiceStartUpDisabled;
                                    break;
                                case enStartMode.Manual:
                                    startupmode = ServiceStartUpManual;
                                    break;
                            }
                        }

                        AddNewListItem(serviceName, serviceDisplayName, status, startupmode);



                    }
                }
                else
                {
                    string[,] arrMock ={
                                      {"Test001", "Running", "Automatic"},
                                      {"Test002", "Stopped", "Automatic"},
                                      {"Test003", "Stopped", "Automatic"},
                                      {"Test004", "Stopped", "Automatic"},
                                      {"Test005", "Stopped", "Automatic"},
                                      };

                    /*
                    AddNewListItem("Test001", "Running", "Automatic");
                    AddNewListItem("Test001", "Running", "Automatic");
                    AddNewListItem("Test003", "Stopped", "Automatic");
                    AddNewListItem("Test004", "Stopped", "Automatic");
                    AddNewListItem("Test005", "Stopped", "Automatic");
                    */
                    int i;
                    int j;
                    for (i = 0; i < arrMock.GetLength(0); i++)
                    {
                        cKontrolService c = new cKontrolService(IServiceAdapterFactory.Create(arrMock[i, 0], true));
                        c.service.Properties.Status = ServiceControllerStatus.Stopped;
                        String startupmode = "Manual";
                        DicService.Add(c.service.Properties.ServiceName, c);
                        AddNewListItem(arrMock[i, 0], "", arrMock[i, 1], startupmode);

                    }


                    cKontrolService c2 = new cKontrolService(IServiceAdapterFactory.Create("sppsvc", false));
                    //c2.service.Properties.Status = ServiceControllerStatus.Stopped;
                    enStartMode e = ServiceHelper.GetServiceStartMode("sppsvc"); //GetServiceStartMode(serviceName);
                    string startupmode2 = "No information";
                    switch (e)
                    {
                        case enStartMode.Automatic:
                            startupmode2 = "Automatic";
                            break;
                        case enStartMode.Disabled:
                            startupmode2 = "Disabled";
                            break;
                        case enStartMode.Manual:
                            startupmode2 = "Manual";
                            break;
                    }
                    //AddNewListItem(serviceName, status, startupmode);
                    string status = c2.service.Properties.Status.ToString();
                    cKontrolService c3 = new cKontrolService(c2.service);
                    DicService.Add(c2.service.Properties.ServiceName, c3);

                    AddNewListItem("sppsvc", "", status, startupmode2);

                    Random R = new Random();
                    for (i = 1; i <= 100; i++)
                    {
                        int NumberofSecond = R.Next(1, 10);
                        MockServiceAdapter M = MockServiceAdapter.NotGangof4Builder("TestAllocate " + i.ToString())
                                        .SetMegabytesAllocateForService(1)
                                        .SetMilisecondToStart(1000 * NumberofSecond)
                                        .SetMilisecondToStop(10000);

                        M.Properties.Status = ServiceControllerStatus.Stopped;

                        cKontrolService c4 = new cKontrolService(M);
                        status = c4.service.Properties.Status.ToString();
                        DicService.Add(M.Properties.ServiceName, c4);
                        AddNewListItem(c4.service.Properties.ServiceName, "", status, "Manual");
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        /*
        private void LoadServiceV2()
        {
            ServiceController[] services = ServiceController.GetServices();




            try
            {
                foreach (ServiceController service in ServiceController.GetServices())
                {
                    string serviceName = service.ServiceName;
                    string serviceDisplayName = service.DisplayName;
                    string serviceType = service.ServiceType.ToString();
                    string status = service.Status.ToString();




                    cKontrolService c = new cKontrolService(service);
                    DicService.Add(serviceName, c);


                    string startupmode = "No information";

                    AddNewListItem(serviceName, "", status, startupmode);



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }
        */

        public static bool IsAdministrator()
        {
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);


            }
            catch (Exception ex)
            {
                return false;
            }

        }
        ILog LogObject = null;
        private void FormControlService_Load(object sender, EventArgs e)
        {
            LogObject = new TxtBoxLog(this.textBox1);


            if (!IsAdministrator())
            {

                frmHowToRunAppAsAdmin f = new frmHowToRunAppAsAdmin();
                f.ShowDialog();

            }
            //  this.checkedListBox1.CheckOnClick = true;

            this.listView1.CheckBoxes = true;
            this.listView1.Columns.Add("#", 20, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Name", 300, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Name Display ", 500, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Status", 100, HorizontalAlignment.Left);

            if (IsShowServiceStartUpType)
            {
                this.listView1.Columns.Add("Startup Type", 100, HorizontalAlignment.Left);
            }

        }
    }
}
