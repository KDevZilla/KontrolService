using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KontrolService
{
    [Serializable]
    [XmlRoot("Setting")]
    public class Setting
    {
        private string _CurrentProjectFilePath = "";
        [XmlElement("CurrentProjectFilePath")]
        public string CurrentProjectFilePath
        {

            get { return _CurrentProjectFilePath; }
            set { _CurrentProjectFilePath = value; }
        }
    }
}
