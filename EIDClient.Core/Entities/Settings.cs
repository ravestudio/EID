using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Entities
{
    public class Settings : Entity<int>
    {
        public string Mode { get; set; }
        public DateTime TestDateTime { get; set; }


        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Mode = jsonObj["Mode"].GetString();
            this.TestDateTime = DateTime.Parse(jsonObj["TestDateTime"].GetString());
        }
    }
}
