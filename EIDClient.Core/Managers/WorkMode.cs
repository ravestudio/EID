﻿using System;
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
            return DateTime.Now.AddHours(-3);

            //return new DateTime(2017, 8, 15, 10, 03, 0);
        }

        public void SetAction(string name, Action action)
        {
            actions.Add(name, action);
        }


        public void Start()
        {

            Task.Run(() =>
            {
                actions["init"].Invoke();

                while (true)
                {
                    actions["update"].Invoke();
                    actions["sendToRobo"].Invoke();

                    Task.Delay(5 * 1000).Wait();
                }
            });
        }
    }
}
