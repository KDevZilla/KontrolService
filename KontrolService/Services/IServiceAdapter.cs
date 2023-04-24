using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace KontrolService.Services
{



    public interface IServiceAdapter
    {
         void Start();
         void Stop();
        
         void WaitForStatus(ServiceControllerStatus status);
         void WaitForStatus(ServiceControllerStatus status,TimeSpan timespan);
         
         //void LoadPhysicialPath(LoadPhysicial L);
         //void LoadPhysicial(String serviceName, IControlDisplayvalue IControl);
         
        
        ServiceAdapterProperties Properties
        {
            set;
            get;
        }
        /*
        public String ServiceName
        {
            set;
            get;
        }
        public String MachineName
        {
            set;
            get;
        }
        public String DisplayName
        {
            set;
            get;
        }
        public ServiceType SercieType
        {
            set;
            get;
        }
        public bool CanPauseAndContinue
        {
            set;
            get;
        }

        public bool CanShutdown
        {
            set;
            get;
        }

        public bool CanStop
        {
            set;
            get;
        }
        */

         void Continue();

    }
}
