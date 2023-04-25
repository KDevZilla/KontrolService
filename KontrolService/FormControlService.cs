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
using System.Threading;
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
        private System.Windows.Forms.Timer timer1;
        delegate void UpdateListViewStatusCallBack(String strServiceName, String Status);
        delegate void LogCallback(string text);
     //   delegate void UpdateListViewStatusCallBack(String strServiceName, String Status);
        delegate void EnableButtonCallBack(Button b, Boolean IsEnable);
        delegate void EnableListViewCallBack(Extendlistview extV, Boolean IsEnable);


        private void LoadProject(HashSet<String> ServiceSelected)
        {
            Pro.LoadService(ServiceSelected);


        }
        private void button10_Click(object sender, EventArgs e)
        {
            frmChooseServices f = new frmChooseServices();
            f.ShowDialog();
            if (!f.HasChooseServiceName)
            {
                return;
            }
            LoadProject(f.hshServiceSelected);
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
        private void ShowFileNameOnCaption(String fullFilePath)
        {
            String onlyFileName = System.IO.Path.GetFileName(fullFilePath).Replace(".prj", "");
            this.Text = $"ControlService [{onlyFileName}]";

        }
        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.InitialDirectory = Util.FileUtil.ProjectFilesPath;
            //"Text files (*.txt)|*.
            opf.Filter = "Project Files (*.prj)|";
            var dialogueResult = opf.ShowDialog();
            if (dialogueResult != DialogResult.OK)
            {
                return;
            }

            string fileName = opf.FileName;
          //  SerializeHelper.SerializeProject(Pro, fileName);
            Project Pro = null;
            SerializeHelper.DeserializeProject(ref Pro, fileName);
            this.Pro = Pro;
            /*
            String onlyFileName = System.IO.Path.GetFileName(fileName).Replace(".prj", "");
            this.Text = $"ControlService [{onlyFileName}]";
            */
            this.ProjectFile = fileName;
            this.ShowFileNameOnCaption(this.ProjectFile);
            // Pro.LoadService(f.hshServiceSelected);

            LoadService(Pro);
            DisplayServiceV2();
            ShowFileNameOnCaption(fileName);
        }

        private void loadServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChooseServices f = new frmChooseServices();
            f.ShowDialog();
            if (!f.HasChooseServiceName)
            {
                return;
            }
            //LoadProject(f.hshServiceSelected);

            Pro.LoadService(f.hshServiceSelected);

            LoadService(Pro);
            DisplayServiceV2();
        }
        private string ProjectFile { get; set; } = "";
        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ProjectFile.Trim() == "")
            {
                SaveFileDialog spf = new SaveFileDialog();
               // OpenFileDialog opf = new OpenFileDialog();
                spf.InitialDirectory = Util.FileUtil.ProjectFilesPath;
                //"Text files (*.txt)|*.
                spf.Filter = "Project Files (*.prj)|";
                var dialogueResult = spf.ShowDialog();
                if (dialogueResult != DialogResult.OK)
                {
                    return;
                }

                string fileName = spf.FileName;
                this.ProjectFile = fileName;
            }

            /*
            this.BackupProject.Name = this.txtProjectName.Text;
            Util.FileUtil.SaveFile(this.ProjectFile, this.BackupProject.GetXMLDoc());
            MessageBox.Show($"Project {BackupProject.Name} was saved.");
            */
            SerializeHelper.SerializeProject(this.Pro, this.ProjectFile);
            MessageBox.Show($"Project {this.ProjectFile} was saved.");

            ShowFileNameOnCaption(this.ProjectFile);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

                SaveFileDialog spf = new SaveFileDialog();
                // OpenFileDialog opf = new OpenFileDialog();
                spf.InitialDirectory = Util.FileUtil.ProjectFilesPath;
                //"Text files (*.txt)|*.
                spf.Filter = "Project Files (*.prj)|";
                var dialogueResult = spf.ShowDialog();
                if (dialogueResult != DialogResult.OK)
                {
                    return;
                }
                string fileName = spf.FileName;
                this.ProjectFile = fileName;


            /*
            this.BackupProject.Name = this.txtProjectName.Text;
            Util.FileUtil.SaveFile(this.ProjectFile, this.BackupProject.GetXMLDoc());
            MessageBox.Show($"Project {BackupProject.Name} was saved.");
            */
            SerializeHelper.SerializeProject(this.Pro, this.ProjectFile);
            MessageBox.Show($"Project {this.ProjectFile} was saved.");

            ShowFileNameOnCaption(this.ProjectFile);
        }

        private void EnableButton(Button b, Boolean IsEnable)
        {
            if (b.InvokeRequired)
            {
                EnableButtonCallBack l = new EnableButtonCallBack(EnableButton);
                this.Invoke(l, new object[] { b, IsEnable });
            }
            else
            {
                b.Enabled = IsEnable;
            }


        }
        System.Timers.Timer timerCheckThread = null;
        private void Log(String str)
        {

            if (LogObject != null)
            {
                LogObject.Log(str);
                return;
            }




        }
        private void button7_Click(object sender, EventArgs e)
        {
            EnableButton((Button)sender, false);
            Log("Begin Start Checked Services");

             setCheckedItem();
         //   Macro MStart = getCheckedItem(true);
            StartCheckedServices();
            //   RunMacroUsingTaskV2(MStart);
            StartTimerEnableButton();
            Log("End Start Checked Services");
        }
        private void StartTimerEnableButton()
        {
            if (timerCheckThread != null)
            {
                timerCheckThread.Enabled = false;
                timerCheckThread.Elapsed -= TimerCheckThread_Elapsed;
            }
            timerCheckThread = new System.Timers.Timer();
            timerCheckThread.Elapsed -= TimerCheckThread_Elapsed;
            timerCheckThread.Elapsed += TimerCheckThread_Elapsed;
            timerCheckThread.Interval = 1000;
            timerCheckThread.Enabled = true;
        }
        private void TimerCheckThread_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            Log("Check if All thread finish");
            CheckToEnableButton();
            if (!HasThreadRunning)
            {
                Log("=== All thread finished. ===");
                timerCheckThread.Enabled = false;
                return;
            }
        }

        private void setCheckedItem()
        {
            foreach (string serviceName in DicService.Keys)
            {
                DicService[serviceName].IsChecked = false;
            }
            int i;
            for (i = 0; i < this.listView1.Items.Count; i++)
            {
                if (this.listView1.Items[i].Checked)
                {
                    string ItemName = this.listView1.Items[i].SubItems[1].Text;
                    DicService[ItemName].IsChecked = true;
                }
            }


        }

        private void StartCheckedServices()
        {
            int i;

            //lst.Clear();
            lstThreadRunning = new List<Thread>();
            // DicThreadhasFinished.Clear();
            foreach (string str in DicService.Keys)
            {
                if (!DicService[str].IsChecked)
                {
                    continue;
                }
                IServiceAdapter service = DicService[str].service;


                if (service.Properties.Status == ServiceControllerStatus.Running)
                {
                    this.Log(str + " Status is alrady running. Program will skip starting.");
                    continue;

                }
                if (ServiceHelper.GetServiceStartMode(str) == enStartMode.Disabled)
                {
                    this.Log(str + " start mode is disabled. Program will skip starting.");
                    continue;
                }
                //service.Start();
                HasThreadRunning = true;
                this.Log("[StartCheckedService] set IsThereAtLeastOneThreadToJoin = true;");
                // IsThereAtLeastOneThreadToJoin = true;
                //DicThreadhasFinished.Add(service.Properties.ServiceName, false);


                Thread t = new Thread(new ParameterizedThreadStart(StartService));
                t.Name = service.Properties.ServiceName;
                lstThreadRunning.Add(t);
                this.Log("[StartCheckedService] add thread into lst ");
                t.Start(service);



                //lst.Add(t);

                // t.Join();

                //service.WaitForStatus (ServiceControllerStatus.Running ,               
            }
            this.DisplayServiceV2();
        }
        private Macro getCheckedItem(Boolean IsThisMacroForStartService)
        {
            Macro.MacroActionType MacroAction = Macro.MacroActionType.Start;

            ServiceAction.ServiceActionenum ServiceAction = KontrolService.ServiceAction.ServiceActionenum.Start;

            if (!IsThisMacroForStartService)
            {
                MacroAction = Macro.MacroActionType.Stop;
                ServiceAction = KontrolService.ServiceAction.ServiceActionenum.Stop;
            }
            Macro M = new Macro("StartChecked", MacroAction);

            int i = 0;
            for (i = 0; i < this.listView1.Items.Count; i++)
            {
                if (this.listView1.Items[i].Checked)
                {

                    string ItemName = this.listView1.Items[i].SubItems[1].Text;
                    DicService[ItemName].IsChecked = true;

                    M.AddService(ItemName, ServiceAction);

                }
            }
            return M;


        }



        private void UpdateService(IServiceAdapter sc)
        {

            DicService[sc.Properties.ServiceName].service = sc;//ServiceAdapterFactory.Create(sc);
            UpdateListViewStatus(sc.Properties.ServiceName, sc.Properties.Status.ToString());

        }
        private void UpdateService(IServiceAdapter sc, String custom)
        {

            DicService[sc.Properties.ServiceName].service = sc;//ServiceAdapterFactory.Create(sc);
            UpdateListViewStatus(sc.Properties.ServiceName, custom);

        }
        private void StartService(object obj)
        {
            IServiceAdapter sc = (IServiceAdapter)obj;
            try
            {
                //IServiceAdapter sc = (IServiceAdapter)obj;
                UpdateService(sc, "Starting");
                sc.Start();

                UpdateService(sc);
                this.Log(sc.Properties.ServiceName + " starting");
                sc.WaitForStatus(ServiceControllerStatus.Running);
                if (sc.Properties.Status == ServiceControllerStatus.Running)
                {
                    this.Log(sc.Properties.ServiceName + " has started");
                    //DisplayServiceV2();
                    UpdateService(sc);

                }
            }
            catch (Exception ex)
            {
                this.Log("StartService :: Exception :: " + ex.Message);
            }
            finally
            {
                // DicThreadhasFinished[sc.Properties.ServiceName] = true;
               // CheckToEnableButton();
              // We check ToEnableButton using Timer insted
            }
        }
        List<Thread> lstThreadRunning = new List<Thread>();
        private Boolean HasThreadRunning = false;
        private void CheckToEnableButton()
        {
            /*
            foreach (String key in DicThreadhasFinished.Keys)
            {
                if (DicThreadhasFinished[key] == false)
                {
                    return;
                }
            }
            */

            int i;
            this.Log($"CheckTOEnableButton::No of Thread::{lstThreadRunning.Count }");
            for (i = 0; i < lstThreadRunning.Count; i++)
            {
                if (lstThreadRunning[i].IsAlive)
                {
                    this.Log($"CheckTOEnableButton::Thread {i} is Alive");
                    return;
                }
            }
            this.Log("CheckTOEnableButton::" + " HasFinishedRunning true");

            HasThreadRunning = false;
            // EnableButton(button7, true);
            LockControl(false);
          //  timer1.Enabled = true;


        }

        private void LockControl(Boolean IsLock)
        {
            Button b = btnStartService;

            if (IsLock)
            {
                EnableButton(b, false);
                EnableListVIew(this.listView1, false);
            }
            else
            {
                EnableButton(b, true);
                EnableListVIew(this.listView1, true);
            }



        }

        private void EnableListVIew(Extendlistview extV, Boolean IsEnable)
        {
            if (extV.InvokeRequired)
            {
                EnableListViewCallBack l = new EnableListViewCallBack(EnableListVIew);
                this.Invoke(l, new object[] { extV, IsEnable });
            }
            else
            {
                extV.Enabled = IsEnable;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Log("Begin Stop Checked Services");
            StopCheckedServices();
            StartTimerEnableButton();

            Log("End Stop Checked Services");
        }

        private void StopCheckedServices()
        {
            int i;
            setCheckedItem();
            lstThreadRunning = new List<Thread>();
            foreach (string str in DicService.Keys)
            {
                if (!DicService[str].IsChecked)
                {
                    continue;
                }
                IServiceAdapter service = DicService[str].service;
                if (ServiceHelper.GetServiceStartMode(str) == enStartMode.Disabled)
                {
                    this.Log(str + " start mode is disabled. Programi will skip stopping.");
                    continue;
                }

                if (service.Properties.Status != ServiceControllerStatus.Running)
                {
                    this.Log(str + " start Status is not Running. Programi will skip stopping.");
                    continue;
                }
                if (!service.Properties.CanStop)
                {
                    this.Log(str + " cannot be stopped.");
                    continue;
                }

                // this.Log("[StopCheckedService] set IsThereAtLeastOneThreadToJoin = true;");
                //IsThereAtLeastOneThreadToJoin = true;
                HasThreadRunning = true;
                Thread t = new Thread(new ParameterizedThreadStart(StopService));

                t.Name = service.Properties.ServiceName;
                lstThreadRunning.Add(t);
                this.Log("[StopCheckedService] add thread into lst ");

                t.Start(service);

                //  t.Join();


                //service.Stop();
            }
            CheckToEnableButton();
            //this.DisplayService();
        }

        private void StopService(object obj)
        {
            IServiceAdapter sc = (IServiceAdapter)obj;
            try
            {
                //ServiceController sc = (ServiceController)obj;

                if (sc.Properties.Status != ServiceControllerStatus.Running)
                {
                    this.Log(sc.Properties.ServiceName + " service is not running, no need to stop");
                }
                UpdateService(sc, "Stopping");

                sc.Stop();
                //  IsThereAtLeastOneThreadToJoin = true;
                this.Log(sc.Properties.ServiceName + " stoping");
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                if (sc.Properties.Status == ServiceControllerStatus.Stopped)
                {
                    this.Log(sc.Properties.ServiceName + " has stopped");
                    //DisplayServiceV2();
                    UpdateService(sc);


                }
            }
            catch (Exception ex)
            {
                this.Log("StopService :: Exception :: " + ex.Message);
            }
            finally
            {
                // DicThreadhasFinished[sc.Properties.ServiceName] = true;
                //CheckToEnableButton();
                //We do it using timer instead
            }
        }

        private void btnRestartService_Click(object sender, EventArgs e)
        {
            Log("Begin Stop Checked Services");
            RestartCheckedServices();
            StartTimerEnableButton();

            Log("End Stop Checked Services");
        }

        private void RestartCheckedServices()
        {
            int i;
            setCheckedItem();
            foreach (string str in DicService.Keys)
            {
                if (!DicService[str].IsChecked)
                {
                    continue;
                }
                IServiceAdapter service = DicService[str].service;

                if (ServiceHelper.GetServiceStartMode(str) == enStartMode.Disabled)
                {
                    continue;
                }

                HasThreadRunning = true;
                Thread t = new Thread(new ParameterizedThreadStart(RestartService));
                t.Start(service);
                //service.Stop();
            }
            //this.DisplayService();
        }

        private void RestartService(object obj)
        {
            StopService(obj);
            StartService(obj);
        }
    }
}
