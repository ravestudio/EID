using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EIDClient.Core.Entities
{
    public class Position : Entity<int>
    {
        public string Firm { get; set; }
        public string SecurityName { get; set; }
        public string Code { get; set; }
        public string Account { get; set; }
        public string Client { get; set; }
        public string Type { get; set; }

        public int IncomingBalance { get; set; }
        public int IncomingLimit { get; set; }

        public int CurrentBalance { get; set; }
        public int CurrentLimit { get; set; }

        public int Blocked { get; set; }
        public int BlockedForPurchase { get; set; }

        public int Total { get; set; }
        public int Available { get; set; }

        public int Balance { get; set; }
        public decimal PurchasePrice { get; set; }

        public override void ReadData(JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Firm = jsonObj["Firm"].GetString();
            this.SecurityName = jsonObj["SecurityName"].GetString();
            this.Code = jsonObj["Code"].GetString();
            this.Account = jsonObj["Account"].GetString();
            this.Client = jsonObj["Client"].GetString();
            this.Type = jsonObj["Type"].GetString();

            this.IncomingBalance = (int)jsonObj["IncomingBalance"].GetNumber();
            this.IncomingLimit = (int)jsonObj["IncomingLimit"].GetNumber();

            this.CurrentBalance = (int)jsonObj["CurrentBalance"].GetNumber();
            this.CurrentLimit = (int)jsonObj["CurrentLimit"].GetNumber();

            this.Blocked = (int)jsonObj["Blocked"].GetNumber();
            this.BlockedForPurchase = (int)jsonObj["BlockedForPurchase"].GetNumber();

            this.Total = (int)jsonObj["Total"].GetNumber();
            this.Available = (int)jsonObj["Available"].GetNumber();

            this.Balance = (int)jsonObj["Balance"].GetNumber();
            this.PurchasePrice = (decimal)jsonObj["PurchasePrice"].GetNumber();
        }
    }
}
