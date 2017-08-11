using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EIDClient.Core.Entities
{
    public class Emitent : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Name = jsonObj["Name"].GetString();

            this.Description = jsonObj["Description"].ValueType != JsonValueType.Null? jsonObj["Description"].GetString():null;

            this.WebSite = jsonObj["WebSite"].ValueType != JsonValueType.Null? jsonObj["WebSite"].GetString():null;
        }

        public List<KeyValuePair<string, string>> ConverToKeyValue()
        {
            List<KeyValuePair<string, string>> keyValueData = new List<KeyValuePair<string, string>>();

            keyValueData.Add(new KeyValuePair<string, string>("Id", Id.ToString()));
            keyValueData.Add(new KeyValuePair<string, string>("Name", Name));
            keyValueData.Add(new KeyValuePair<string, string>("Description", Description));
            keyValueData.Add(new KeyValuePair<string, string>("WebSite", WebSite));

            return keyValueData;
        }
    }
}
