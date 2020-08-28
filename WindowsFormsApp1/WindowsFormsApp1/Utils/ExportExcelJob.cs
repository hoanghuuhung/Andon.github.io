
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AddOn.Entity;
using System.Text;

namespace AddOn.Utils
{
    public class ExportExcelJob : IJob
    {
        //public void Execute(IJobExecutionContext context)
        //{
        //    Console.WriteLine(DateTime.Now + "----exportexcel");
        //    ExportData();
        //    //export excel
        //}

        Task IJob.Execute(IJobExecutionContext context)
        {
            Console.WriteLine(DateTime.Now + "-----exportexcel");
            ExportData();
            return Task.CompletedTask;
            //throw new System.NotImplementedException();
        }

        private void ExportData()
        {
            using (var db = new MyDbContext())
            {
                // lay data downtime
                //var listDownDetails = db.MachineCollection.Find(x => x.FlagDown).ToList();
                //
                var listDownDetails = db.MachineCollection.Find(x => x.Date.Date == DateTime.Now.Date).ToList();
                //var listDowntime = listDownDetails.GroupBy(x => x.Id).Select(g => new
                //{
                //    g.FirstOrDefault().Date,
                //    Id = g.Key,
                //    g.FirstOrDefault().Address,
                //    Downtime = g.Sum(x => x.Downtime),
                //    Counter = g.ToList().Count
                //}).ToList();

                if (listDownDetails.Count > 0)
                {
                    Console.WriteLine(DateTime.Now + "-----exportexcel has data");
                    //string csv = string.Empty;
                    StringBuilder csv = new StringBuilder("");
                    // export data downtime
                    //        DataTable dt = new DataTable();
                    //        dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Date", typeof(string)),
                    //new DataColumn("Machine", typeof(string)),
                    //new DataColumn("Downtime",typeof(int)),
                    //new DataColumn("Counter",typeof(int)),
                    //new DataColumn("Address PLC",typeof(string))});
                    csv.Append("Date,Time,Machine,Downtime,Address PCL\r\n");

                    

                    foreach (var mc in listDownDetails)
                    {
                        //dt.Rows.Add(mc.Date.ToString("DD/mm/yyy") , mc.Id, mc.Downtime, mc.Downtime, mc.Address);
                        //csv += "="+mc.Date.ToString("dd/MM/yyyy")+",=" + mc.Date.ToString("HH:mm:ss") + "," + mc.Name + "," + mc.Downtime + "," + mc.Address +"\r\n";
                        csv.Append("=\""+ mc.DateStart.ToString("yyyy/MM/dd") + "\",\""+ mc.DateStart.ToString("HH:mm:ss") + "\",\"" + mc.Name + "\",\"" + mc.Downtime + "\",\"" + mc.Address + "\"\r\n");
                    }
                    
                    string timestamp =  DateTime.Now.ToString("yyyy/MM/dd");
                    string folderName = "DateExport/" + timestamp;

                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }

                    File.WriteAllText(folderName + "/DowntimeDetails_"+ timestamp + ".csv", csv.ToString());
                    
                }


            }
        }
    }
}
