using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TLS_AD_tool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string btnText)
        {
            InitializeComponent();
            this.btnstr = btnText;
        }

        private string btnstr;

        private void button1_Click(object sender, EventArgs e)
        {
            
            DirectoryEntry de = new DirectoryEntry();
            string username = textBox1.Text;
            string password = textBox2.Text;
            try
            {
                de = ADHelpers.GetDirectoryEntryByAccount(username, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
                return;
            }
            if (de == null)
            {
                MessageBox.Show("Auth Failed or Account Locked!");
                return;
            }

            string group = "";

            var memberGroups = de.Properties["memberOf"].Value;

            if (memberGroups.GetType() == typeof(string))
            {
                group = (String)memberGroups;
            }
            else if (memberGroups.GetType().IsArray)
            {
                var memberGroupsEnumerable = memberGroups as IEnumerable;
                if (memberGroupsEnumerable != null)
                {
                    var asStringEnumerable = memberGroupsEnumerable.OfType<object>().Select(obj => obj.ToString());
                    group = String.Join(", ", asStringEnumerable);
                }
            }
            else
            {
                group = "";
            }

            if (group.IndexOf("gu.local_account.adm") == -1)
            {
                MessageBox.Show("You have no right to access GroupInfo");
                return;
            }


            //GetOU
              string OU = de.Parent.Parent.Parent.Name.Substring(3);

            DataTable dt = new DataTable();
            string column = "";
            switch (this.btnstr)
            {
                case "Retrive PC":
                    column = "pCName";
            dt.Columns.Add(column);
            dt = GetComputers(de.Parent.Parent.Parent.Children,dt);
                    break;
                case "Get AD user":
                    column = "sAMAccountName";
            dt.Columns.Add(column);
            dt = GetUsers(de.Parent.Parent.Children,dt);
                    break;
                default:
                    break;
            }
            /*
            string column = "sAMAccountName";
            dt.Columns.Add(column);
            dt = GetUsers(de.Parent.Parent.Children,dt);
             * */
            DataTable dtCopy = dt.Copy();
            DataView dv = dt.DefaultView;
            dv.Sort = column;
            dtCopy = dv.ToTable();
            Form1 f1 = (Form1)this.Owner;
            f1.area = OU;
            f1._Login = true;
            f1.dt = dtCopy;
            this.Close();
            /*
            if (textBox1.Text=="superadmin"&&textBox2.Text=="123")
            {
                Form1 f1 = (Form1)this.Owner;//把Form2的父窗口指针赋给Form1
                f1.StrValue = "pass";//使用父窗口指针赋值
                this.Close();
            }
             * */
        }


        private DataTable GetUsers(DirectoryEntries des,DataTable dt)
        {

            //  string column = "sAMAccountName";
            // dt.Columns.Add(column);
            foreach (DirectoryEntry dec in des)
            {
                if (dec != null)
                {
                    if (dec.SchemaClassName == "organizationalUnit")
                    {
                        GetUsers(dec.Children, dt);
                    }

                    DataRow dr = dt.NewRow();
                    dr["sAMAccountName"] = dec.Properties["sAMAccountName"].Value;
                    if (dr["sAMAccountName"].ToString() != "")
                    {
                        dt.Rows.Add(dr);
                    }

                }
            }

            return dt;
        }


        private DataTable GetComputers(DirectoryEntries des, DataTable dt)
        {
            foreach (DirectoryEntry dec in des)
            {
                if (dec != null)
                {
                    if (dec.Name.Substring(3) == "Computers")
                    {
                        GetComputers(dec.Children, dt);
                    }

                    DataRow dr = dt.NewRow();
                    if (dec.SchemaClassName=="computer")
                    {
                        dr["pCName"] = dec.Name.Substring(3);
                        if (dr["pCName"].ToString() != "")
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                    
                }
            }

            return dt;
        }


    }
}
