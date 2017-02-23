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
        public decimal GrossProfit
        {
            get { return this.Revenue - this.Expenses; }
        }

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
        public decimal TotalAssets
        {
            get { return this.CurrentAssets + this.FixedAssets; }
        }

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
        public decimal LiabilitiesAndEquity {
            get { return this.Equity + this.CurrentLiabilities + this.LongTermLiabilities; }
        }

        public virtual Emitent Emitent { get; set; }
    }
}
