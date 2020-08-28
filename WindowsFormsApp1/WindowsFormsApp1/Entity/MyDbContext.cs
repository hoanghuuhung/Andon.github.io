using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AddOn.Entity
{
   public class MyDbContext : IDisposable
    {
        public ILiteCollection<MachineState> MachineCollection { get; set; }
        private ILiteDatabase _db;
        public MyDbContext()
        {
            _db = new LiteDatabase(@"AddOn.db");


            MachineCollection = _db.GetCollection<MachineState>("Machines");


        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
