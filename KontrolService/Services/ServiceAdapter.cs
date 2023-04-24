using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace KontrolService.Services
{
    public class ServiceAdapter : IServiceAdapter
    {
        public string s = "";
        public void Test()
        {
            ServiceController SC = new ServiceController();
           
            //SC.ServiceName 
        }
        private ServiceController _Service = null;
        /*
        private string _PhysicalPath = "";
        private string LoadPhysicalPath()
        {
            return "";
        }
        */

        public ServiceController Service
        {
            get
            {
                return _Service;
            }
        }

        public ServiceAdapter(string servicename)
        {
            _Service = new ServiceController(servicename);
        }
        public ServiceAdapter(ServiceController pSc)
        {
            _Service = pSc;
            _Properties = new ServiceAdapterProperties(pSc);
          
        }
        public void Start()
        {
            _Service.Start();
        }
        public void Stop()
        {
            _Service.Stop();
        }

        

        private String _StartMode;
        public string StartMode
        {
            get
            {
                return "";
                

            }
        }
        
        void C_Callback(string something)
        {

        }



        public void Continue()
        {
            //throw new NotImplementedException();
            _Service.Continue();
        }




        public void WaitForStatus(ServiceControllerStatus status)
        {
            //throw new NotImplementedException();
            _Service.WaitForStatus(status);
        }

        public void WaitForStatus(ServiceControllerStatus status, TimeSpan timespan)
        {
            //throw new NotImplementedException();
            _Service.WaitForStatus(status, timespan);
        }
        private ServiceAdapterProperties _Properties;
        public ServiceAdapterProperties Properties
        {
            get
            {
               // throw new NotImplementedException();
                return _Properties;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /*
        public void LoadPhysicial()
        {
            throw new NotImplementedException();
        }
        */
        /*
        public void LoadPhysicial(String serviceName, IControlDisplayvalue IControl)
        {
            if (this.Properties.LoadPhysicial == null)
            {
                throw new Exception(" LoadPhysicial function is null, you need to set this function on properties first");
            }

            string PhysicialPath = this.Properties.LoadPhysicial(serviceName , IControl);
            this.Properties.PhysicialPath = PhysicialPath;

        }
        */

    }
}
