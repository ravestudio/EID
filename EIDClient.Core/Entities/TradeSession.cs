using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Entities
{
    public class TradeSession : Entity<int>
    {
        public DateTime Date { get; set; }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Date = DateTime.Parse(jsonObj["Date"].GetString());
        }
    }
}
