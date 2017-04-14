using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Entities
{
    public class Order : Entity<int>
    {
        public decimal Number { get; set; }
        public string Code { get; set; }
        public string Time { get; set; }
        public string Operation { get; set; }
        public string Account { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Volume { get; set; }
        public string State { get; set; }
        public string Class { get; set; }
        public string Comment { get; set; }

        public List<KeyValuePair<string, string>> ConverToKeyValue()
        {
            List<KeyValuePair<string, string>> keyValueData = new List<KeyValuePair<string, string>>();

            keyValueData.Add(new KeyValuePair<string, string>("Id", Id.ToString()));
            keyValueData.Add(new KeyValuePair<string, string>("Number", Number.ToString()));
            keyValueData.Add(new KeyValuePair<string, string>("Code", Code));
            keyValueData.Add(new KeyValuePair<string, string>("Operation", Operation));
            keyValueData.Add(new KeyValuePair<string, string>("Account", Account));
            keyValueData.Add(new KeyValuePair<string, string>("Price", Price.ToString()));
            keyValueData.Add(new KeyValuePair<string, string>("Count", Count.ToString()));
            keyValueData.Add(new KeyValuePair<string, string>("Volume", Volume.ToString()));
            keyValueData.Add(new KeyValuePair<string, string>("State", State));
            keyValueData.Add(new KeyValuePair<string, string>("Class", Class));
            keyValueData.Add(new KeyValuePair<string, string>("Comment", Comment));

            return keyValueData;
        }
    }

}
