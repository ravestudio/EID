using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EIDClient.Core.Entities
{
    public class Security : Entity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public int IssueSize { get; set; }

        public override void ReadData(JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Code = jsonObj["Code"].GetString();
            this.Name = jsonObj["Name"].GetString();
            this.IssueSize = (int)jsonObj["IssueSize"].GetNumber();
        }
    }
}
