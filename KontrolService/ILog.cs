using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KontrolService
{
    public interface ILog
    {
         void Log(string message);
    }
    public class TxtFileLog : ILog
    {

        public void Log(string message)
        {
            //throw new NotImplementedException();
            System.IO.StreamWriter SW = new System.IO.StreamWriter(_Path);
            SW.Write(DateTime.Now.ToString("HH:mm:ssss ") + message);
            SW.Close();

        }
        private string _Path = "";
        
        public TxtFileLog(String Path)
        {
        //    System.IO.StreamWriter SW = new System.IO.StreamWriter(Path);
            _Path = Path;
            
        }

    }

    public class TxtBoxLog : ILog
    {
        delegate void LogCallback(string text);
        public void Log(string message)
        {
            //throw new NotImplementedException();
            if (_txtBoxLog.InvokeRequired)
            {
                LogCallback l = new LogCallback(Log);
                System.Windows.Forms.Form f = _txtBoxLog.FindForm();

                f.Invoke(l, new object[] { message });
            }
            else
            {
                _txtBoxLog.AppendText(DateTime.Now.ToString("HH:mm:ssss ") + message);
                _txtBoxLog.AppendText(Environment.NewLine);
            }

        }
        private System.Windows.Forms.TextBox _txtBoxLog = null;

        public TxtBoxLog(System.Windows.Forms.TextBox  txtBoxLog)
        {
            //    System.IO.StreamWriter SW = new System.IO.StreamWriter(Path);
            _txtBoxLog = txtBoxLog;

        }

    }
}
