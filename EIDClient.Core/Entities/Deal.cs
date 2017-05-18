using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EIDClient.Core.Entities
{
    public class Deal : Entity<int>
    {
        public decimal Number { get; set; }
        public decimal OrderNumber { get; set; }
        public string Code { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Operation { get; set; }
        public string Account { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Volume { get; set; }
        public string Class { get; set; }

        public override void ReadData(JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Number = (decimal)jsonObj["Number"].GetNumber();
            this.OrderNumber = (decimal)jsonObj["OrderNumber"].GetNumber();
            this.Code = jsonObj["Code"].GetString();
            this.Time = jsonObj["Time"].GetString();
            this.Date = jsonObj["Date"].GetString();
            this.Operation = jsonObj["Operation"].GetString();
            this.Account = jsonObj["Account"].GetString();

            this.Price = (decimal)jsonObj["Price"].GetNumber();
            this.Count = (int)jsonObj["Count"].GetNumber();
            this.Volume = (decimal)jsonObj["Volume"].GetNumber();

            this.Class = jsonObj["Class"].GetString();            
        }
    }
}
