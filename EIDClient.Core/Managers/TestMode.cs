using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Managers
{
    public class TestMode : ITradeMode
    {
        private DateTime datetime;
        private IDictionary<string, Action> actions = null;

        public TestMode(DateTime date)
        {
            this.datetime = date;
            this.actions = new Dictionary<string, Action>();
        }

        public DateTime GetDate()
        {
            this.datetime = this.datetime.AddMinutes(1);

            return this.datetime;
        }

        public void SetAction(string name, Action action)
        {
            actions.Add(name, action);
        }

        public void Start()
        {
            while(true)
            {

            }
        }
    }
}
