using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.ServiceProcess;
using Microsoft.Win32;
using System.Management;
using System.Threading;
using System.Security.Principal;
using KontrolService.Services;
using System.Threading.Tasks;

namespace KontrolService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Dictionary<string, ServiceController> DicService = new Dictionary<string, ServiceController>();
        
        Dictionary<string, cKontrolService> DicService = new Dictionary<string, cKontrolService>();

        const String ServiceStartUpNoInformation = "No information";
        const String ServiceStartUpAutomatic = "Automatic";
        const String ServiceStartUpDisabled = "Disabled";
        const String ServiceStartUpManual = "Manual";

        const int NameColOrder = 1;
         int DisplayNameColOrder = 2;
         int ServiceStatusColOrder = 3;
         int ServiceStatupColOrder = 4;

        private void DisplayServiceUnused()
        {
            int i=0;
            //this.checkedListBox1.Items.Clear();
            ServiceController[] services= ServiceController.GetServices();
            
            foreach(string strServiceName in DicService.Keys )
            {
                foreach (ServiceController service in ServiceController.GetServices())
                {
                    string serviceName = service.ServiceName;
                    if(serviceName == strServiceName )
                    {
                        DicService[serviceName].service = IServiceAdapterFactory.Create(service); ;
                    }
                }

                
               // DicService[strServiceName] = 
                string Record = strServiceName + " " + DicService[strServiceName].service.Properties.Status;
               // this.checkedListBox1.Items.Add(Record);
               // int LastIndex= this.checkedListBox1.Items.Count -1;
                if (DicService[strServiceName].IsChecked)
                {
                    //this.checkedListBox1.SetItemCheckState(LastIndex, CheckState.Checked);
                    this.listView1.CheckedItems[0].Checked = true;

                }
            }
            
        }



        private void UpdateService(IServiceAdapter  sc)
        {

            DicService[sc.Properties.ServiceName].service = sc;//ServiceAdapterFactory.Create(sc);
            UpdateListViewStatus(sc.Properties.ServiceName , sc.Properties.Status.ToString () );

        }
        private void UpdateService(IServiceAdapter sc,String custom)
        {

            DicService[sc.Properties.ServiceName].service = sc;//ServiceAdapterFactory.Create(sc);
            UpdateListViewStatus(sc.Properties.ServiceName, custom);

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

        private Project Pro = new Project("Untitled");
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
                        if (IsShowServiceStartUpType )
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
                    string[,] arrMock={
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
                    for(i=0;i<arrMock.GetLength (0);i++)
                    {
                        cKontrolService c = new cKontrolService(IServiceAdapterFactory.Create(arrMock[i, 0], true));
                        c.service.Properties.Status = ServiceControllerStatus.Stopped;
                        String startupmode = "Manual";
                        DicService.Add(c.service.Properties.ServiceName , c);
                        AddNewListItem(arrMock[i, 0],"", arrMock[i, 1], startupmode);
                        
                    }
                    

                    cKontrolService c2= new cKontrolService(IServiceAdapterFactory.Create("sppsvc", false));
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
                        DicService.Add(c2.service.Properties.ServiceName ,c3);

                        AddNewListItem("sppsvc","", status , startupmode2);

                        Random R = new Random();
                        for (i = 1; i <= 100; i++)
                        {
                            int NumberofSecond = R.Next(1, 10);
                            MockServiceAdapter M = MockServiceAdapter.NotGangof4Builder("TestAllocate " + i.ToString ())
                                            .SetMegabytesAllocateForService(1)
                                            .SetMilisecondToStart(1000 * NumberofSecond )
                                            .SetMilisecondToStop(10000);
                            
                            M.Properties.Status = ServiceControllerStatus.Stopped;

                            cKontrolService c4 = new cKontrolService(M);
                            status = c4.service.Properties.Status.ToString();
                            DicService.Add(M.Properties.ServiceName, c4);
                            AddNewListItem(c4.service.Properties.ServiceName,"", status, "Manual");
                        }
                    
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            


        }


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

                        AddNewListItem(serviceName,"", status, startupmode);
                    


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            


        }


        public static  bool IsAdministrator()
        {
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                return  principal.IsInRole(WindowsBuiltInRole.Administrator);

               
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        ILog LogObject = null;
        private void Form1_Load(object sender, EventArgs e)
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
            //this.LoadService();
            //this.DisplayServiceV2();




            //this.LoadService();
            //this.LoadServiceV2();
            //this.DisplayServiceV2();
            //this.DisplayService();


            /*
            ListViewItem lstViewItem = new ListViewItem();
            lstViewItem.SubItems.Add("Tomcat8");
            lstViewItem.SubItems.Add("Running");
            
            this.listView1.Items.Add(lstViewItem);
            */
          //  button11_Click(null, null);

        }

       

        private void AddNewListItem(String serviceName,
            String serviceDisplayName,
            String Status,
            String StartupType){
                ListViewItem lstViewItem = new ListViewItem();
                lstViewItem.Name = serviceName;
            lstViewItem.SubItems.Add (serviceName );
            lstViewItem.SubItems.Add(serviceDisplayName);
            lstViewItem.SubItems.Add (Status  );

            if (IsShowServiceStartUpType)
            {
                 lstViewItem.SubItems.Add (StartupType);
            }
            this.listView1.Items.Add(lstViewItem);
                
    }


        private void setMacrotoCheckItem(List<String> lstServices)
        {
            int i;

            for (i = 0; i < this.listView1.Items.Count; i++)
            {
                string ItemName = this.listView1.Items[i].SubItems[1].Text;
                if(ItemName.ToLower().Equals("tomcat"))
                {
                    string strHello = "Hello";
                }
                this.listView1.Items[i].Checked = false ;
               

            }
            for (i = 0; i < this.listView1.Items.Count; i++)
            {
                    string ItemName = this.listView1.Items[i].SubItems[1].Text;
                int j;
                for (j = 0; j < lstServices.Count; j++)
                {
                    if(ItemName.Trim().ToLower ()==lstServices [j].Trim().ToLower())
                    {
                        this.listView1.Items[i].Checked = true;
                        break;
                    }
                }
                  //  DicService[ItemName].IsChecked = true;

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
        private Macro getCheckedItem(Boolean IsThisMacroForStartService)
        {
            Macro.MacroActionType MacroAction = Macro.MacroActionType.Start;

            ServiceAction.ServiceActionenum ServiceAction = KontrolService.ServiceAction.ServiceActionenum.Start;

            if(!IsThisMacroForStartService )
            {
                MacroAction = Macro.MacroActionType.Stop;
                ServiceAction = KontrolService.ServiceAction.ServiceActionenum.Stop;
            }
            Macro M = new Macro("StartChecked", MacroAction );

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
                CheckToEnableButton();
            }
        }
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
            for(i=0;i<lstThreadRunning.Count;i++)
            {
                if(lstThreadRunning[i].IsAlive )
                {
                    return;
                }
            }
            this.Log("CheckTOEnableButton::" + " HasFinishedRunning true");
           
            HasThreadRunning = false;
           // EnableButton(button7, true);
            LockControl(false);
            timer1.Enabled = true;


        }
        private void LockControl(Boolean IsLock)
        {
            Button b = button7;

              if(IsLock)
            {
                EnableButton(b, false);
                EnableListVIew(this.listView1, false);
            } else
            {
                EnableButton(b, true);
                EnableListVIew(this.listView1, true);
            }


           
        }

        private void RestartService(object obj)
        {
            StopService(obj);
            StartService(obj);
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
                CheckToEnableButton();
            }
        }
            
        List<Thread> lstThreadRunning = new List<Thread>();
        //Dictionary<String, Boolean> DicThreadhasFinished = new Dictionary<string, bool>();
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
                t.Name = service.Properties.ServiceName ;
                lstThreadRunning.Add(t);
                this.Log("[StartCheckedService] add thread into lst ");
                t.Start(service);
                

                
                //lst.Add(t);

                // t.Join();

                //service.WaitForStatus (ServiceControllerStatus.Running ,               
            }
            this.DisplayServiceV2();
        }


        private void StartCheckedServicesUsingTask()
        {
            int i;
            setCheckedItem();
            //lst.Clear();
            // lstThreadRunning = new List<Thread>();
            // DicThreadhasFinished.Clear();
            //List<Task<>> tasks = new List<Task<>>();

            List<Task> tasks = new List<Task>();
            
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

                this.Log("  Add task for ::" + service.Properties.ServiceName);
                HasThreadRunning = true;
                tasks.Add(Task.Run(() => StartService(service)));


                //lst.Add(t);

                // t.Join();

                //service.WaitForStatus (ServiceControllerStatus.Running ,               
            }
            //tasks[0].
            Task.WhenAll(tasks).ContinueWith(task => TaskFinished("Start"));

            this.Log("  Before Display Service V2 ");
            this.DisplayServiceV2();
            this.Log("  After Display Service V2");
        }

        private void RunMacroUsingTaskV2(Macro M)
        {
            int i;
            //setCheckedItem();
            //lst.Clear();
            // lstThreadRunning = new List<Thread>();
            // DicThreadhasFinished.Clear();
            //List<Task<>> tasks = new List<Task<>>();

            List<Task> tasks = new List<Task>();

            for(i=0;i<M.lstServiceAction.Count;i++)
            //foreach (string serviceName in M.DicServiceAction.Keys)
            {

                String serviceName = M.lstServiceAction[i].ServiceName;
                ServiceAction.ServiceActionenum Action = M.lstServiceAction[i].Action;
                IServiceAdapter service = DicService[serviceName].service;

                
                
                
                if (Action == ServiceAction.ServiceActionenum.Start)
                {
                    if (service.Properties.Status == ServiceControllerStatus.Running)
                    {
                        this.Log(serviceName + " Status is alrady running. Program will skip starting.");
                        continue;

                    }

                    if (ServiceHelper.GetServiceStartMode(serviceName) == enStartMode.Disabled)
                    {
                        this.Log(serviceName + " start mode is disabled. Program will skip starting.");
                        continue;
                    }
                    tasks.Add(Task.Run(() => StartService(service)));
                }
                else
                {
                    if (service.Properties.Status != ServiceControllerStatus.Running)
                    {
                        this.Log(serviceName + " Status is not Running. Program will skip stopping.");
                        continue;
                    }

                    if (ServiceHelper.GetServiceStartMode(serviceName) == enStartMode.Disabled)
                    {
                        this.Log(serviceName + " start mode is disabled. Program will skip stopping.");
                        continue;
                    }
                    tasks.Add(Task.Run(() => StopService(service)));
                }

                this.Log("  Add task for ::" + service.Properties.ServiceName);
                HasThreadRunning = true;
                    
            }

            Task.WhenAll(tasks).ContinueWith(task => TaskFinished(M.Name));
            this.Log("  Before Display Service V2 ");
            this.DisplayServiceV2();
            this.Log("  After Display Service V2");
        }
        private void StopCheckedServicesUsingTasks()
        {
            int i;
            setCheckedItem();
            // lstThreadRunning = new List<Thread>();

            List<Task> tasks = new List<Task>();
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
                HasThreadRunning = true;
                this.Log("  Add task for ::" + service.Properties.ServiceName);
                tasks.Add(Task.Run(() => StopService(service)));


                //service.Stop();

            }
            Task.WhenAll(tasks).ContinueWith(task => TaskFinished("Stop"));
            CheckToEnableButton();
            //this.DisplayService();
        }
        private void EnableUI(List<Task> tasks)
        {
            //var finalValue = tasks.Sum(task => task.Result);
            //MessageBox.Show("Sum is : " + finalValue);

            this.Log("Task has finished so we enable UI");

        }

        private void TaskFinished(String S)
        {
            //var finalValue = tasks.Sum(task => task.Result);
            //MessageBox.Show("Sum is : " + finalValue);

            this.Log(S + " has finished so we enable UI");
            if(QuMacro.Count == 0)
            {
                this.Log(" No task left in queue");
                return;
                
            }
            this.Log(" There are Task left in queue");
            HasThreadRunning = false;
            StartTimer();
            //RunMacroUsingTask(QuMacro.Dequeue());


        }
        /*
        private void StartCheckedServicesWaitForThread()
        {
            int i;
            setCheckedItemV2();
            //lst.Clear();
            lstThreadRunning = new List<Thread>();

            DicThreadhasFinished.Clear();
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
                Thread t = new Thread(new ParameterizedThreadStart(StartService));
                t.Start(service);
                //lst.Add(t);
                lstThreadRunning.Add(t);
                DicThreadhasFinished.Add(service.Properties.ServiceName, false);
                //service.WaitForStatus (ServiceControllerStatus.Running ,               
            }
            this.DisplayServiceV2();
        }
        */


        private void StartServicesFromMacro(List<String> lstServices)
        {
            int i;
            //setCheckedItemV2();
            //lst.Clear();
            //DicThreadhasFinished.Clear();
            lstThreadRunning = new List<Thread>();
            for (i=0;i<lstServices.Count;i++)
            {
                if(DicService.ContainsKey (lstServices[i]))
                {
                    DicService[lstServices[i]].IsChecked = true;
                }

            }
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
                Thread t = new Thread(new ParameterizedThreadStart(StartService));
                t.Start(service);
                //lst.Add(t);
                lstThreadRunning.Add(t);
               // DicThreadhasFinished.Add(service.Properties.ServiceName, false);
                //service.WaitForStatus (ServiceControllerStatus.Running ,               
            }
            this.DisplayServiceV2();
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
                IServiceAdapter  service = DicService[str].service;

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
        Boolean IsShowServiceStartUpType = false;
        private void UpdateListViewStatus(String strServiceName, String Status)
        {
            if (this.listView1.InvokeRequired)
            {
                UpdateListViewStatusCallBack l = new UpdateListViewStatusCallBack(UpdateListViewStatus);

                this.Invoke(l, new object[] { strServiceName , Status  });
            }
            else
            {
                ListViewItem lv = this.listView1.Items[strServiceName];
                lv.SubItems[ServiceStatusColOrder ].Text = Status;
                switch (Status)
                {
                    case "Starting":
                    case "Stopping":
                        lv.UseItemStyleForSubItems = false;
                        lv.SubItems[ServiceStatusColOrder].BackColor = Color.White;
                        if(IsShowServiceStartUpType )
                        {
                            lv.SubItems[ServiceStatupColOrder].BackColor = Color.White;
                        }
                        //lv.SubItems[3].BackColor = Color.White;
                        break;
                    case "Stopped":
                        //lv.BackColor = Color.Yellow;
                        lv.UseItemStyleForSubItems = false;
                        lv.SubItems[ServiceStatusColOrder].BackColor = Color.Yellow;
                        if(IsShowServiceStartUpType )
                        {
                            lv.SubItems[ServiceStatupColOrder].BackColor = Color.White;
                        }
                        //lv.SubItems[3].BackColor = Color.Yellow ;
                        break;
                    case "Running":
                        //lv.BackColor = Color.LightGreen;
                        lv.UseItemStyleForSubItems = false;
                        lv.SubItems[ServiceStatusColOrder].BackColor = Color.LightGreen;
                        if(IsShowServiceStartUpType )
                        {
                            lv.SubItems[ServiceStatupColOrder].BackColor = Color.LightGreen;
                        }
                        //lv.SubItems[3].BackColor = Color.LightGreen;
                        break;

                }
                if (IsShowServiceStartUpType) {
                    if (lv.SubItems[ServiceStatupColOrder].Text == ServiceStartUpDisabled)
                    {
                        //lv.BackColor = Color.Crimson;
                        lv.UseItemStyleForSubItems = false;
                        lv.SubItems[NameColOrder ].BackColor = Color.Crimson;
                        lv.SubItems[DisplayNameColOrder].BackColor = Color.Crimson;
                        lv.SubItems[ServiceStatusColOrder ].BackColor = Color.Crimson;
                        lv.SubItems[ServiceStatupColOrder].BackColor = Color.Crimson;
                    }
                }
                

                //this.listView1.Items[strServiceName].SubItems[2].Text = Status;
               
            }
        }

        private void Log(String str)
        {

            if(LogObject != null)
            {
                LogObject.Log(str);
                return;
            }

            

            
        }
        delegate void LogCallback(string text);
        delegate void UpdateListViewStatusCallBack(String strServiceName, String Status);
        delegate void EnableButtonCallBack(Button b, Boolean IsEnable);
        delegate void EnableListViewCallBack(Extendlistview extV, Boolean IsEnable);

      

        private void button1_Click(object sender, EventArgs e)
        {

          

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < this.listView1.Items.Count; i++)
            {
                //this.checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                this.listView1.Items[i].Checked = true;
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            int i;
            for (i = 0; i < this.listView1.Items.Count; i++)
            {
                //this.checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                this.listView1.Items[i].Checked = false;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Log("Begin Stop Checked Services");
            StopCheckedServices();            
            Log("End Stop Checked Services");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //this.DisplayService();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            EnableButton((Button)sender, false);
            Log("Begin Start Checked Services");

            // setCheckedItem();
            Macro MStart = getCheckedItem(true);
            //StartCheckedServices();
            RunMacroUsingTaskV2(MStart);

            Log("End Start Checked Services");

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (this.listView1.SelectedIndices.Count == 0)
            {
                return;
            }
            int index = this.listView1.SelectedIndices[0];
            //this.listView1.SelectedIndices.Clear();
            this.listView1.Items[index].Selected = false;
            this.listView1.Items[index].Focused = false;
            this.listView1.Items[index].Checked = !this.listView1.Items[index].Checked;

            
            //this.listView1.SelectedIndices.Clear();


            //this.listView1.Items[this.listView1.SelectedIndices ].Checked =
              //  !this.listView1.Items[this.listView1.SelectedIndexChanged].Checked;
            

        }

        private void button8_Click(object sender, EventArgs e)
        {
            RestartCheckedServices();

        }

      
        private void button9_Click(object sender, EventArgs e)
        {
            //MockServiceAdapter M = MockServiceAdapter.Builder("Test001").SetBehaviour("Test Behaviour");
            MockServiceAdapter M = MockServiceAdapter.NotGangof4Builder("TestAllocate")
                                    .SetMegabytesAllocateForService(3)
                                    .SetMilisecondToStart(5000);
            
            //M.beh = "Hello";

            //ServiceController SCC = new ServiceController("TEST_SERVICE");
            
            //SCC.Start();
            /*
            IServiceAdapter Service=  IServiceAdapterFactory.Create ("TestService",true);
            Log("Service Status::" + Service.Properties.Status);

            Service.Start();
            Log("Service Status::" + Service.Properties.Status);
            if (Service.Properties.Status == ServiceControllerStatus.Running)
            {
                Log("Finished testing");
            }
            */

            IServiceAdapter service = IServiceAdapterFactory.Create("MSSQLFDLauncher", false);
            /*
            IControlDisplayvalue I=new  txtDisplayValue (this.textBox1 );
            service.Properties.LoadPhysicial = ServiceHelper.LoadPhysicalPath;
            service.LoadPhysicial(service.Properties.ServiceName ,I );
            */

            this.Text  = ServiceHelper.GetAccountName(service.Properties.ServiceName);

            //IControlShowvalue I = new txtShowValue(this.textBox1);
            //service.LoadPhysicial (new I

            //IServiceAdapter Serv = ServiceAdapterFactory.Create ("TestService2",true,new ServiceAdapterProperties (
            //Service.DisplayName = "Test Display";
            //Service.
            /*

            IControlShowvalue I = new txtShowValue(this.textBox1);
            //Not Using Factory
            cLoadValueThenShow LoadThenShow = new cLoadValueThenShow(I);

            //Using factory
            LoadThenShow = new cLoadValueThenShow(ShowValueFactory.CreateItem(this.textBox1));


            Thread t = new Thread(new ParameterizedThreadStart(SetValue));
            t.Start(LoadThenShow);

            //Using factory inside contructure
            LoadThenShow = new cLoadValueThenShow(this.listView1.Items[2]);
            */

            //MockServiceAdapter Mc = new MockServiceAdapter();
            IServiceAdapter Mc = new MockServiceAdapter();
            //Mc.
            
            //LoadThenShow.LoadValue(I);

            //LoadThenShow.LoadValue(I);
            /*
            ListViewItem Lv= this.listView1.Items[2];

            //Lv.SubItems[2].Text = "(Retriving Status)...";

            I = new ListViewItemShowValue(Lv);

            LoadThenShow.SetValue(I, "(Retriving Status)...");

            LoadThenShow.SetValue(I, "Running");
            */

        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            frmChooseServices f = new frmChooseServices();
            f.ShowDialog();
            if(!f.HasChooseServiceName )
            {
                return;
            }
            Pro.LoadService(f.hshServiceSelected);

         
            LoadService(Pro);

            DisplayServiceV2();

        }

        private void btnSaveProject_Click(object sender, EventArgs e)
        {
            /*
            DialogResult saveDiaResult = saveFileDialog1.ShowDialog();
            if(saveDiaResult != DialogResult.OK )
            {
                return;
            }
            string fileName = saveFileDialog1.FileName;
            */
            string fileName = @"D:\Temp\2021_11_02\Test123.xml";
            SerializeHelper.SerializeProject(Pro, fileName);
           // XMLSerializer.Serialize(Pro, fileName);

            /*
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Pro.GetType());
            System.IO.TextWriter txtWriter = new System.IO.StreamWriter(fileName);
            x.Serialize(txtWriter, Pro);
            */


        }

        private void button11_Click(object sender, EventArgs e)
        {
          SerializeHelper.DeserializeProject(ref Pro, @"D:\Temp\2021\2021_11_02\Test123.xml");
            LoadService(Pro);

            DisplayServiceV2();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            EnableButton((Button)sender, false);
            Log("Begin Start Checked Services");
            List<String> lstMacro = new List<string>();

            lstMacro.Add("CWSVCGWCGenerateNPPI");
            lstMacro.Add("CWSVCGWCMonitoring");
            lstMacro.Add("CWSVCGWCMonitoringInward");

            setMacrotoCheckItem(lstMacro);
            StartCheckedServices();

           // StartServicesFromMacro(lstMacro);

            Log("End Start Checked Services");
        }
        //Boolean IsThereAtLeastOneThreadToJoin = false;
        /*
        private void RunMacro(Macro M)
        {

            Log("[Macro " + M.Name + "] begin.");
         

                setMacrotoCheckItem(M.lstService);

        
            switch (M.ActionType)
                {
                    case Macro.MacroActionType.Start:
                        StartCheckedServices();
                        break;
                    case Macro.MacroActionType.Stop:
                        StopCheckedServices();
                        break;
                }
          
                Log("[Macro " + M.Name + "] End.");
            
        }
        */
        /*
        private void RunMacroUsingTaskBK(Macro M)
        {

            Log("[Macro " + M.Name + "] begin.");


            setMacrotoCheckItem(M.lstService);


            switch (M.ActionType)
            {
                case Macro.MacroActionType.Start:
                    StartCheckedServicesUsingTask();
                    break;
                case Macro.MacroActionType.Stop:
                    StopCheckedServicesUsingTasks();
                    break;
            }

            Log("[Macro " + M.Name + "] End.");

        }
        */

        private void RunMacroUsingTask(Macro M)
        {

            Log("[Macro " + M.Name + "] begin.");


           // setMacrotoCheckItem(M.lstService);


            switch (M.ActionType)
            {
                case Macro.MacroActionType.Start:
                    StartCheckedServicesUsingTask();
                    break;
                case Macro.MacroActionType.Stop:
                    StopCheckedServicesUsingTasks();
                    break;
            }

            Log("[Macro " + M.Name + "] End.");

        }
        private Boolean HasThreadRunning = false;
        private void button1_Click_1(object sender, EventArgs e)
        {

            Macro M = new Macro("Switch to Client 1", Macro.MacroActionType.Parent);

            Macro MS1 = new Macro("  Stop Others services", Macro.MacroActionType.Stop);
            MS1.AddService("MSSQL$SQLEXPRESS2017", ServiceAction.ServiceActionenum.Stop );
            MS1.AddService("Tomcat", ServiceAction.ServiceActionenum.Stop);

            Macro MS2 = new Macro("  Start Client1 services", Macro.MacroActionType.Start);
            MS2.AddService("MSSQL$MSSQLSERVER2019", ServiceAction.ServiceActionenum.Start );
            MS2.AddService("Tomcat9", ServiceAction.ServiceActionenum.Start);
            MS2.AddService("CWSVCGWCGenerateNPPI", ServiceAction.ServiceActionenum.Start);
            MS2.AddService("CWSVCGWCMonitoring", ServiceAction.ServiceActionenum.Start);
           MS2.AddService("CWSVCGWCMonitoringInward", ServiceAction.ServiceActionenum.Start);

            M.lstSubMacro.Add(MS1);
            M.lstSubMacro.Add(MS2);
            RunMacroUsingTaskV2(M);

           // RunMacro(M);


            

            // StartServicesFromMacro(lstMacro);

            //Log("End Start Checked Services");
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
          
        }

     
        private System.Collections.Generic.Queue<Macro> QuMacro = new Queue<Macro>();
        private void button6_Click_1(object sender, EventArgs e)
        {
            Macro M = new Macro("Switch to Client 1", Macro.MacroActionType.Parent);

            Macro MS1 = new Macro("  Stop Others services", Macro.MacroActionType.Stop);
            MS1.AddService("MSSQL$SQLEXPRESS2017", ServiceAction.ServiceActionenum.Stop );
            MS1.AddService("Tomcat", ServiceAction.ServiceActionenum.Stop);
            MS1.AddService("CWBatchAuditMatch", ServiceAction.ServiceActionenum.Stop);
            MS1.AddService("CWBatchHousekeep", ServiceAction.ServiceActionenum.Stop);
            MS1.AddService("CWBatchProcessInwardFile", ServiceAction.ServiceActionenum.Stop);
            MS1.AddService("CWBatchProcessInwRtnFile", ServiceAction.ServiceActionenum.Stop);


            Macro MS2 = new Macro("  Start Client1 services", Macro.MacroActionType.Start);
            MS2.AddStartService("MSSQL$MSSQLSERVER2019" );
            MS2.AddStartService("Tomcat9");
            MS2.AddStartService("CWSVCGWCGenerateNPPI");
            MS2.AddStartService ("CWSVCGWCMonitoring");
            MS2.AddStartService("CWSVCGWCMonitoringInward");
            MS2.AddStartService("CWSVCGWTransferNPPI");
            MS2.AddStartService("CWSVCGWTransferOutward");
            MS2.AddStartService("CWSVCGWCBahGenerateCHS");


            M.lstSubMacro.Add(MS1);
            M.lstSubMacro.Add(MS2);

            QuMacro.Enqueue(MS1);
            QuMacro.Enqueue(MS2);
            StartTimer();



           // RunMacro(M);
        }
        private void StartTimer()
        {
            timer1.Tick += Timer1_Tick;
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }
        private void StopTimer()
        {
            timer1.Enabled = false;
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            StopTimer();
            this.Log("Timer1_Tick::Begin");
            try
            {
                this.Log("Timer1_Tick::QuMacro.Count::" + QuMacro.Count.ToString());

                if (QuMacro.Count == 0)
                {
                    this.Log("Timer1_Tick::End");
                    timer1.Enabled = false;
                    return;
                }

                if (HasThreadRunning)
                {
                    this.Log("Timer1_Tick::   Has HasThreadRunning running");
                    return;
                }

                this.Log("Timer1_Tick::   There is no Thread rnning");
                this.Log("Timer1_Tick::Before calling RunMacro");
                //RunMacro(QuMacro.Dequeue());
                RunMacroUsingTask(QuMacro.Dequeue());
                this.Log("Timer1_Tick::After calling RunMacro");
            } finally
            {
                this.Log("Time1_Tick::End");

            }
            
           // timer1.Enabled = false;

        }

        private void button13_Click(object sender, EventArgs e)
        {
            int i;
            this.Log("   Begin to abort thread");
            this.timer1.Enabled = false;
            QuMacro = new Queue<Macro>();
            for(i=0;i<lstThreadRunning.Count;i++)
            {
                lstThreadRunning[i].Abort();
                this.Log("   Thread ::" + lstThreadRunning[i].Name + " is going to die.");
            }

            for (i = 0; i < lstThreadRunning.Count; i++)
            {
                while(lstThreadRunning [i].ThreadState.HasFlag (ThreadState.Aborted))
                {
                    Thread.Sleep(0);
                }
                this.Log("   Thread ::" + lstThreadRunning[i].Name + " is dead");
            }

            this.Log("   Finsh to abort thread");
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            Macro M = new Macro("Switch to Client 1", Macro.MacroActionType.Parent);


            Macro MS1 = new Macro("  Stop Client1 services", Macro.MacroActionType.Stop);
            MS1.AddStopService ("MSSQL$MSSQLSERVER2019");
            MS1.AddStopService("Tomcat9");
            MS1.AddStopService("CWSVCGWCGenerateNPPI");
            MS1.AddStopService("CWSVCGWCMonitoring");
            MS1.AddStopService("CWSVCGWCMonitoringInward");
            MS1.AddStopService("CWSVCGWTransferNPPI");
            MS1.AddStopService("CWSVCGWTransferOutward");
            MS1.AddStopService("CWSVCGWCBahGenerateCHS");


            Macro MS2 = new Macro("  Start Others services", Macro.MacroActionType.Start);
            MS2.AddStartService ("MSSQL$SQLEXPRESS2017");
            MS2.AddStartService("Tomcat");
            MS2.AddStartService("CWBatchAuditMatch");
            MS2.AddStartService("CWBatchHousekeep");
            MS2.AddStartService("CWBatchProcessInwardFile");
            MS2.AddStartService("CWBatchProcessInwRtnFile");




            M.lstSubMacro.Add(MS1);
            M.lstSubMacro.Add(MS2);

            QuMacro.Enqueue(MS1);
            QuMacro.Enqueue(MS2);
            StartTimer();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Macro M = new Macro("Switch to Client 1", Macro.MacroActionType.Parent);


            Macro MS1 = new Macro("  Stop Client1 services", Macro.MacroActionType.Stop);
            MS1.AddStopService ("MSSQL$MSSQLSERVER2019");
            MS1.AddStopService("Tomcat9");
            MS1.AddStopService("CWSVCGWCGenerateNPPI");
            MS1.AddStopService("CWSVCGWCMonitoring");
            MS1.AddStopService("CWSVCGWCMonitoringInward");
            MS1.AddStopService("CWSVCGWTransferNPPI");
            MS1.AddStopService("CWSVCGWTransferOutward");
            MS1.AddStopService("CWSVCGWCBahGenerateCHS");


            Macro MS2 = new Macro("  Start Others services", Macro.MacroActionType.Start);
            MS2.AddStartService("MSSQL$SQLEXPRESS2017");
            MS2.AddStartService("Tomcat");
            MS2.AddStartService("CWBatchAuditMatch");
            MS2.AddStartService("CWBatchHousekeep");
            MS2.AddStartService("CWBatchProcessInwardFile");
            MS2.AddStartService("CWBatchProcessInwRtnFile");




            M.lstSubMacro.Add(MS1);
            M.lstSubMacro.Add(MS2);

            QuMacro.Enqueue(MS1);
            QuMacro.Enqueue(MS2);

           // RunMacroUsingTask(QuMacro.Dequeue());

            StartTimer();


        }

        private void button15_Click(object sender, EventArgs e)
        {

        }
    }
}
