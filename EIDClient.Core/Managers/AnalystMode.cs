using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Managers
{
    public class AnalystMode : ITradeMode
    {
        private IDictionary<string, Action> actions = null;

        public AnalystMode()
        {
            this.actions = new Dictionary<string, Action>();
        }

        public DateTime GetDate()
        {
            return DateTime.Now.AddHours(3);
            //return new DateTime(2017, 7, 5, 10, 0, 0);
        }

        public void SetAction(string name, Action action)
        {
            actions.Add(name, action);
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    actions["init"].Invoke();
                    actions["sendToRobo"].Invoke();

                    Task.Delay(15000).Wait();
                }
            });
        }
    }
}
