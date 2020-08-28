using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActUtlTypeLib;
using AddOn.Entity;
using AddOn.Utils;

namespace AddOn
{

    public partial class Form1 : Form
    {
        public ActUtlType plc = new ActUtlType();

        private string[] listmay = { "BM0","BM1", "BM2", "BM3", "BM4", "BM9", "KM2","KM3", "KM4", "KM5", "REGRIND",
                                     "2F1","2F2","2F3","2F4","2F5","2F6","2F7","2F8","2F9","2F10","KABAR"};// 22 MACHINE
        private string[] listdiachi = {"Y0","Y1", "Y2", "Y3", "Y4", "Y5", "Y6", "Y7", "Y10", "Y11", "Y12", "Y13", "Y14","Y15",
                                         "Y16","Y17","Y20","Y21","Y22","Y23","Y24","Y25"}; // 22 dia chi
        private string[] Addres_timer = { "TN0","TN1", "TN2", "TN3", "TN4", "TN5", "TN6","TN7", "TN8", "TN9", "TN10",
                                     "TN11","TN12","TN13","TN14","TN15","TN16","TN17","TN18","TN19","TN20","TN21"};
        public Form1()
        {

            InitializeComponent();

            lastestMachineState = new List<MachineState>();
            using (var db = new MyDbContext())
            {
                for (int i = 0; i < listmay.Length; i++)
                {
                    var machineId = listmay[i];//ten may

                    var newMachine = new MachineState() { Name = machineId, Address = listdiachi[i], State = 1, AddressTimer = Addres_timer[i], Date = DateTime.Now, FlagDown = false };
                    lastestMachineState.Add(newMachine);

                }
            }

            Init();

            plc.ActLogicalStationNumber = 1;
            plc.Open();
            plc.Connect();
            plc.SetDevice("M0", 0);

            BTN_CONNECTION.BackColor = Color.Red;

            //định thời export excel mỗi ngày lúc 23h 59p
            new Scheduler().Start();
        }



        private List<MachineState> lastestMachineState;
        private bool forceToCal = false; 
        private void button4_Click(object sender, EventArgs e)
        {
            // plc.SetDevice(txtaddress.Text, Convert.ToInt16(txtvalues.Text));
          
        }
        //
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            int volume;
            plc.GetDevice("M384",out volume);
            if(volume==1)
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
            }    
            else
            {
                pictureBox2.Visible = true;
                pictureBox1.Visible = false;
            }    
           // var listToAdd = new List<MachineState>();
            
            
            //if (lastestMachineState == null)
            //{
            //    return;
           // }
            
            //foreach (MachineState mc in lastestMachineState)
            //{
                
                
               
            //}    
                


            
            // save state thay đổi vào database

