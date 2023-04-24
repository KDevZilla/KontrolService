using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KontrolService
{
    [Serializable()]
    [XmlRoot("Project")]
    public class Project
    {
        private string _Name = "";
        [XmlElement("Name")]
        public string Name
        {

            get { return _Name; }
            set { _Name = value; }
        }
        
        private  HashSet<String> _hshServiceName = null;

        [XmlElement("HshServiceName")]
        public HashSet<String> HshServiceName
        {
            get { return _hshServiceName; }
            set { _hshServiceName = value; }
        }
        


        
        public Project(String pName)
        {
            Initial(pName, null);
        }
        public void LoadService(HashSet<String> pHshService)
        {
            if (pHshService != null)
            {
                
                _hshServiceName = new HashSet<string>();
                foreach (string str in pHshService)
                {
                    _hshServiceName.Add(str);
                }
                

            }
        }
        private void Initial(String pName,HashSet <String> pHshService)
        {
            this._Name = pName;
            LoadService(pHshService);
        }

        private  Project()
        {

        }
        public Project(String pName, HashSet<String> phshService)
        {
            Initial(pName, phshService);
        }
        
        private List<Macro> _lstMacro = null;
        [XmlElement("lstMacro")]
        public List<Macro> lstMacro
        {
            get
            {
                if(_lstMacro ==null)
                {
                    _lstMacro = new List<Macro>();
                }
                return _lstMacro;
            }
            set
            {
                _lstMacro = value;
            }
        }
        

    }
}
