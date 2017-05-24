using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EIDClient.Core.Entities
{
    public class Candle : Entity<int>, ICandle
    {
        public DateTime begin { get; set; }
        public decimal open { get; set; }
        public decimal close { get; set; }
        public decimal high { get; set; }
        public string Code { get; set; }
        public decimal low { get; set; }
        public decimal value { get; set; }
        public decimal volume { get; set; }

        public override void ReadData(JsonObject jsonObj)
        {
            this.Code = jsonObj["Code"].GetString();

            this.begin = DateTime.Parse(jsonObj["begin"].GetString());
            this.open = (decimal)jsonObj["open"].GetNumber();
            this.close = (decimal)jsonObj["close"].GetNumber();
            this.high = (decimal)jsonObj["high"].GetNumber();
            this.low = (decimal)jsonObj["low"].GetNumber();
            this.value = (decimal)jsonObj["value"].GetNumber();
            this.volume = (decimal)jsonObj["volume"].GetNumber();
        }
    }
}
