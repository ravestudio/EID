using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Entities
{
    public class Financial : Entity<int>
    {

        public int Year { get; set; }
        public int Period { get; set; }
        /// <summary>
        /// Выручка
        /// </summary>
        public decimal Revenue { get; set; }

        /// <summary>
        /// операционные расходы
        /// </summary>
        public decimal OperatingExpenses { get; set; }

        /// <summary>
        /// Операционная прибыль
        /// </summary>
        public decimal OperatingIncome { get; set; }

        /// <summary>
        /// Чистая прибыль
        /// </summary>
        public decimal NetIncome { get; set; }

        /// <summary>
        /// Оборотные активы
        /// </summary>
        public decimal CurrentAssets { get; set; }

        /// <summary>
        /// Внеоборотные активы
        /// </summary>
        public decimal FixedAssets { get; set; }

        /// <summary>
        /// Итого активы
        /// </summary>
        public decimal TotalAssets { get; set; }

        /// <summary>
        /// Краткосрочные обязательства
        /// </summary>
        public decimal CurrentLiabilities { get; set; }

        /// <summary>
        /// Долгосрочные обязательства
        /// </summary>
        public decimal LongTermLiabilities { get; set; }

        /// <summary>
        /// Итого обязательства
        /// </summary>
        public decimal TotalLiabilities { get; set; }

        /// <summary>
        /// Капитал
        /// </summary>
        public decimal Equity { get; set; }

        /// <summary>
        /// Итого капитал и обязательства
        /// </summary>
        public decimal LiabilitiesAndEquity { get; set; }

        public decimal FlowOperatingActivities { get; set; }

        public decimal ChangesInAssets { get; set; }

        public decimal Capex { get; set; }

        public decimal Amortization { get; set; }

        public decimal FlowOperatingTaxesPaid { get; set; }

        public decimal FlowInvestingActivities { get; set; }

        public decimal FlowFinancingActivities { get; set; }

        /// <summary>
        /// Средства направленные на продажу (выкуп) собственых акций
        /// </summary>
        public decimal StockIssuance { get; set; }

        /// <summary>
        /// Дивиденды, выплаченные по акциям Компании
        /// </summary>
        public decimal DividendsPaid { get; set; }

        /// <summary>
        /// Прибыль на акцию
        /// </summary>
        public decimal EarningsPerShare { get; set; }

        /// <summary>
        /// Цена за акцию
        /// </summary>
        public decimal Price { get; set; }

        public decimal EBITDA { get; set; }
        public decimal FCF { get; set; }

        /// <summary>
        /// P/E Ratio 
        /// </summary>
        public decimal PriceEarningsRatio { get; set; }

        public decimal ReturnOnEquity { get; set; }


        public int EmitentId { get; set; }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["Id"].GetNumber();
            this.Year = (int)jsonObj["Year"].GetNumber();
            this.Period = (int)jsonObj["Period"].GetNumber();

            this.Revenue = (decimal)jsonObj["Revenue"].GetNumber();
            this.OperatingExpenses = (decimal)jsonObj["OperatingExpenses"].GetNumber();
            this.OperatingIncome = (decimal)jsonObj["OperatingIncome"].GetNumber();
            this.NetIncome = (decimal)jsonObj["NetIncome"].GetNumber();

            this.CurrentAssets = (decimal)jsonObj["CurrentAssets"].GetNumber();
            this.FixedAssets = (decimal)jsonObj["FixedAssets"].GetNumber();
            this.TotalAssets = (decimal)jsonObj["TotalAssets"].GetNumber();
            
            this.CurrentLiabilities = (decimal)jsonObj["CurrentLiabilities"].GetNumber();
            this.LongTermLiabilities = (decimal)jsonObj["LongTermLiabilities"].GetNumber();
            this.TotalLiabilities = (decimal)jsonObj["TotalLiabilities"].GetNumber();
            this.Equity = (decimal)jsonObj["Equity"].GetNumber();
            this.LiabilitiesAndEquity = (decimal)jsonObj["LiabilitiesAndEquity"].GetNumber();

            this.FlowOperatingActivities = (decimal)jsonObj["FlowOperatingActivities"].GetNumber();
            this.ChangesInAssets = (decimal)jsonObj["ChangesInAssets"].GetNumber();
            this.Amortization = (decimal)jsonObj["Amortization"].GetNumber();
            this.Capex = (decimal)jsonObj["Capex"].GetNumber();
            this.FlowOperatingTaxesPaid = (decimal)jsonObj["FlowOperatingTaxesPaid"].GetNumber();

            this.FlowInvestingActivities = (decimal)jsonObj["FlowInvestingActivities"].GetNumber();
            this.FlowFinancingActivities = (decimal)jsonObj["FlowFinancingActivities"].GetNumber();

            this.StockIssuance = (decimal)jsonObj["StockIssuance"].GetNumber();
            this.DividendsPaid = (decimal)jsonObj["DividendsPaid"].GetNumber();

            this.EarningsPerShare = (decimal)jsonObj["EarningsPerShare"].GetNumber();
            this.Price = (decimal)jsonObj["Price"].GetNumber();

            this.EBITDA = (decimal)jsonObj["EBITDA"].GetNumber();
            this.FCF = (decimal)jsonObj["FCF"].GetNumber();

            this.PriceEarningsRatio = (decimal)jsonObj["PriceEarningsRatio"].GetNumber();
            this.ReturnOnEquity = (decimal)jsonObj["ReturnOnEquity"].GetNumber();

        }

        private string GetString(decimal value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public List<KeyValuePair<string, string>> ConverToKeyValue()
        {
            List<KeyValuePair<string, string>> keyValueData = new List<KeyValuePair<string, string>>();

            keyValueData.Add(new KeyValuePair<string, string>("Id", Id.ToString()));
            keyValueData.Add(new KeyValuePair<string, string>("Year", Year.ToString()));
            keyValueData.Add(new KeyValuePair<string, string>("Period", Period.ToString()));

            keyValueData.Add(new KeyValuePair<string, string>("Revenue", GetString(Revenue)));
            keyValueData.Add(new KeyValuePair<string, string>("OperatingIncome", GetString(OperatingIncome)));
            keyValueData.Add(new KeyValuePair<string, string>("NetIncome", GetString(NetIncome)));

            keyValueData.Add(new KeyValuePair<string, string>("CurrentAssets", GetString(CurrentAssets)));
            keyValueData.Add(new KeyValuePair<string, string>("FixedAssets", GetString(FixedAssets)));
            keyValueData.Add(new KeyValuePair<string, string>("CurrentLiabilities", GetString(CurrentLiabilities)));
            keyValueData.Add(new KeyValuePair<string, string>("LongTermLiabilities", GetString(LongTermLiabilities)));

            keyValueData.Add(new KeyValuePair<string, string>("FlowOperatingActivities", GetString(FlowOperatingActivities)));
            keyValueData.Add(new KeyValuePair<string, string>("ChangesInAssets", GetString(ChangesInAssets)));
            keyValueData.Add(new KeyValuePair<string, string>("Capex", GetString(Capex)));
            keyValueData.Add(new KeyValuePair<string, string>("Amortization", GetString(Amortization)));
            keyValueData.Add(new KeyValuePair<string, string>("FlowOperatingTaxesPaid", GetString(FlowOperatingTaxesPaid)));

            keyValueData.Add(new KeyValuePair<string, string>("FlowInvestingActivities", GetString(FlowInvestingActivities)));
            keyValueData.Add(new KeyValuePair<string, string>("FlowFinancingActivities", GetString(FlowFinancingActivities)));

            keyValueData.Add(new KeyValuePair<string, string>("StockIssuance", GetString(StockIssuance)));
            keyValueData.Add(new KeyValuePair<string, string>("DividendsPaid", GetString(DividendsPaid)));
            keyValueData.Add(new KeyValuePair<string, string>("EarningsPerShare", GetString(EarningsPerShare)));

            keyValueData.Add(new KeyValuePair<string, string>("Price", GetString(Price)));

            if (EmitentId != 0)
            {
                keyValueData.Add(new KeyValuePair<string, string>("Emitent.Id", EmitentId.ToString()));
            }
            return keyValueData;

        }
    }
}
