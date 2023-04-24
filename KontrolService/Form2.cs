using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KontrolService
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i;
            StringBuilder strB = new StringBuilder();
            for (i = 0; i < this.listView1.Items.Count; i++)
            {
                if (this.listView1.Items[i].Checked)
                {
                    strB.Append(this.listView1.Items[i].SubItems[1].Text );
                }
            }
            this.textBox1.Text = strB.ToString();

        }
        private void LoadListView()
        {
            this.listView1.Columns.Add("ServiceName");
            this.listView1.Columns.Add("Status");
            this.listView1.Columns[0].Width = 300;

            ListViewItem lvt = new ListViewItem("Tomcat7", "Stop");
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(lvt, "Stop");
            lvt.SubItems.Add(subItem);
            
            this.listView1.Items.Add(lvt);

            this.listView1.CheckBoxes = true;

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.LoadListView();
        }
    }
}
