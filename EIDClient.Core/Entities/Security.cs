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

        private SecurityInfo _securityInfo = null;
        private MarketData _marketData = null;

        public SecurityInfo SecurityInfo
        {
            get { return _securityInfo; }
            set {
                _securityInfo = value;
                RaisePropertyChanged("CurrentPrice");
                RaisePropertyChanged("PriceChangePrcnt");
            }
        }

        public MarketData MarketData
        {
            get { return _marketData; }
            set {
                _marketData = value;
                RaisePropertyChanged("CurrentPrice");
                RaisePropertyChanged("PriceChangePrcnt");
            }
        }

        public decimal CurrentPrice
        {
            get
            {
                decimal res = 0m;

                if (MarketData != null && SecurityInfo != null)
                {
                    res = MarketData.LCURRENTPRICE != 0 ? MarketData.LCURRENTPRICE : SecurityInfo.PREVLEGALCLOSEPRICE;
                }

                return res;
            }
        }

        public decimal PriceChangePrcnt
        {
            get
            {
                decimal res = 0m;

                if (MarketData != null && MarketData.OPENPERIODPRICE > 0)
                {
                    res = Math.Round((CurrentPrice - MarketData.OPENPERIODPRICE) / (MarketData.OPENPERIODPRICE / 100), 2);
                }

                return res;
            }
        }
    }
}
