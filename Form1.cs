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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            /*
            panel1.Visible = false;
            this.enu.CheckedChanged += new EventHandler(this.radioBtn_CheckedChange);
            this.diu.CheckedChanged += new EventHandler(this.radioBtn_CheckedChange);
            this.unu.CheckedChanged += new EventHandler(this.radioBtn_CheckedChange);
                */
            label7.Text = netcheck.PingResult("tls.ad");
        }
        private static bool _login = false;

        // private string strValue;
        public bool _Login
        {
            set
            {
                _login = value;
            }
            get
            {
                return _login;
            }
        }

        private DataTable Dt;
        public DataTable dt
        {
            set
            {
                Dt = value;
            }
            get
            {
                return Dt;
            }
        }


        private string Area;

        public string area
        {
            get
            {
                return Area;
            }
            set
            {
                Area = value;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (usn.Text == "" || npwd.Text == "" || compwd.Text == "")
            {
                MessageBox.Show("either username or password can not be blank!");
                return;
            }
            if (npwd.Text != compwd.Text)
            {
                MessageBox.Show("new password different with second one！");
                return;
            }
            /*
            Form2 f = new Form2();
            f.Owner = this;
            f.ShowDialog();
            */

            string adUser = usn.Text;
            string nPasswd = npwd.Text;
            string comPwd = compwd.Text;

            string code = ADHelpers.SetPasswordByAccount(adUser, nPasswd);
            string result = "";
            switch (code)
            {
                case "-2146232828":
                    result = "Reset Failed!New Password does not meet domain policy";
                    break;
                case "-2147467261":
                    result = "Can not find this user in domain";
                    break;
                default:
                    result = "Password reset finished！";
                    break;
            }
            MessageBox.Show(result);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            // this.Close(); keep memory
            // this.Dispose(); will release memory 
            Application.Exit();
        }



        private void compwd_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (compwd.Text != npwd.Text)
            {
                label5.Visible = true;
                label5.Text = "diff";
            }
            else
            {
                label5.Text = "ok";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!_login)
            {
                Form2 f = new Form2(this.button3.Text);
                f.Owner = this;
                f.StartPosition = FormStartPosition.Manual;
                f.Location = this.PointToScreen(new Point((this.Width - f.Width) / 2, (this.Height - f.Height) / 2));
                f.ShowDialog();
                
                label4.Visible = true;
                label8.Text = this.Area;
                // this.button3.Text = "Switch Account";
            }

            // usn.DataSource = ADHelpers.GetAllUsers("sAMAccountName");

            if (dt != null && dt.Columns.Contains("samaccountname"))
            {
                usn.DataSource = dt;
                usn.DisplayMember = "samaccountname";
            }

        }

        /*
        private void radioBtn_CheckedChange(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
            {
                return;
            }
            string locationShow = string.Empty;
            switch (((RadioButton)sender).Text.ToString())
            {
                case "Enable User":
                    locationShow = "启用用户";
                    this.label4.Text = locationShow;
                    break;
                case "Disable User":
                    locationShow = "禁用用户";
                    this.label4.Text = locationShow;
                    break;
                case "Unlock User":
                    locationShow = "解锁用户";
                    this.label4.Text = locationShow;
                    break;
                default:
                    break;
            }

        }
         */

        private void button4_Click(object sender, EventArgs e)
        {
            /*
            if (rpc.Text.Length==0||rpc.Text.Contains(" "))
            {
                MessageBox.Show("Computer name can not be blank or contain space string");
                return;
            }
            */
            if (!_login)
            {
                Form2 f = new Form2(this.button4.Text);
                f.Owner = this;
                f.StartPosition = FormStartPosition.Manual;
                f.Location = this.PointToScreen(new Point((this.Width - f.Width) / 2, (this.Height - f.Height) / 2));
                f.ShowDialog();
                //this.button4.PerformClick();
                label4.Visible = true;
                label8.Text = this.Area;
            }
            //            else
            //            {
            if (this.button4.Text == "Remove PC")
            {
                switch (ADHelpers.RemoveComputerFromName(rpc.Text))
                {
                    case 1:
                        MessageBox.Show("computer not found in AD");
                        this.button4.Text = "Retrive PC";
                        break;
                    case 0:
                        MessageBox.Show("computer deleted");
                        this.button4.Text = "Retrive PC";
                        break;
                    default:
                        MessageBox.Show("unknow problems");
                        this.button4.Text = "Retrive PC";
                        break;
                }
            }

            if (dt != null && dt.Columns.Contains("pCName"))
            {
                rpc.DataSource = dt;
                rpc.DisplayMember = "pCName";
                this.button5.Visible = true;
                this.button4.Text = "Remove PC";
            }

            //            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!this._Login)
            {
                MessageBox.Show("Get AD users first");
                return;
            }
            if (ADHelpers.Unlock(usn.Text))
            {
                MessageBox.Show("Unlock finished");
            }
            else
            {
                MessageBox.Show("Unlock Failed");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this._Login = false;
            this.button4.Text = "Retrive PC";
            if (usn.Items.Count > 0 || rpc.Items.Count > 0)
            {
                rpc.DataSource = null;
                rpc.Items.Clear();
                usn.DataSource = null;
                usn.Items.Clear();
                this.Dt.Clear();
                this.Area = "Unknow";
            }
            npwd.Clear();
            compwd.Clear();
        }




    }
}
