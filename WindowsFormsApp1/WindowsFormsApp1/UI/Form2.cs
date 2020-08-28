using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using ActUtlTypeLib;

namespace AddOn
{
    public partial class Form2 : Form
    {
       // public ActUtlType plc = new ActUtlType();
        private Form1 form1;
        public Form2 (Form1 form)
        {
            form1 = form ?? throw new ArgumentNullException(nameof(form));
            InitializeComponent();
            TXTPASS.UseSystemPasswordChar = true;
            //plc.ActLogicalStationNumber = 1;
            //plc.Open();
            //plc.Connect();
        }
        public Form2()
       {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            TXTUSER.Focus();
            TXTPASS.Focus();
            timer1.Start();
            

        }
        DateTime realtime = DateTime.Now;
        string password = "";
        private void LOGINSYSTEM()///////////////////////////CHECKING LOGIN//////////////////////////////
        {
            if (TXTUSER.Text.Length == 0 && TXTPASS.Text.Length == 0)

                MessageBox.Show("Please enter your username and password!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

            else
            {
                if (TXTUSER.Text.Length == 0)
                    MessageBox.Show("Please enter your username!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                else
                {
                    if (TXTPASS.Text.Length == 0)
                        MessageBox.Show("Please enter your password!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                    else
                    {
                        if ((TXTUSER.Text == "Admin") && (TXTPASS.Text == password))
                            MessageBox.Show("Login Success!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        else
                           if (TXTUSER.Text != "Admin")
                            MessageBox.Show("Username entered is incorrect.Please again.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        if (TXTPASS.Text != password)
                            MessageBox.Show("Password entered is incorrect.Please again.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                    }
                }

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        ///////////////////////PASSWORD DANG NHAP//////////////////////
        private void timer1_Tick(object sender, EventArgs e)  
        {
            string setpass = DateTime.Now.ToLongTimeString();
            string passmonth = DateTime.Now.Month.ToString();
            var cvpassmonth = Convert.ToInt32(passmonth);
            if (cvpassmonth <= 9) passmonth = "0" + passmonth;
            string passhour = DateTime.Now.Hour.ToString();
            var cvpasshour = Convert.ToInt32(passhour);
            if (cvpasshour <= 9) passhour = "0" + passhour;
            string passdate = DateTime.Now.Day.ToString();
            var cvpassdate = Convert.ToInt32(passdate);
            if (cvpassdate <= 9) passdate = "0" + passdate;
            string passminute = DateTime.Now.Minute.ToString();
            var cvpassminute = Convert.ToInt32(passminute);
            if (cvpassminute <= 9) passminute = "0" + passminute;
            String passyear = DateTime.Now.Year.ToString();
            //password = passdate + passmonth + passyear + passhour + passminute;
            password = "3333";
        }
        ////////////////////////////////////////////////////////////////////////
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                TXTPASS.UseSystemPasswordChar = false;
            else
                TXTPASS.UseSystemPasswordChar = true;
        }

        private void BTN_LOGIN_Click_1(object sender, EventArgs e)
        {
            //plc.SetDevice("M0", 0);
            LOGINSYSTEM();
            if ((TXTUSER.Text == "Admin") && (TXTPASS.Text == password))
            {
                this.Close();
                form1.onLoginSucess();

            }
        }
    }
}
