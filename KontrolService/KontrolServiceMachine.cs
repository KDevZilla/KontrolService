using KontrolService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace KontrolService
{
    public class KontrolServiceMachine
    {
        List<Thread> lst = new List<Thread>();
        Dictionary<String, Boolean> DicThreadhasFinished = new Dictionary<string, bool>();
        Dictionary<string, cKontrolService> DicService = new Dictionary<string, cKontrolService>();
        public delegate void UpateServiceEventHandler(KontrolServiceMachine sender, String ServiceName, String Status);
        public delegate void UpateProgressEventHandler(KontrolServiceMachine sender, Boolean IsFinished);

        public  event UpateServiceEventHandler UpdateServiceEvent;
        public  event UpateProgressEventHandler UpdateProgressEvent;
        private Exception _Excep = null;
        private List<Exception> _lstExceptions = null;
        public List<Exception> lstException
        {
            get { return _lstExceptions; }
        }
        public Boolean HasException
        {
            get {
                if(_lstExceptions ==null)
                {
                    return true;
                }

                return _lstExceptions.Count == 0;
            }
        }
        private void ClearException()
        {
            _lstExceptions = new List<Exception>();
        }
        public ILog logObj = null;
        public void TestStart()
        {

        }
        private void Log(String message)
        {
            if(logObj != null)
            {
                logObj.Log(DateTime.Now.ToString("HH:mm:ssss ") + message);
            }
            
        }
        public void SetItems(List<String> lst)
        {
            foreach (String item in DicService.Keys )
            {
                DicService[item].IsChecked = false;
            }

            foreach (String checkeditem in lst)
            {
                DicService[checkeditem].IsChecked = true;
            }
        }
        public void StartCheckedServices()
        {
            int i;
            this.ClearException();

            DicThreadhasFinished.Clear();
            foreach (string serviceName in DicService.Keys)
            {
                if (!DicService[serviceName].IsChecked)
                {
                    continue;
                }
                IServiceAdapter service = DicService[serviceName].service;


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

                Thread t = new Thread(new ParameterizedThreadStart(StartService));
                t.Start(service);

                DicThreadhasFinished.Add(service.Properties.ServiceName, false);
            
            }
            this.DisplayServiceV2();
        }

        public Macro CurrentMacro = null;
        private void StartService(object obj)
        {
            IServiceAdapter sc = (IServiceAdapter)obj;
            try
            {

                UpdateService(sc, "Starting");
                sc.Start();
                UpdateService(sc);
                this.Log(sc.Properties.ServiceName + " starting");
                sc.WaitForStatus(ServiceControllerStatus.Running);
                if (sc.Properties.Status == ServiceControllerStatus.Running)
                {
                    this.Log(sc.Properties.ServiceName + " has started");
                    UpdateService(sc);

                }
            }
            catch (Exception ex)
            {
                HandleException(ex);

            }
            finally
            {
                DicThreadhasFinished[sc.Properties.ServiceName] = true;
                CheckIfNeedtoFireFinishedProgressEvent();
            }
        }
        private void HandleException(Exception ex)
        {
            this.Log("Exception :: " + ex.Message);
            _lstExceptions.Add(ex);
        }
        private void CheckIfNeedtoFireFinishedProgressEvent()
        {
            foreach (String key in DicThreadhasFinished.Keys)
            {
                if (DicThreadhasFinished[key] == false)
                {
                    return;
                }
            }
            if(UpdateProgressEvent != null)
            {
                UpdateProgressEvent(this, true);
            }
            if(CurrentMacro != null)
            {
                Log("Has finished ::" + CurrentMacro.Name);
            }
           // EnableButton(button7, true);
        }

        private void UpdateService(IServiceAdapter sc, String custom)
        {

            DicService[sc.Properties.ServiceName].service = sc;//ServiceAdapterFactory.Create(sc);
                                                               // UpdateListViewStatus(sc.Properties.ServiceName, custom);
            if (UpdateServiceEvent != null)
            {
                UpdateServiceEvent(this, sc.Properties.ServiceName, custom);
            }

        }
        private void DisplayServiceV2()
        {

            ServiceController[] services = ServiceController.GetServices();

            foreach (string strServiceName in DicService.Keys)
            {
                if (UpdateServiceEvent != null)
                {
                    UpdateServiceEvent(this, strServiceName, DicService[strServiceName].service.Properties.Status.ToString());
                }

            }

        }
        private void UpdateService(IServiceAdapter sc)
        {
            DicService[sc.Properties.ServiceName].service = sc;//ServiceAdapterFactory.Create(sc);

            if (UpdateServiceEvent != null)
            {
                UpdateServiceEvent(this, sc.Properties.ServiceName, sc.Properties.Status.ToString());
            }

        }
        private void StartServicesFromMacro(List<String> lstServices)
        {
            int i;
            //setCheckedItemV2();
            //lst.Clear();
            ClearException();
            DicThreadhasFinished.Clear();
            SetItems(lstServices);


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
                DicThreadhasFinished.Add(service.Properties.ServiceName, false);
                //service.WaitForStatus (ServiceControllerStatus.Running ,               
            }
            this.DisplayServiceV2();
        }
        public void StopCheckedServices()
        {
            int i;
            ClearException();
            foreach (string serviceName in DicService.Keys)
            {
                if (!DicService[serviceName].IsChecked)
                {
                    continue;
                }
                IServiceAdapter service = DicService[serviceName].service;
                if (ServiceHelper.GetServiceStartMode(serviceName) == enStartMode.Disabled)
                {
                    this.Log(serviceName + " start mode is disabled. Programi will skip stopping.");
                    continue;
                }

                if (service.Properties.Status != ServiceControllerStatus.Running)
                {
                    this.Log(serviceName + " start Status is not Running. Programi will skip stopping.");
                    continue;
                }
                if (!service.Properties.CanStop)
                {
                    this.Log(serviceName + " cannot be stopped.");
                    continue;
                }
                Thread t = new Thread(new ParameterizedThreadStart(StopService));
                t.Start(service);

                
            }

        }

        public void RestartCheckedServices()
        {
            int i;
            //setCheckedItemV2();
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



                Thread t = new Thread(new ParameterizedThreadStart(RestartService));
                t.Start(service);
                //service.Stop();
            }
            //this.DisplayService();
        }


        private void StopService(object obj)
        {
            try
            {
                //ServiceController sc = (ServiceController)obj;
                IServiceAdapter sc = (IServiceAdapter)obj;
                if (sc.Properties.Status != ServiceControllerStatus.Running)
                {
                    this.Log(sc.Properties.ServiceName + " service is not running, no need to stop");
                }
                UpdateService(sc, "Stopping");
                sc.Stop();
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
                
                HandleException(ex);
            }
        }

        private void RestartService(object obj)
        {
            StopService(obj);
            StartService(obj);
        }
    }
}
