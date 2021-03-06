﻿using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Managers
{
    public class DemoMode : ITradeMode
    {
        private DateTime datetime;
        private IDictionary<string, Action> actions = null;
        private WebApiClient _apiClient = null;

        private string ServerURL = "http://localhost:99/";

        public DemoMode(WebApiClient client)
        {
            this._apiClient = client;
            this.actions = new Dictionary<string, Action>();
        }

        public DateTime GetDate()
        {
            return DateTime.Now.AddHours(-3);
        }

        public void SetAction(string name, Action action)
        {
            actions.Add(name, action);
        }

        public void Start()
        {
            Task.Run(() =>
            {
                actions["initEmpty"].Invoke(); 

                while (true)
                {
                    actions["update"].Invoke();
                    actions["sendToRobo"].Invoke();

                    Task.Delay(5 * 1000).Wait();

                    string read_Result = _apiClient.GetData(string.Format("{0}{1}", this.ServerURL, "admin/ReadTransactionResult")).Result;

                    string create_stop_Result = _apiClient.GetData(string.Format("{0}{1}", this.ServerURL, "admin/CreateStopOrders")).Result;

                    Task.Delay(5 * 1000).Wait();
                }
            });
        }
    }
}
