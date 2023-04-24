using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KontrolService
{
    public class ServiceAction
    {
        private string _ServiceName = "";
        public enum ServiceActionenum
        {
            Start,
            Stop
        }
        private ServiceActionenum _Action = ServiceActionenum.Start;
        [XmlElement("Action")]
        public ServiceActionenum Action
        {

            get { return _Action; }
            set { _Action = value; }
        }

        [XmlElement("ServiceName")]
        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }
        }

        public ServiceAction (String pServiceName, ServiceActionenum pAction)
        {
            _ServiceName = pServiceName;
            _Action = pAction;
        }
        private ServiceAction()
        {

        }
    }
}
