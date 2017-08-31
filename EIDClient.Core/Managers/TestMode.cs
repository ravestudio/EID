using EIDClient.Core.Entities;
using EIDClient.Core.Repository;
using EID.Library;
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
        private SettingsRepository _settingsRepo = null;

        private string ServerURL = "http://localhost:99/";

        public TestMode(WebApiClient client, SettingsRepository repo)
        {
            this._apiClient = client;
            this._settingsRepo = repo;
            this.actions = new Dictionary<string, Action>();
        }

        public DateTime GetDate()
        {
            //this.datetime = this.datetime.AddMinutes(1);

            var list = _settingsRepo.GetAll().Result;
            Settings settings = list.First();

            return settings.TestDateTime;
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

                for (int i = 0; i < 550; i++)
                {
                    actions["update"].Invoke();

                    actions["sendToRobo"].Invoke();

                    actions["showData"].Invoke();

                    string res = _apiClient.GetData(string.Format("{0}{1}", this.ServerURL, "admin/EmulateData")).Result;

                    string create_stop_Result = _apiClient.GetData(string.Format("{0}{1}", this.ServerURL, "admin/CreateStopOrders")).Result;
                }
            });
        }
    }
}
