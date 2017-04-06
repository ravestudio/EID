using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Common.ISS
{
    public class Candle
    {
        public Candle()
        {

        }

        public Candle(EIDService.Common.Entities.Candle candle)
        {
            this.Code = candle.Code;

            int year = int.Parse(candle.CandleDate.Substring(0,4));
            int moth = int.Parse(candle.CandleDate.Substring(4,2));
            int day = int.Parse(candle.CandleDate.Substring(6,2));

            int hour = int.Parse(candle.CandleTime.Substring(0,2));
            int minute = int.Parse(candle.CandleTime.Substring(2,2));

            this.begin = new DateTime(year, moth, day, hour, minute, 0);
            this.open = candle.OpenPrice;
            this.close = candle.ClosePrice;
            this.high = candle.MaxPrice;
        }

        public string Code { get; set; }
        public DateTime begin { get; set; }
        public decimal open { get; set; }
        public decimal close { get; set; }
        public decimal high { get; set; }
    }
}
