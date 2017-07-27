using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot
{
    public class Signal
    {
        public string Security { get; set; }
        public string Action { get; set; }
        public DateTime DateTime {get; set;}

        public decimal Price { get; set; }
    }
}
