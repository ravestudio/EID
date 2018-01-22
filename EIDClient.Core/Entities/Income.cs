using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EIDClient.Core.Entities
{
    public class Income : Entity<int>
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }

        public override void ReadData(JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Date = DateTime.Parse(jsonObj["Date"].GetString());
            this.Value = (decimal)jsonObj["Value"].GetNumber();
            this.Comment = jsonObj["Comment"].GetString();
        }
    }
}
