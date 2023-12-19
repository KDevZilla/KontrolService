using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KontrolService
{
   // public delegate void setCheckedItemEventHandler(object sender, List<String> lst);
    public class MediatorService
    {
        
        private KontrolServiceMachine con = new KontrolServiceMachine();

        public KontrolServiceUI ServiceUIObj { get; set; } = null;
        public void Initial()
        {
            con.UpdateProgressEvent += Con_UpdateProgressEvent;
            con.UpdateServiceEvent += Con_UpdateServiceEvent;
            
           // UI.setCheckItemEvent += UI_setCheckItemEvent;
        }

        private void UI_setCheckItemEvent(object sender, List<string> lst)
        {
            con.SetItems(lst);
        }

        private void Con_UpdateServiceEvent(KontrolServiceMachine sender, string ServiceName, string Status)
        {
            // throw new NotImplementedException();
            ServiceUIObj.UpdateServiceStatustoUI(ServiceName, Status);

        }

        private void Con_UpdateProgressEvent(KontrolServiceMachine sender, bool IsFinished)
        {
           // throw new NotImplementedException();

        }

        public void UpdateServiceStatustoUI(String strServiceName, String Status)
        {
            ServiceUIObj.UpdateServiceStatustoUI(strServiceName , Status);

        }
        public void setCheckItems(List<String> lst)
        {
            con.SetItems(lst);
        }

        public void Start()
        {
            con.StartCheckedServices();

        }
        

    }
    public interface KontrolServiceUI
    {
         void UpdateServiceStatustoUI(String strServiceName, String Status);
        // event setCheckedItemEventHandler setCheckItemEvent;




        
    }
}
