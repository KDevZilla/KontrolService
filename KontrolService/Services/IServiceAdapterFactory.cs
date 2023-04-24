using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace KontrolService.Services
{
    public class IServiceAdapterFactory
    {
        public static IServiceAdapter Create(ServiceController sc)
        {
            return new ServiceAdapter(sc);
        }
        
        public static IServiceAdapter Create(String serviceName, bool IsMockeService, ServiceAdapterProperties pProperties)
        {
            if (serviceName.Trim() == "")
            {
                return new MockServiceAdapter();
            }

            if (!IsMockeService)
            {
                ServiceController Sc = new ServiceController(serviceName);
                if (pProperties == null)
                {
                    return new ServiceAdapter(Sc);
                    //return Create(new ServiceController(serviceName));
                }
                else
                {
                    //throw new Exception("There is properties object cannot be used with Service Controller");
                    //return Create(new ServiceController(serviceName));
                }
                
            }

            if (pProperties == null)
            {
                pProperties = new ServiceAdapterProperties();
                pProperties.ServiceName = serviceName;
                
            }
            
            return new MockServiceAdapter(pProperties);
            //return new ServiceAdapter(serviceName);
        }

        public static  IServiceAdapter Create(String serviceName, bool IsMockeService)
        {
            return Create(serviceName, IsMockeService, null);

          
            //return new ServiceAdapter(serviceName);
            //ServiceAdapter sc = new ServiceAdapter();
            //sc.CanPauseAndContinue
            //sc.CanShutdown
            //sc.CanStop
            //sc.DisplayName
            //sc.MachineName
            //sc.PhysicalPath
            //sc.SercieType
            //sc.StartMode
            
        }
        
        
    }

 
}
