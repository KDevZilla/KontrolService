using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KontrolService
{
    public class CallBack
    {

            public delegate void CallbackEventHandler(string something);
            public event CallbackEventHandler Callback;
            
            public void DoRequest(string request)
            {
                // do stuff....
                if (Callback != null)
                    Callback("bla");
            }
        
    }
}
