using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class StopOrder : Entity<int>
    {
        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }
        /// <summary>
        /// Номер
        /// </summary>
        public decimal Number { get; set; }

        /// <summary>
        /// Заявка условя
        /// </summary>
        //public decimal BaseOrder { get; set; }

        /// <summary>
        /// Номер заявки зарегистрированной по наступлению условия стоп-цены
        /// </summary>
        public decimal OrderNumber { get; set; }

        public string Code { get; set; }
        public string Time { get; set; }
        public string Operation { get; set; }
        public string Account { get; set; }

        public string OrderType { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal StopPrice { get; set; }
        public decimal StopLimitPrice { get; set; }

        public string Client { get; set; }
        public string Class { get; set; }
        public string State { get; set; }
        public string Result { get; set; }

        public OrderStateEnum OrderState { get; set; }
        public OrderOperationEnum OrderOperation { get; set; }
    }
}
