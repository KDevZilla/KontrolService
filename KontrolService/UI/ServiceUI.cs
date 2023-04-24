using KontrolService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KontrolService.UI
{
    public class ServiceUI : KontrolServiceUI 
    {
        public Extendlistview listview = new Extendlistview();
        private MediatorService Mediator = new MediatorService();
        delegate void LogCallback(string text);
        delegate void UpdateListViewStatusCallBack(String strServiceName, String Status);
        delegate void EnableButtonCallBack(Button b, Boolean IsEnable);
        public ServiceUI(Extendlistview plistview, MediatorService pMediator)
        {
            listview = plistview;
            Mediator = pMediator;
            Mediator.ServiceUIObj = this;

        }
        const String SERVICE_STARTUP_NOINFORMATION = "No information";
        const String SERVICE_STARTUP_AUTOMATIC = "Automatic";
        const String SERVICE_STARTUP_DISABLED = "Disabled";
        const String SERVICE_STARTUP_MANUAL = "Manual";

        const String SERVICE_STATUS_STARTING = "Starting";
        const String SERVICE_STATUS_STOPPING = "Stopping";
        const String SERVICE_STATUS_STOPPED = "Stopped";
        const String SERVICE_STATUS_RUNNING = "Running";

        const int NameColOrder = 1;
        int DisplayNameColOrder = 2;
        int ServiceStatusColOrder = 3;
        int ServiceStatupColOrder = 4;

        private Color _ColorStatusStopping = Color.White;
        private Color _ColorStatusStarting = Color.White;
        private Color _ColorStatusStopped = Color.Yellow ;
        private Color _ColorStatusRunning = Color.Green;

        private Color _ColorStartUpTypeDisabled = Color.Crimson;


        public  Color ColorStatusStopping
        {
            get
            {
                return _ColorStatusStopping;
            }
            set
            {
                _ColorStatusStopping = value;
            }
        }
        public  Color ColorStatusStarting
        {
            get
            {
                return _ColorStatusStarting;
            }
            set
            {
                _ColorStatusStarting = value;
            }
        }
        public Color ColorStatusStopped
        {
            get
            {
                return _ColorStatusStopped;
            }
            set
            {
                _ColorStatusStopped = value;
            }
        }
        public  Color ColorStatusRunning
        {
            get
            {
                return _ColorStatusRunning;
            }
            set
            {
                _ColorStatusRunning = value;
            }
        }

        public Color ColorStartUpTypeDisabled = Color.Crimson;
        private Form Form
        {
            get
            {
                if(listview.Parent.GetType () == typeof (Form) )
                {
                    return (Form) this.listview.Parent;
                }
                object tempForm = listview.Parent;
                while(tempForm.GetType () != typeof (Form))
                {
                    tempForm = ((Control)tempForm).Parent;
                }
                return (Form)tempForm;
            }
        }

        public void UpdateServiceStatustoUI(string ServiceName, string Status)
        {
            // throw new NotImplementedException();
            Boolean IsShowServiceStartUpType = false;

                if (this.listview.InvokeRequired)
                {
                    UpdateListViewStatusCallBack l = new UpdateListViewStatusCallBack(UpdateServiceStatustoUI);

                    this.Form.Invoke(l, new object[] { ServiceName, Status });
                }
                else
                {
                    ListViewItem lv = this.listview.Items[ServiceName];
                    lv.SubItems[ServiceStatusColOrder].Text = Status;
                    switch (Status)
                    {
                        case SERVICE_STATUS_STARTING :
                        case SERVICE_STATUS_STOPPING :
                            lv.UseItemStyleForSubItems = false;
                            lv.SubItems[ServiceStatusColOrder].BackColor = _ColorStatusStopping ;
                            if (IsShowServiceStartUpType)
                            {
                                lv.SubItems[ServiceStatupColOrder].BackColor = Color.White;
                            }
                            //lv.SubItems[3].BackColor = Color.White;
                            break;
                        case SERVICE_STATUS_STOPPED :
                            //lv.BackColor = Color.Yellow;
                            lv.UseItemStyleForSubItems = false;
                            lv.SubItems[ServiceStatusColOrder].BackColor = _ColorStatusStopped ;
                            if (IsShowServiceStartUpType)
                            {
                                lv.SubItems[ServiceStatupColOrder].BackColor = Color.White;
                            }
                            //lv.SubItems[3].BackColor = Color.Yellow ;
                            break;
                        case SERVICE_STATUS_RUNNING :
                            //lv.BackColor = Color.LightGreen;
                            lv.UseItemStyleForSubItems = false;
                            lv.SubItems[ServiceStatusColOrder].BackColor = _ColorStatusRunning ;
                            if (IsShowServiceStartUpType)
                            {
                                lv.SubItems[ServiceStatupColOrder].BackColor = _ColorStatusRunning;
                            }
                            //lv.SubItems[3].BackColor = Color.LightGreen;
                            break;

                    }
                    if (IsShowServiceStartUpType)
                    {
                        if (lv.SubItems[ServiceStatupColOrder].Text == SERVICE_STARTUP_DISABLED)
                        {
                            //lv.BackColor = Color.Crimson;
                            lv.UseItemStyleForSubItems = false;
                            lv.SubItems[NameColOrder].BackColor = _ColorStartUpTypeDisabled;
                            lv.SubItems[DisplayNameColOrder].BackColor = _ColorStartUpTypeDisabled;
                            lv.SubItems[ServiceStatusColOrder].BackColor = _ColorStartUpTypeDisabled;
                            lv.SubItems[ServiceStatupColOrder].BackColor = _ColorStartUpTypeDisabled;
                        }
                    }


                    //this.listview.Items[strServiceName].SubItems[2].Text = Status;

                
            }

        }

        Boolean IsShowServiceStartUpType = false;
        
        

    }
}
