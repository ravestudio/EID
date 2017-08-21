using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{

    public class PosType : Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Name { get; set; }
    }

    public enum PosTypeEnum : byte
    {
        T0 = 1,
        T1 = 2,
        T2 = 3
    }
}
