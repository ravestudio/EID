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
        /// Себестоимость проданных товаров
        /// </summary>
        public decimal Expenses { get; set; }

        /// <summary>
        /// Валовая прибыль
        /// </summary>
        public decimal GrossProfit { get; set; }

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
        /// Капитал
        /// </summary>
        public decimal Equity { get; set; }

        /// <summary>
        /// Краткосрочные обязательства
        /// </summary>
        public decimal CurrentLiabilities { get; set; }

        /// <summary>
        /// Долгосрочные обязательства
        /// </summary>
        public decimal LongTermLiabilities { get; set; }

        /// <summary>
        /// Итого капитал и обязательства
        /// </summary>
        public decimal LiabilitiesAndEquity { get; set; }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.Year = (int)jsonObj["Year"].GetNumber();
            this.Period = (int)jsonObj["Period"].GetNumber();

            this.Revenue = (decimal)jsonObj["Revenue"].GetNumber();
            this.OperatingExpenses = (decimal)jsonObj["Revenue"].GetNumber();
            this.Expenses = (decimal)jsonObj["Expenses"].GetNumber();
            this.GrossProfit = (decimal)jsonObj["GrossProfit"].GetNumber();
            this.OperatingIncome = (decimal)jsonObj["OperatingIncome"].GetNumber();
            this.NetIncome = (decimal)jsonObj["NetIncome"].GetNumber();

            this.CurrentAssets = (decimal)jsonObj["CurrentAssets"].GetNumber();
            this.FixedAssets = (decimal)jsonObj["FixedAssets"].GetNumber();
            this.TotalAssets = (decimal)jsonObj["TotalAssets"].GetNumber();

            this.Equity = (decimal)jsonObj["Equity"].GetNumber();
            this.CurrentLiabilities = (decimal)jsonObj["CurrentLiabilities"].GetNumber();
            this.LongTermLiabilities = (decimal)jsonObj["LongTermLiabilities"].GetNumber();
            this.LiabilitiesAndEquity = (decimal)jsonObj["LiabilitiesAndEquity"].GetNumber();



            base.ReadData(jsonObj);
        }
    }
}
