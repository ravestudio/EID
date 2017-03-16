using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EIDClient.Core.ISS
{
    public class MicexISSClient
    {
        private WebApiClient _apiClient = null;
        public MicexISSClient(WebApiClient apiClient)
        {
            _apiClient = apiClient;

        }

        public Task<IList<Candle>> GetCandles(string security, DateTime from, int interval)
        {
            string url = string.Format("http://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities/{0}/candles.xml?from={1:yyyy-MM-dd}&interval={2}", security, from, interval);

            TaskCompletionSource<IList<Candle>> TCS = new TaskCompletionSource<IList<Candle>>();

            _apiClient.GetData(url).ContinueWith(t =>
            {
                IList<Candle> candlelist = new List<Candle>();
                string data = t.Result;

                XElement candles = GetDataBlock(XDocument.Parse(data), "candles");
                XElement rows = GetRows(candles);

                foreach(XElement el in rows.Elements())
                {
                    Candle candle = new Candle()
                    {
                        begin = DateTime.Parse(GetAttribute(el, "begin"), CultureInfo.InvariantCulture),
                        open = decimal.Parse(GetAttribute(el, "open"), CultureInfo.InvariantCulture),
                        close = decimal.Parse(GetAttribute(el, "close"), CultureInfo.InvariantCulture),
                        high = decimal.Parse(GetAttribute(el, "high"), CultureInfo.InvariantCulture)
                    };
                    candlelist.Add(candle);        
                }

                TCS.SetResult(candlelist);
            });

            return TCS.Task;
        }

        public XElement GetDataBlock(XDocument xml, string block_id)
        {
            XElement block = null;

            var elements = xml.Element("document").Elements();

            block = elements.SingleOrDefault(e => e.Attribute("id").Value == block_id);

            return block;
        }

        public XElement GetRows(XElement element)
        {
            return element.Elements().SingleOrDefault(e => e.Name == "rows");
        }

        public string GetAttribute(XElement element, string attr)
        {
            return element.Attributes().Single(a => a.Name.ToString().ToUpper() == attr.ToUpper()).Value;
        }
    }
}
