using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Position : Entity<int>
    {

        public override string Key {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Firm { get; set; }
        public string SecurityName { get; set; }
        public string Code { get; set; }
        public string Account { get; set; }
        public string Client { get; set; }
        public string Type { get; set; }

        public PosTypeEnum PosType { get; set; }

        public int IncomingBalance { get; set; }
        public int IncomingLimit { get; set; }

        public int CurrentBalance { get; set; }
        public int CurrentLimit { get; set; }

        public int Blocked { get; set; }
        public int BlockedForPurchase { get; set; }

        public int Total { get; set; }
        public int Available { get; set; }

        public int Balance { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
