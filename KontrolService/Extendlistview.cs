using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KontrolService
{
    public class Extendlistview:ListView
    {
        //private Timer timerRefrerhTreeviewHeader = null;
        public Extendlistview()
        {

            this.OwnerDraw = true;
            this.DrawColumnHeader += ListView_DrawColumnHeader;
            this.DrawItem += ListView_DrawItem;
            this.DrawSubItem += ListView_DrawSubItem;
            this.ColumnClick += ListView_ColumnClick;
            this.ItemCheck += ListView_ItemCheck;

        }


       Boolean  IsCheckAll = false;
        private void ListView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //     this.timerRefrerhTreeviewHeader.Enabled = true;
            string str = "HEllo";
            int i = 0;
            IsCheckAll = true;
            if (e.NewValue == CheckState.Unchecked)
            {
                IsCheckAll = false;
            }
            else
            {
                for (i = 0; i < this.Items.Count; i++)
                {
                    //this.checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                    if(i==e.Index )
                    {
                        continue;
                    }
                    if (!this.Items[i].Checked)
                    {
                        
                        IsCheckAll = false;
                        break;
                    }

                }
            }
            // this.Log("ListView1_ItemCheck");
          //  this.Items [0].Bounds.
            this.Invalidate();
            this.Refresh();

        }
        private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(this.Columns[e.Column].Tag);
                }
                catch (Exception)
                {
                }
                this.Columns[e.Column].Tag = !value;
                foreach (ListViewItem item in this.Items)
                    item.Checked = !value;




            }
        }

        private void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
           
            if (e.ColumnIndex == 0)
            {
                e.DrawBackground();
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(e.Header.Tag);
                }
                catch (Exception)
                {
                }
                int i;
                //Boolean IsCheck = true;
                /*
                for (i = 0; i < this.Items.Count; i++)
                {
                    //this.checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                    if (!this.Items[i].Checked)
                    {
                        IsCheck = false;
                        break;
                    }

                }
                */
                this.Columns[0].Tag = IsCheckAll;
                CheckBoxRenderer.DrawCheckBox(e.Graphics,
                    new Point(e.Bounds.Left + 4, e.Bounds.Top + 4),
                    IsCheckAll  ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal :
                    System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            }
            else
            {
                e.DrawDefault = true;
            }
        }
    }
}
