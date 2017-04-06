using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Entities
{
    public class SecurityData : Entity<string>
    {
        public SecurityInfo SecurityInfo { get; set; }
        public MarketData MarketData { get; set; }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.MarketData = new MarketData();
            this.SecurityInfo = new SecurityInfo();

            var md = jsonObj["MarketData"].GetObject();
            var si = jsonObj["SecurityInfo"].GetObject();

            this.MarketData.LCURRENTPRICE = (decimal)md["LCURRENTPRICE"].GetNumber();
            this.MarketData.OPENPERIODPRICE = (decimal)md["OPENPERIODPRICE"].GetNumber();

            this.SecurityInfo.PREVLEGALCLOSEPRICE = (decimal)si["PREVLEGALCLOSEPRICE"].GetNumber();

        }
    }
}
