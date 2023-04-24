using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using KontrolService.Services;
namespace KontrolService
{
    public class cKontrolService
    {
        public IServiceAdapter service = null;
        public bool IsChecked = false;
        public cKontrolService(IServiceAdapter pservice)
        {
            Initial(pservice, false);
            
        }
        public cKontrolService(ServiceController sc)
        {
            Initial(IServiceAdapterFactory.Create(sc), false);
        }


        private void Initial(IServiceAdapter pservice, bool pIsChecked)
        {
            this.service = pservice;
            this.IsChecked = pIsChecked;
        }
        public cKontrolService(IServiceAdapter pservice, bool pIsChecked)
        {
            Initial(pservice, pIsChecked );
            List<IServiceAdapter> lst = new List<IServiceAdapter>();
            
        }
    }
}
