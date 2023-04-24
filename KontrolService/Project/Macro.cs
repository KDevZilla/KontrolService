using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KontrolService
{
    public class Macro
    {
        public enum MacroActionType
        {
            Start,
            Stop,
            Parent
        }
        
        public MacroActionType ActionType = MacroActionType.Start;

        private String _Name = "";
        [XmlElement("Name")]
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /*
        private List<String> _lstService = new List<string>();
        public List<String> lstService
        {
            get
            {
                return _lstService;
            }
            set
            {
                _lstService = value;
            }
        }
        */
        //private Dictionary<String, ServiceAction.ServiceActionenum> _DicServiceAction = new Dictionary<string, ServiceAction.ServiceActionenum>();
        private List<ServiceAction> _lstServiceAction = new List<ServiceAction>();
        
        public List<ServiceAction> lstServiceAction
        {
            get
            {
                return _lstServiceAction;
            }
            set
            {
                _lstServiceAction = value;
            }
        }
        public Macro (String pName, MacroActionType pActionType)
        {
            _Name = pName;
            ActionType = pActionType;
        }
        public void AddStartService(String serviceName)
        {
            AddService(serviceName, ServiceAction.ServiceActionenum.Start);
        }
        public void AddStopService(String serviceName)
        {
            AddService(serviceName, ServiceAction.ServiceActionenum.Stop);
        }
        public void AddService(String serviceName, ServiceAction.ServiceActionenum Action )
        {
            _lstServiceAction.Add(new ServiceAction(serviceName, Action));
            //_DicServiceAction.Add(serviceName, Action);
        }

        private Macro()
        {

        }
        private List<Macro> _lstSubMacro = new List<Macro> ();
        public List<Macro> lstSubMacro
        {
            get
            {
                return _lstSubMacro;
            }
            set
            {
                _lstSubMacro = value;
            }
        }


    }
}
