using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace KontrolService.Services
{
    //public delegate String LoadPhysicialFunction(string serviceName,IControlDisplayvalue ControlDisplay);
    public class ServiceAdapterProperties
    {
        private String _ServiceName = "";
        private String _MachineName = "";
        private String _DisplayName = "";
        private ServiceType _ServiceType;
        private bool _CanShutdown = true;
        private bool _CanStop = true;
        private bool _CanPauseAndContinue = false;
        private ILog _Log = null;
        private ServiceControllerStatus _Status ;
       // private LoadPhysicialFunction _LoadPhysicial;
       // private String _PhysicialPath = "";
       /*
        public String PhysicialPath
        {
            set
            {
                _PhysicialPath = value;
            }
            get
            {
                return _PhysicialPath;
            }
        }
        */

        /*
        public LoadPhysicialFunction LoadPhysicial
        {
            get
            {
                return _LoadPhysicial;
            }
            set
            {
                _LoadPhysicial = value;
            }
            
        }
        */

        public ServiceControllerStatus Status
        {

            get
            {
                if (_sc != null)
                {
                   
                    return _sc.Status;
                }
                else
                {
                    return _Status;
                }
            }
            set
            {
                if (_sc != null)
                {
                    //_sc.ServiceControllerStatus = value;
                }
                else
                {
                    //_ServiceName = value;
                    _Status  = value;
                }
            }
        }

        public String ServiceName
        {

            get
            {
                if (_sc != null)
                {
                    return _sc.ServiceName;
                }
                else
                {
                    return _ServiceName;
                }
            }
            set
            {
                if (_sc != null)
                {
                    _sc.ServiceName = value;
                }
                else
                {
                    _ServiceName = value;
                }
            }
        }
        public String MachineName
        {
            get
            {
                if (_sc != null)
                {
                    return _sc.MachineName;
                }
                else
                {
                    return _MachineName;
                }
            }
            set
            {
                if (_sc != null)
                {
                    _sc.MachineName = value;
                }
                else
                {
                    _MachineName = value;
                }
            }
        }
        public String DisplayName
        {
            get
            {
                if (_sc != null)
                {
                    return _sc.DisplayName;
                }
                else
                {
                    return _DisplayName;
                }
            }
            set
            {
                if (_sc != null)
                {
                    _sc.DisplayName = value;
                }
                else
                {
                    _DisplayName = value;
                }
            }
        }
        public ServiceType ServiceType
        {
            get
            {
                if (_sc != null)
                {
                    return _sc.ServiceType;
                }
                else
                {
                    return ServiceType;
                }
            }
            set
            {
                if (_sc != null)
                {
                    //    _sc.ServiceType = value;
                    //    _sc.ServiceType = value;
                }
                else
                {
                    //  _ServiceName  = value;
                    _ServiceType = value;
                }
            }
        }
        public bool CanPauseAndContinue
        {
            get
            {
                if (_sc != null)
                {
                    return _sc.CanPauseAndContinue;
                }
                else
                {
                    return _CanPauseAndContinue;
                }
            }
            set
            {
                if (_sc != null)
                {
                    //_sc.CanPauseAndContinue = value;

                }
                else
                {
                    _CanPauseAndContinue = value;
                }
            }
        }
        public bool CanShutdown
        {
            get
            {
                if (_sc != null)
                {
                    return _sc.CanShutdown;
                }
                else
                {

                    return _CanShutdown;
                }
            }
            set
            {
                if (_sc != null)
                {
                    //_sc.CanPauseAndContinue = value;

                }
                else
                {
                    _CanShutdown = value;
                }
            }
        }


        public bool CanStop
        {
            get
            {
                if (_sc != null)
                {
                    return _sc.CanStop;
                }
                else
                {
                    return _CanStop;
                }
            }
            set
            {
                if (_sc != null)
                {
                    //_sc.CanPauseAndContinue = value;

                }
                else
                {
                    _CanStop = value;
                }
            }
        }

        public ILog Log
        {
            get
            {

                return _Log;

            }
            set
            {
                _Log = value;

            }
        }

        private ServiceController _sc = null;
        public ServiceAdapterProperties()
        {
        }
        public ServiceAdapterProperties(ServiceController sc)
        {
            _sc = sc;
        }

    }
}
