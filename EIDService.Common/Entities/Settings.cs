using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Settings: Entity<int>
    {
        public string Mode { get; set; }
        public DateTime TestDateTime { get; set; }

        public ModeType ModeType
        {
            get
            {
                return Mode == "Test" ? ModeType.Test : ModeType.Work;
            }

            set
            {
                this.Mode = value == ModeType.Test ? "Test" : "Work";
            }
        }
    }
}
