﻿using System;
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

        /// <summary>
        /// depreciation and amortization
        /// </summary>
        public decimal Amortization { get; set; }

        /// <summary>
        /// Изменения в активах (NWC, Net working capital change)
        /// </summary>
        public decimal ChangesInAssets { get; set; }

        /// <summary>
        /// капитальные затраты 
        /// </summary>
        public decimal Capex { get; set; }

        /// <summary>
        /// Налог на прибыль уплаченный
        /// </summary>
        public decimal FlowOperatingTaxesPaid { get; set; }


        public decimal EBITDA
        {
            get { return this.OperatingIncome + this.Amortization; }
        }

        /// <summary>
        /// Free Cash Flow
        /// </summary>
        public decimal FCF
        {
            get { return this.EBITDA - this.FlowOperatingTaxesPaid - this.Capex + this.ChangesInAssets; }
        }

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

        /// <summary>
        /// P/E Ratio 
        /// </summary>
        public decimal PriceEarningsRatio
        {
            get { return Math.Round(Price / EarningsPerShare, 2); }
        }

        public decimal ReturnOnEquity
        {
            get { return Math.Round(this.NetIncome / this.Equity * 100, 2); }
        }


        public virtual Emitent Emitent { get; set; }
    }
}
