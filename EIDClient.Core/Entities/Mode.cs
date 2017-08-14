using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Entities
{
    public class Mode : Entity<int>
    {
        public string Firm { get; set; }
        public string Account { get; set; }
        public string Class { get; set; }
        public string Client { get; set; }

        public bool Active { get; set; }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();

            this.Firm = jsonObj["Firm"].GetString();
            this.Account = jsonObj["Account"].GetString();
            this.Class = jsonObj["Class"].GetString();
            this.Client = jsonObj["Client"].GetString();

        }
    }
}