           // if (!forceToCal)
            //{
           ///     forceToCal = true;
           // }
            //if (listToAdd.Count > 0)
            //{
            //    Task.Run(() =>
            //    {
            //        using(var db = new MyDbContext())
            //        {
            //            db.MachineCollection.InsertBulk(listToAdd);
            //        }
            //    });
            //}
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            timer3.Start();
           
            

        }

      
        private void Init()
        {
            if (!Program.IsLogin)
            {
               
               
                PIC_CONNECT.Visible = false;
                PIC_DISCONNECT.Visible = true;
                BTN_CONNECTION.Visible = false;
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                hideobject();
                Form2 lg = new Form2(this);
                lg.ShowDialog();
            }
            else
            {
                BTN_CONNECTION.Visible = true;
                pictureBox1.Visible = true;
                pictureBox2.Visible = true;
                PIC_CONNECT.Visible = true;
                PIC_DISCONNECT.Visible = false;
                showobject();

            }
        }
        public void onLoginSucess()
        {
            BTN_CONNECTION.Visible = true;
            PIC_DISCONNECT.Visible = false;
            PIC_CONNECT.Visible = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            showobject();
         
        }
        private void PIC_DISCONNECT_Click(object sender, EventArgs e)//logout
        {
            plc.SetDevice("M0", 0);
            hideobject();
            Program.IsLogin = false;
            BTN_CONNECTION.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            Form2 lg = new Form2(this);
            lg.ShowDialog();
        }

        private void PIC_CONNECT_Click(object sender, EventArgs e)//login
        {
            plc.SetDevice("M0", 0);
            BTN_CONNECTION.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            hideobject();
            Form2 lg = new Form2(this);
            lg.ShowDialog();

            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lbldate.Text = DateTime.Now.ToLongTimeString();
            lbltime.Text = DateTime.Now.ToLongDateString();
        
        }

        private void showobject ()
        {
            BTN_MB0.BackColor = Color.White; TXT_DB0.BackColor = Color.White;
            BTN_MB1.BackColor = Color.White; TXT_DB1.BackColor = Color.White;
            BTN_MB2.BackColor = Color.White; TXT_DB2.BackColor = Color.White;
            BTN_MB3.BackColor = Color.White; TXT_DB3.BackColor = Color.White;
            BTN_MB4.BackColor = Color.White; TXT_DB4.BackColor = Color.White;
            BTN_MB9.BackColor = Color.White; TXT_DB9.BackColor = Color.White;
            BTN_MK2.BackColor = Color.White; TXT_DK2.BackColor = Color.White;
            BTN_MK3.BackColor = Color.White; TXT_DK3.BackColor = Color.White;
            BTN_MK4.BackColor = Color.White; TXT_DK4.BackColor = Color.White;
            BTN_MK5.BackColor = Color.White; TXT_DK5.BackColor = Color.White;
            BTN_MREG.BackColor = Color.White; TXT_DREG.BackColor = Color.White;
           
            BTN_M2F1.BackColor = Color.White; TXT_D2F1.BackColor = Color.White;
            BTN_M2F2.BackColor = Color.White; TXT_D2F2.BackColor = Color.White;
            BTN_M2F3.BackColor = Color.White; TXT_D2F3.BackColor = Color.White;
            BTN_M2F4.BackColor = Color.White; TXT_D2F4.BackColor = Color.White;
            BTN_M2F5.BackColor = Color.White; TXT_D2F5.BackColor = Color.White;
            BTN_M2F6.BackColor = Color.White; TXT_D2F6.BackColor = Color.White;
            BTN_M2F7.BackColor = Color.White; TXT_D2F7.BackColor = Color.White;
            BTN_M2F8.BackColor = Color.White; TXT_D2F8.BackColor = Color.White;
            BTN_M2F9.BackColor = Color.White; TXT_D2F9.BackColor = Color.White;
            BTN_M2F10.BackColor = Color.White; TXT_D2F10.BackColor = Color.White;
            BTN_MKAB.BackColor = Color.White; TXT_DKAB.BackColor = Color.White;

            BTN_PB0.BackColor = Color.White; TXT_CB0.BackColor = Color.White;
            BTN_PB1.BackColor = Color.White; TXT_CB1.BackColor = Color.White;
            BTN_PB2.BackColor = Color.White; TXT_CB2.BackColor = Color.White;
            BTN_PB3.BackColor = Color.White; TXT_CB3.BackColor = Color.White;
            BTN_PB4.BackColor = Color.White; TXT_CB4.BackColor = Color.White;
            BTN_PB9.BackColor = Color.White; TXT_CB9.BackColor = Color.White;
            BTN_PK2.BackColor = Color.White; TXT_CK2.BackColor = Color.White;
            BTN_PK3.BackColor = Color.White; TXT_CK3.BackColor = Color.White;
            BTN_PK4.BackColor = Color.White; TXT_CK4.BackColor = Color.White;
            BTN_PK5.BackColor = Color.White; TXT_CK5.BackColor = Color.White;
            BTN_PREG.BackColor = Color.White; TXT_CREG.BackColor = Color.White;
            BTN_P2F1.BackColor = Color.White; TXT_C2F1.BackColor = Color.White;
            BTN_P2F2.BackColor = Color.White; TXT_C2F2.BackColor = Color.White;
            BTN_P2F3.BackColor = Color.White; TXT_C2F3.BackColor = Color.White;
            BTN_P2F4.BackColor = Color.White; TXT_C2F4.BackColor = Color.White;
            BTN_P2F5.BackColor = Color.White; TXT_C2F5.BackColor = Color.White;
            BTN_P2F6.BackColor = Color.White; TXT_C2F6.BackColor = Color.White;
            BTN_P2F7.BackColor = Color.White; TXT_C2F7.BackColor = Color.White;
            BTN_P2F8.BackColor = Color.White; TXT_C2F8.BackColor = Color.White;
            BTN_P2F9.BackColor = Color.White; TXT_C2F9.BackColor = Color.White;
            BTN_P2F10.BackColor = Color.White; TXT_C2F10.BackColor = Color.White;
            BTN_PKAB.BackColor = Color.White; TXT_CKAB.BackColor = Color.White;

        }
        private void hideobject ()
        {
            BTN_MB0.BackColor = Color.Silver; TXT_DB0.BackColor = Color.Silver;
            BTN_MB1.BackColor = Color.Silver; TXT_DB1.BackColor = Color.Silver;
            BTN_MB2.BackColor = Color.Silver; TXT_DB2.BackColor = Color.Silver;
            BTN_MB3.BackColor = Color.Silver; TXT_DB3.BackColor = Color.Silver;
            BTN_MB4.BackColor = Color.Silver; TXT_DB4.BackColor = Color.Silver;
            BTN_MB9.BackColor = Color.Silver; TXT_DB9.BackColor = Color.Silver;
            BTN_MK2.BackColor = Color.Silver; TXT_DK2.BackColor = Color.Silver;
            BTN_MK3.BackColor = Color.Silver; TXT_DK3.BackColor = Color.Silver;
            BTN_MK4.BackColor = Color.Silver; TXT_DK4.BackColor = Color.Silver;
            BTN_MK5.BackColor = Color.Silver; TXT_DK5.BackColor = Color.Silver;
            BTN_MREG.BackColor = Color.Silver; TXT_DREG.BackColor = Color.Silver;
            BTN_M2F1.BackColor = Color.Silver; TXT_D2F1.BackColor = Color.Silver;
            BTN_M2F2.BackColor = Color.Silver; TXT_D2F2.BackColor = Color.Silver;
            BTN_M2F3.BackColor = Color.Silver; TXT_D2F3.BackColor = Color.Silver;
            BTN_M2F4.BackColor = Color.Silver; TXT_D2F4.BackColor = Color.Silver;
            BTN_M2F5.BackColor = Color.Silver; TXT_D2F5.BackColor = Color.Silver;
            BTN_M2F6.BackColor = Color.Silver; TXT_D2F6.BackColor = Color.Silver;
            BTN_M2F7.BackColor = Color.Silver; TXT_D2F7.BackColor = Color.Silver;
            BTN_M2F8.BackColor = Color.Silver; TXT_D2F8.BackColor = Color.Silver;
            BTN_M2F9.BackColor = Color.Silver; TXT_D2F9.BackColor = Color.Silver;
            BTN_M2F10.BackColor = Color.Silver; TXT_D2F10.BackColor = Color.Silver;
            BTN_MKAB.BackColor = Color.Silver; TXT_DKAB.BackColor = Color.Silver;

            BTN_PB0.BackColor = Color.Silver; TXT_CB0.BackColor = Color.Silver;
            BTN_PB1.BackColor = Color.Silver; TXT_CB1.BackColor = Color.Silver;
            BTN_PB2.BackColor = Color.Silver; TXT_CB2.BackColor = Color.Silver;
            BTN_PB3.BackColor = Color.Silver; TXT_CB3.BackColor = Color.Silver;
            BTN_PB4.BackColor = Color.Silver; TXT_CB4.BackColor = Color.Silver;
            BTN_PB9.BackColor = Color.Silver; TXT_CB9.BackColor = Color.Silver;
            BTN_PK2.BackColor = Color.Silver; TXT_CK2.BackColor = Color.Silver;
            BTN_PK3.BackColor = Color.Silver; TXT_CK3.BackColor = Color.Silver;
            BTN_PK4.BackColor = Color.Silver; TXT_CK4.BackColor = Color.Silver;
            BTN_PK5.BackColor = Color.Silver; TXT_CK5.BackColor = Color.Silver;
            BTN_PREG.BackColor = Color.Silver; TXT_CREG.BackColor = Color.Silver;
            BTN_P2F1.BackColor = Color.Silver; TXT_C2F1.BackColor = Color.Silver;
            BTN_P2F2.BackColor = Color.Silver; TXT_C2F2.BackColor = Color.Silver;
            BTN_P2F3.BackColor = Color.Silver; TXT_C2F3.BackColor = Color.Silver;
            BTN_P2F4.BackColor = Color.Silver; TXT_C2F4.BackColor = Color.Silver;
            BTN_P2F5.BackColor = Color.Silver; TXT_C2F5.BackColor = Color.Silver;
            BTN_P2F6.BackColor = Color.Silver; TXT_C2F6.BackColor = Color.Silver;
            BTN_P2F7.BackColor = Color.Silver; TXT_C2F7.BackColor = Color.Silver;
            BTN_P2F8.BackColor = Color.Silver; TXT_C2F8.BackColor = Color.Silver;
            BTN_P2F9.BackColor = Color.Silver; TXT_C2F9.BackColor = Color.Silver;
            BTN_P2F10.BackColor = Color.Silver; TXT_C2F10.BackColor = Color.Silver;
            BTN_PKAB.BackColor = Color.Silver; TXT_CKAB.BackColor = Color.Silver;


        }



        private void BTN_CONNECTION_Click(object sender, EventArgs e)
        {

            int readdata;
            short hour;
            short minute;
            short second;
            short year;
            short month;
            short day;
            short DW;
           
            string DayofWeek;
            string syear = DateTime.Now.Year.ToString();
            int index = syear.Length;
            string Year = syear.Substring(2, 2);
            year = Convert.ToInt16(Year);
            month = Convert.ToInt16(DateTime.Now.Month.ToString());
            day = Convert.ToInt16(DateTime.Now.Day.ToString());
            
            hour = Convert.ToInt16(DateTime.Now.Hour.ToString());
            minute = Convert.ToInt16(DateTime.Now.Minute.ToString());
            second = Convert.ToInt16(DateTime.Now.Second.ToString());
            DayofWeek = DateTime.Now.DayOfWeek.ToString();
            
            if (DayofWeek == "Sunday")
            {
                DW = 0;
                plc.SetClockData(year, month, day, DW, hour, minute, second);
            }                
            
            if (DayofWeek == "Monday")
            {
                DW = 1;
                plc.SetClockData(year, month, day, DW, hour, minute, second);
                
            }
            if (DayofWeek == "Tuesday")
            {
                DW = 2;
                plc.SetClockData(year, month, day, DW, hour, minute, second);
            }
            if (DayofWeek == "Wednesday")
            {
                DW = 3;
                plc.SetClockData(year, month, day, DW, hour, minute, second);
            }
            if (DayofWeek == "Thursday") 
            {
                DW = 4;
                plc.SetClockData(year, month, day, DW, hour, minute, second);
            }
            if (DayofWeek == "Friday") 
            {
                DW = 5;
                plc.SetClockData(year, month, day, DW, hour, minute, second);
            }
            if (DayofWeek == "Saturday") 
            {
                DW = 6;
                plc.SetClockData(year, month, day, DW, hour, minute, second);
            }
           

            plc.GetDevice("M0", out readdata);
           
            if (readdata == 0)
            {
                plc.SetDevice("M0", 1);
            
            }    
            else plc.SetDevice("M0", 0);

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            int readdata;
            plc.GetDevice("M0", out readdata);
            if (readdata == 0)
            {
                BTN_CONNECTION.BackColor = Color.Red;
                

            }
            else
            {
                BTN_CONNECTION.BackColor = Color.LimeGreen;
         
            }
            readdata_plc();
        }
        private string[] Addres_counter = { "D128","D130", "D132", "D134", "D136", "D138", "D140","D142", "D144", "D146", "D148",
                                     "D150","D152","D154","D156","D158","D160","D162","D164","D166","D168","D170"};
        

        private string[] Adress_pro = { "Y26", "Y27", "Y30", "Y31", "Y32", "Y33", "Y34", "Y35", "Y36", "Y37", "Y40", "Y41", "Y42",
                                        "Y43", "Y44", "Y45", "Y46", "Y47", "Y50", "Y51", "Y52", "Y53"};
        private int[] valuecounter = new int[22];
        private int[] valuetimer = new int[22];
        private int[] statepro = new int[22];
        private int[] statemaint = new int[22];
        private void readdata_plc()
        {

            var listDownTimeToAdd = new List<MachineState>();
            for (int i = 0; i < Addres_counter.Length; i++)
            {
                
                    plc.GetDevice(Addres_counter[i], out valuecounter[i] );
                    plc.GetDevice(Addres_timer[i], out valuetimer[i]);
                    plc.GetDevice(Adress_pro[i], out statepro[i]);
                    plc.GetDevice(listdiachi[i], out statemaint[i]);
                //downtime 
                var mc = lastestMachineState.FirstOrDefault(x => x.AddressTimer.ToLower() == Addres_timer[i].ToLower());
                var mainState = statemaint[i];
                var proState = statepro[i];
                var downtime = valuetimer[i];
               
                if (mainState == 1)
                {
                    mc.State = 0;
                    Console.WriteLine("Address: " + mc.AddressTimer);
                    Console.WriteLine("mainState: " + mainState);
                    Console.WriteLine("downtime: " + mc.Downtime);
                    mc.Downtime = valuetimer[i];

                } else if ( mc.Downtime > 0)
                {
                    Console.WriteLine("Address: " + mc.AddressTimer);
                    Console.WriteLine("mainState: " + mainState);
                    Console.WriteLine("downtime: " + mc.Downtime);
                    mc.State = 1;
                    var now = DateTime.Now;
                    var dateStart = new DateTime(now.Ticks - downtime);
                    listDownTimeToAdd.Add(new MachineState
                    {
                        Downtime = mc.Downtime,
                        Name = mc.Name,
                        FlagDown = true,
                        Address = mc.Address,
                        AddressTimer = mc.AddressTimer,
                        Date = DateTime.Now,
                        DateStart = dateStart,
                        State = 1
                    });
                    mc.Downtime = 0;
                }
                


            }
            if(listDownTimeToAdd.Count > 0)
            {
                Console.WriteLine("Record" + listDownTimeToAdd.Count);
               // Task.Run(() =>
               //{
                    using (var db = new MyDbContext())
                    {
                        db.MachineCollection.InsertBulk(listDownTimeToAdd);
                    }
                //});
            }
            if (statemaint[0] == 1) BTN_MB0.BackColor = Color.Orange; else BTN_MB0.BackColor = Color.White;
            if (statemaint[1] == 1) BTN_MB1.BackColor = Color.Orange; else BTN_MB1.BackColor = Color.White;
            if (statemaint[2] == 1) BTN_MB2.BackColor = Color.Orange; else BTN_MB2.BackColor = Color.White;
            if (statemaint[3] == 1) BTN_MB3.BackColor = Color.Orange; else BTN_MB3.BackColor = Color.White;
            if (statemaint[4] == 1) BTN_MB4.BackColor = Color.Orange; else BTN_MB4.BackColor = Color.White;
            if (statemaint[5] == 1) BTN_MB9.BackColor = Color.Orange; else BTN_MB9.BackColor = Color.White;
            if (statemaint[6] == 1) BTN_MK2.BackColor = Color.Orange; else BTN_MK2.BackColor = Color.White;
            if (statemaint[7] == 1) BTN_MK3.BackColor = Color.Orange; else BTN_MK3.BackColor = Color.White;
            if (statemaint[8] == 1) BTN_MK4.BackColor = Color.Orange; else BTN_MK4.BackColor = Color.White;
            if (statemaint[9] == 1) BTN_MK5.BackColor = Color.Orange; else BTN_MK5.BackColor = Color.White;
            if (statemaint[10] == 1) BTN_MREG.BackColor = Color.Orange; else BTN_MREG.BackColor = Color.White;
            if (statemaint[11] == 1) BTN_M2F1.BackColor = Color.Orange; else BTN_M2F1.BackColor = Color.White;
            if (statemaint[12] == 1) BTN_M2F2.BackColor = Color.Orange; else BTN_M2F2.BackColor = Color.White;
            if (statemaint[13] == 1) BTN_M2F3.BackColor = Color.Orange; else BTN_M2F3.BackColor = Color.White;
            if (statemaint[14] == 1) BTN_M2F4.BackColor = Color.Orange; else BTN_M2F4.BackColor = Color.White;
            if (statemaint[15] == 1) BTN_M2F5.BackColor = Color.Orange; else BTN_M2F5.BackColor = Color.White;
            if (statemaint[16] == 1) BTN_M2F6.BackColor = Color.Orange; else BTN_M2F6.BackColor = Color.White;
            if (statemaint[17] == 1) BTN_M2F7.BackColor = Color.Orange; else BTN_M2F7.BackColor = Color.White;
            if (statemaint[18] == 1) BTN_M2F8.BackColor = Color.Orange; else BTN_M2F8.BackColor = Color.White;
            if (statemaint[19] == 1) BTN_M2F9.BackColor = Color.Orange; else BTN_M2F9.BackColor = Color.White;
            if (statemaint[20] == 1) BTN_M2F10.BackColor = Color.Orange; else BTN_M2F10.BackColor = Color.White;
            if (statemaint[21] == 1) BTN_MKAB.BackColor = Color.Orange; else BTN_MKAB.BackColor = Color.White;


            if (statepro[0] == 1) BTN_PB0.BackColor = Color.LawnGreen; else BTN_PB0.BackColor = Color.White;
            if (statepro[1] == 1) BTN_PB1.BackColor = Color.LawnGreen; else BTN_PB1.BackColor = Color.White;
            if (statepro[2] == 1) BTN_PB2.BackColor = Color.LawnGreen; else BTN_PB2.BackColor = Color.White;
            if (statepro[3] == 1) BTN_PB3.BackColor = Color.LawnGreen; else BTN_PB3.BackColor = Color.White;
            if (statepro[4] == 1) BTN_PB4.BackColor = Color.LawnGreen; else BTN_PB4.BackColor = Color.White;
            if (statepro[5] == 1) BTN_PB9.BackColor = Color.LawnGreen; else BTN_PB9.BackColor = Color.White;
            if (statepro[6] == 1) BTN_PK2.BackColor = Color.LawnGreen; else BTN_PK2.BackColor = Color.White;
            if (statepro[7] == 1) BTN_PK3.BackColor = Color.LawnGreen; else BTN_PK3.BackColor = Color.White;
            if (statepro[8] == 1) BTN_PK4.BackColor = Color.LawnGreen; else BTN_PK4.BackColor = Color.White;
            if (statepro[9] == 1) BTN_PK5.BackColor = Color.LawnGreen; else BTN_PK5.BackColor = Color.White;
            if (statepro[10] == 1) BTN_PREG.BackColor = Color.LawnGreen; else BTN_PREG.BackColor = Color.White;
            if (statepro[11] == 1) BTN_P2F1.BackColor = Color.LawnGreen; else BTN_P2F1.BackColor = Color.White;
            if (statepro[12] == 1) BTN_P2F2.BackColor = Color.LawnGreen; else BTN_P2F2.BackColor = Color.White;
            if (statepro[13] == 1) BTN_P2F3.BackColor = Color.LawnGreen; else BTN_P2F3.BackColor = Color.White;
            if (statepro[14] == 1) BTN_P2F4.BackColor = Color.LawnGreen; else BTN_P2F4.BackColor = Color.White;
            if (statepro[15] == 1) BTN_P2F5.BackColor = Color.LawnGreen; else BTN_P2F5.BackColor = Color.White;
            if (statepro[16] == 1) BTN_P2F6.BackColor = Color.LawnGreen; else BTN_P2F6.BackColor = Color.White;
            if (statepro[17] == 1) BTN_P2F7.BackColor = Color.LawnGreen; else BTN_P2F7.BackColor = Color.White;
            if (statepro[18] == 1) BTN_P2F8.BackColor = Color.LawnGreen; else BTN_P2F8.BackColor = Color.White;
            if (statepro[19] == 1) BTN_P2F9.BackColor = Color.LawnGreen; else BTN_P2F9.BackColor = Color.White;
            if (statepro[20] == 1) BTN_P2F10.BackColor = Color.LawnGreen; else BTN_P2F10.BackColor = Color.White;
            if (statepro[21] == 1) BTN_PKAB.BackColor = Color.LawnGreen; else BTN_PKAB.BackColor = Color.White;

            TXT_CB0.Text = valuecounter[0].ToString();
            TXT_CB1.Text = valuecounter[1].ToString();
            TXT_CB2.Text = valuecounter[2].ToString();
            TXT_CB3.Text = valuecounter[3].ToString();
            TXT_CB4.Text = valuecounter[4].ToString();
            TXT_CB9.Text = valuecounter[5].ToString();
            TXT_CK2.Text = valuecounter[6].ToString();
            TXT_CK3.Text = valuecounter[7].ToString();
            TXT_CK4.Text = valuecounter[8].ToString();
            TXT_CK5.Text = valuecounter[9].ToString();
            TXT_CREG.Text = valuecounter[10].ToString();
            TXT_C2F1.Text = valuecounter[11].ToString();
            TXT_C2F2.Text = valuecounter[12].ToString();
            TXT_C2F3.Text = valuecounter[13].ToString();
            TXT_C2F4.Text = valuecounter[14].ToString();
            TXT_C2F5.Text = valuecounter[15].ToString();
            TXT_C2F6.Text = valuecounter[16].ToString();
            TXT_C2F7.Text = valuecounter[17].ToString();
            TXT_C2F8.Text = valuecounter[18].ToString();
            TXT_C2F9.Text = valuecounter[19].ToString();
            TXT_C2F10.Text = valuecounter[20].ToString();
            TXT_CKAB.Text = valuecounter[21].ToString();


            TXT_DB0.Text = (valuetimer[0]/10).ToString();
            TXT_DB1.Text = (valuetimer[1] / 10).ToString();
            TXT_DB2.Text = (valuetimer[2] / 10).ToString();
            TXT_DB3.Text = (valuetimer[3] / 10).ToString();
            TXT_DB4.Text = (valuetimer[4] / 10).ToString();
            TXT_DB9.Text = (valuetimer[5] / 10).ToString();
            TXT_DK2.Text = (valuetimer[6] / 10).ToString();
            TXT_DK3.Text = (valuetimer[7] / 10).ToString();
            TXT_DK4.Text = (valuetimer[8] / 10).ToString();
            TXT_DK5.Text = (valuetimer[9] / 10).ToString();
            TXT_DREG.Text = (valuetimer[10] / 10).ToString();
            TXT_D2F1.Text = (valuetimer[11] / 10).ToString();
            TXT_D2F2.Text = (valuetimer[12] / 10).ToString();
            TXT_D2F3.Text = (valuetimer[13] / 10).ToString();
            TXT_D2F4.Text = (valuetimer[14] / 10).ToString();
            TXT_D2F5.Text = (valuetimer[15] / 10).ToString();
            TXT_D2F6.Text = (valuetimer[16] / 10).ToString();
            TXT_D2F7.Text = (valuetimer[17] / 10).ToString();
            TXT_D2F8.Text = (valuetimer[18] / 10).ToString();
            TXT_D2F9.Text = (valuetimer[19] / 10).ToString();
            TXT_D2F10.Text = (valuetimer[20] / 10).ToString();
            TXT_DKAB.Text = (valuetimer[21] / 10).ToString();
            

        }


        private void Form1_CLosed(object sender, FormClosedEventArgs e)
        {

            if (MessageBox.Show("YOU WANT TO CLOSE THIS APPLICATION?", "NOTIFITION", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.OK)

            {
                plc.SetDevice("M0", 0);
                Application.Exit();
       
            }
            
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //pictureBox2.Visible = false;
            //pictureBox1.Visible = true;
           
            plc.SetDevice("M1", 1);
            plc.SetDevice("M1", 0);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            //pictureBox1.Visible = false;
            //pictureBox2.Visible = true;
            plc.SetDevice("M2", 1);
            plc.SetDevice("M2", 0);
        }
    }
    
    
}
