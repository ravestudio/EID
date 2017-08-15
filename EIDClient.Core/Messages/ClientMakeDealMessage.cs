using EIDClient.Core.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Messages
{
    public class ClientMakeDealMessage
    {
        public string Sec { get; set; }
        public string Operation { get; set; }
    }
}
