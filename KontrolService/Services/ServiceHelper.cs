using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using KontrolService;
using System.Collections;

namespace KontrolService.Services
{
    public enum enStartMode
        {
            Automatic,
            Manual,
            Disabled,
            NoInformation
        }
   
   


    


    public class ServiceHelper
    {


       
        public static String GetAccountName(string serviceName)
        {
           
            RegistryKey fileServiceKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + 
                serviceName);

            string serviceAccountName = (String)fileServiceKey.GetValue("ObjectName");
            return serviceAccountName;

        }

        
        //public static LoadPhysicalPath(Service

        
        public static enStartMode  GetServiceStartMode(string serviceName)
        {
            uint success = 1;
            
            string filter = String.Format("SELECT * FROM Win32_Service WHERE Name = '{0}'", serviceName);

            ManagementObjectSearcher query = new ManagementObjectSearcher(filter);

            // No match = failed condition
            if (query == null)
            {
                return enStartMode.NoInformation;
            }

            enStartMode Result = enStartMode.NoInformation;

            try
            {
                ManagementObjectCollection services = query.Get();

                foreach (ManagementObject service in services)
                {
                    String strStartMode=service.GetPropertyValue("StartMode").ToString() ;
                    
                    switch  (strStartMode ){
                        case "Auto":
                            Result = enStartMode.Automatic;

                            break;
                        case "Manual":
                            Result = enStartMode.Manual;
                            break ;
                        case "Disabled":
                            Result = enStartMode.Disabled;
                            break ;
                        //case "":
                        //    break;
                        default :
                            throw new Exception("The value of StartMode is :" + strStartMode + " which is not match any item");
                    }

                    //return service.GetPropertyValue("StartMode").ToString() == "Auto" ? "Automatic" : "Manual";
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Result;
        }
    }
}
