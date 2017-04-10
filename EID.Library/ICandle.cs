using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EID.Library
{
    public interface ICandle
    {
        string Code { get; set; }
        DateTime begin { get; set; }
        decimal open { get; set; }
        decimal close { get; set; }
        decimal high { get; set; }
    }
}
