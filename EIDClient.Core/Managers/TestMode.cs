﻿using EIDClient.Library;
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
        private WebApiClient _apiClient = null;

        private string ServerURL = "http://localhost:99/";

        public TestMode(DateTime dateTime, WebApiClient client)
        {
            this.datetime = dateTime;
            this._apiClient = client;
            this.actions = new Dictionary<string, Action>();
        }

        public DateTime GetDate()
        {
            //this.datetime = this.datetime.AddMinutes(1);

            return this.datetime;
        }

        public void SetAction(string name, Action action)
        {
            actions.Add(name, action);
        }

        public void Start()
        {
            actions["init"].Invoke();

            for (int i = 0; i < 100; i++)
            {
                actions["update"].Invoke();

                actions["sendToRobo"].Invoke();

                string res = _apiClient.GetData(string.Format("{0}{1}", this.ServerURL, "admin/processData")).Result;
            }
        }
    }
}
