using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
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
        /// Операционная прибыль
        /// </summary>
        public decimal OperatingIncome { get; set; }

        /// <summary>
        /// операционные расходы
        /// </summary>
        public decimal OperatingExpenses
        {
            get { return this.Revenue - this.OperatingIncome; }
        }

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
        public decimal TotalAssets
        {
            get { return this.CurrentAssets + this.FixedAssets; }
        }

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
        public decimal TotalLiabilities
        {
            get { return this.CurrentLiabilities + this.LongTermLiabilities; }
        }

        /// <summary>
        /// Капитал
        /// </summary>
        public decimal Equity
        {
            get { return this.TotalAssets - this.TotalLiabilities; }
        }

        /// <summary>
        /// Итого капитал и обязательства
        /// </summary>
        public decimal LiabilitiesAndEquity
        {
            get { return this.Equity + this.TotalLiabilities; }
        }

        public decimal FlowOperatingActivities { get; set; }

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


        public virtual Emitent Emitent { get; set; }
    }
}
