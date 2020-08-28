using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn.Entity
{
    public class MachineState
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int State { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateStart { get; set; }
        public double Downtime { get; set; }
        public bool FlagDown { get; set; }
              ///////////////////////
        public string AddressTimer { get; set; }
        public double value { get; set; }
        public int Counter { get; set; }
    }
}
