using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Order : Entity<int>
    {
        IDictionary<string, OrderStateType> state_dic = null;
        public Order()
        {
            state_dic = new Dictionary<string, OrderStateType>();

            state_dic.Add("Активна", OrderStateType.IsActive);
            state_dic.Add("Исполнена", OrderStateType.Executed);
            state_dic.Add("Снята", OrderStateType.Canceled);
        }

        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public decimal Number { get; set; }
        public string Code { get; set; }
        public string Time { get; set; }
        public string Operation { get; set; }
        public string Account { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Volume { get; set; }
        public string State { get; set; }
        public string Class { get; set; }
        public string Client { get; set; }
        public string Comment { get; set; }

        public OrderStateType StateType
        {
            get
            {
                return state_dic[this.State];
            }

            set
            {
                this.State = state_dic.Single(p => p.Value == value).Key;
            }
        }
    }
}
