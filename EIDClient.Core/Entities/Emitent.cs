using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Entities
{
    public class Emitent : Entity<int>
    {
        public string Name { get; set; }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.Name = jsonObj["Name"].GetString();

            base.ReadData(jsonObj);
        }
    }
}
