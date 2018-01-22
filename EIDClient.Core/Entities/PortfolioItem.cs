using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EIDClient.Core.Entities
{
    public class PortfolioItem : Entity<int>
    {
        public string Code { get; set; }
        public int Count { get; set; }
        public string Account { get; set; }

        public decimal Price { get; set; }

        public decimal Value
        {
            get
            {
                return this.Price * this.Count;
            }
        }

        public decimal Perc { get; set; }

        public override void ReadData(JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Code = jsonObj["Code"].GetString();
            this.Count = (int)jsonObj["Count"].GetNumber();
            this.Account = jsonObj["Account"].GetString();

        }
    }
}
