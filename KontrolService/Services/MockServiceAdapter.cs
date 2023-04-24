using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace KontrolService.Services
{
    public class MockServiceAdapter : IServiceAdapter
    {
        /*
        private String _ServiceName = "";
        private String _MachineName = "";
        private String _DisplayName = "";
        private ServiceType _ServiceType;
        private bool _CanPauseAndContinue = false;
        private bool _CanShutdown = false;
        partial bool _CanStop = false;
        private ILog _Log = null;
        */
        public MockServiceAdapter()
        {
            _Properties = new ServiceAdapterProperties();
        }
        public MockServiceAdapter(ServiceAdapterProperties pProperties)
        {
            if (pProperties == null)
            {
                throw new Exception("Properties parameter cannot be null");
            }
            _Properties = pProperties;
        }
        private List<int> lstMemoryAllocate = null;
        void IServiceAdapter.Start()
        {
            //throw new NotImplementedException();
            System.Threading.Thread.Sleep(_MilisecondToStart);
            lstMemoryAllocate = new List<int>();

            int iNumberofInt = 262144 * _Megabyte;
            int i;
            for (i = 1; i < iNumberofInt; i++)
            {
                lstMemoryAllocate.Add(i);
            }
            _Properties.Status = ServiceControllerStatus.Running;

        }

        void IServiceAdapter.Stop()
        {
            //throw new NotImplementedException();
            System.Threading.Thread.Sleep(_MilisecondToStop);
            lstMemoryAllocate.Clear();
            _Properties.Status = ServiceControllerStatus.Stopped;
            


        }


        void IServiceAdapter.Continue()
        {
            //throw new NotImplementedException();
        }

        private ServiceAdapterProperties _Properties = null;
        public ServiceAdapterProperties Properties
        {
            get
            {
                return _Properties;
                //throw new NotImplementedException();
            }
            set
            {
                _Properties = value;
                //throw new NotImplementedException();
            }
        }


        public void WaitForStatus(ServiceControllerStatus status)
        {
            //throw new NotImplementedException();
        }

        public void WaitForStatus(ServiceControllerStatus status, TimeSpan timespan)
        {
            //throw new NotImplementedException();
        }
        private int _MilisecondToStart = 0;
        private int _MilisecondToStop = 0;
        private int _Megabyte = 0;
        public static  MockServiceAdapter NotGangof4Builder(String serviceName)
        {
            MockServiceAdapter m = new MockServiceAdapter();
            m.Properties.ServiceName = serviceName;
            return m;
        }
        public  MockServiceAdapter SetMilisecondToStart(int PMilisecondTOStart)
        {
            this._MilisecondToStart = PMilisecondTOStart;
            return this;
        }

        public MockServiceAdapter SetMilisecondToStop(int PMilisecondTOStop)
        {
            this._MilisecondToStop = PMilisecondTOStop;
            return this;
        }
        public MockServiceAdapter SetMegabytesAllocateForService(int pMeagabyte)
        {
            this._Megabyte = pMeagabyte;
            return this;
        }
        

        private String _beh = "";
        public String beh
        {
            get {
                return beh;
            }
        }
        public MockServiceAdapter SetBehaviour(String pbeh)
        {
            this._beh = pbeh;
            //this.Properties.Status = ServiceControllerStatus.
            return this;

        }
        /*
        public void LoadPhysicial(String serviceName, IControlDisplayvalue IControl)
        {
            
            if (this.Properties.LoadPhysicial == null)
            {
                throw new Exception(" LoadPhysicial function is null, you need to set this function on properties first");
            }
            string PhysicialPath = this.Properties.LoadPhysicial(this.Properties.ServiceName, IControl);
            this.Properties.PhysicialPath = PhysicialPath;

        }
        */

    }
}
