using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common
{
    public class CandleToISS
    {
        public EID.Library.ISS.Candle Convert(EIDService.Common.Entities.Candle c)
        {
            EID.Library.ISS.Candle candle = new EID.Library.ISS.Candle();

            candle.Code = c.Code;

            int year = int.Parse(c.CandleDate.Substring(0, 4));
            int moth = int.Parse(c.CandleDate.Substring(4, 2));
            int day = int.Parse(c.CandleDate.Substring(6, 2));

            int hour = int.Parse(c.CandleTime.Substring(0, 2));
            int minute = int.Parse(c.CandleTime.Substring(2, 2));

            candle.begin = new DateTime(year, moth, day, hour, minute, 0);
            candle.open = c.OpenPrice;
            candle.close = c.ClosePrice;
            candle.high = c.MaxPrice;
            candle.low = c.MinPrice;
            candle.value = c.Value;
            candle.volume = c.Volume;

            return candle;
        }
    }
}
