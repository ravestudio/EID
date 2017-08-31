using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class EIDProcess : Entity<int>
    {
        public EIDProcessStatus Status { get; set; }
        public EIDProcessType Type { get; set; }

        public string Data { get; set; }
    }

    public enum EIDProcessType : int
    {
        ClosePosition = 0
    }

    public enum EIDProcessStatus : int
    {
        Created = 0,
        KillStop = 1,
        KillStopCompleted = 2,
        ClosePosition = 3,
        ClosepositionCompleted = 4,
        Completed = 5
    }
}
