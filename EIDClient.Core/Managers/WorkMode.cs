using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Managers
{
    public class WorkMode : ITradeMode
    {
        private IDictionary<string, Action> actions = null;

        public WorkMode()
        {
            this.actions = new Dictionary<string, Action>();
        }

        public DateTime GetDate()
        {
            return DateTime.Now;
        }

        public void SetAction(string name, Action action)
        {
            actions.Add(name, action);
        }

        public void Start()
        {
            //Task t = new Task(() =>
            //{
            //    while (true)
            //    {
            //        Task.Delay(5000).Wait();
            //        actions["robot"].Invoke();
            //    }
            //});

            Task t2 = new Task(() =>
            {
                while (true)
                {
                    Task.Delay(1000).Wait();
                    actions["updater"].Invoke();
                }
            });

            t2.Start();
        }
    }
}
