using KontrolService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace KontrolService
{
    public partial class frmChooseServices : Form
    {
        public frmChooseServices()
        {
            InitializeComponent();
        }
       
        public HashSet<String> hshServiceSelected { get; private set; } = null;

        public Boolean HasChooseServiceName { get; private set; } = false;
        

        private void button1_Click(object sender, EventArgs e)
        {
            hshServiceSelected = new HashSet<string>();
            int i;
    
            for (i = 0; i < this.listView1.Items.Count  ; i++)
            {
                if(!this.listView1.Items[i].Checked )
                {
                    continue;
                }
                hshServiceSelected.Add(this.listView1.Items [i].SubItems[NameColOrder].Text);
            }
            HasChooseServiceName = true;
            this.Close();

        }

        private void AddNewListItem(String serviceName,
         String serviceDisplayName)
        {
            ListViewItem lstViewItem = new ListViewItem();
            lstViewItem.Name = serviceName;
            lstViewItem.SubItems.Add(serviceName);
            lstViewItem.SubItems.Add(serviceDisplayName);

            this.listView1.Items.Add(lstViewItem);

        }
        const int NameColOrder = 1;
        const int DisplayNameColOrder = 2;

        private void frmChooseServices_Load(object sender, EventArgs e)
        {

            this.listView1.CheckBoxes = true;
            this.listView1.Columns.Add("#", 20, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Name", 300, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Display Name", 500, HorizontalAlignment.Left);


            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in ServiceController.GetServices())
            {
                string serviceName = service.ServiceName;
                string serviceDisplayName = service.DisplayName;
                string serviceType = service.ServiceType.ToString();
                string status = service.Status.ToString();


                AddNewListItem(serviceName, serviceDisplayName );



            }
            this.ActiveControl = txtSearch;


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private int LastItemFocusIndex = -1;
        private int NextIndex = -1;
        private void DeselectLastItem()
        {
            if(LastItemFocusIndex <0 || LastItemFocusIndex > this.listView1.Items.Count -1)
            {
                return;
            }
            listView1.Items[LastItemFocusIndex ].BackColor = Color.White;
            listView1.Items[LastItemFocusIndex].ForeColor = Color.Black;
        }

        private void SelectItem(int SelectedIndex)
        {
            if (SelectedIndex  < 0 || SelectedIndex > this.listView1.Items.Count - 1)
            {
                return;
            }

             //  listView1.Items[SelectedIndex].BackColor = Color.CornflowerBlue;
             // listView1.Items[SelectedIndex].ForeColor = Color.White ;
            listView1.Items[SelectedIndex].Selected = true;
            listView1.EnsureVisible(SelectedIndex);
            LastItemFocusIndex = SelectedIndex;

        }
        private void PerformSearch(String Criteria)
        {

            Criteria = Criteria.ToLower();

            Boolean CanfindItem = false;
            for(int i=NextIndex +1;i<listView1.Items.Count;i++)
            {
                ListViewItem item = listView1.Items[i];
                Boolean IsMatchNameOrDisplayName = false;
                IsMatchNameOrDisplayName = IsMatchNameOrDisplayName || item.SubItems[DisplayNameColOrder].Text.ToLower().IndexOf (Criteria) > -1;
                IsMatchNameOrDisplayName = IsMatchNameOrDisplayName || item.SubItems[NameColOrder].Text.ToLower().IndexOf (Criteria) > -1;

                if (IsMatchNameOrDisplayName)
                {

                    SelectItem(i);
                    CanfindItem = true;
                    break;

                }
               

            }

            if(!CanfindItem && NextIndex > -1)
            {
                NextIndex = -1;
                PerformSearch(Criteria);
            }
            //this.txtSearch.Focus();

        }
        private void TextChanged(object sender, EventArgs e)
        {
            PerformSearch(((TextBox)sender).Text);
        }

        private void txtSearchKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return )
            {
                NextIndex = LastItemFocusIndex;
                PerformSearch(((TextBox)sender).Text);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            HasChooseServiceName = false;
            this.Close();

        }
    }
}
