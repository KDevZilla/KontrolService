using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KontrolService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new frmTestService());
            //Application.Run(new frmHowToRunAppAsAdmin());
          //  Application.Run(new TestThread());

             Application.Run(new Form1());
            //Application.Run(new Form2());
            //Application.Run(new frmChooseServices());

        }
    }

}
