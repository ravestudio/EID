﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIDClient.Core.Entities;
using EID.Library;

namespace EIDClient.Core.Repository
{
    public class EmitentRepository : Repository<Entities.Emitent, int>
    {
        public EmitentRepository(WebApiClient apiClient)
            : base(apiClient)
        {
            this.apiPath = "api/emitent/";

        }

        public override Task<IEnumerable<Emitent>> GetAll()
        {
            TaskCompletionSource<IEnumerable<Entities.Emitent>> TCS = new TaskCompletionSource<IEnumerable<Entities.Emitent>>();

            IList<Entities.Emitent> emitentList = new List<Entities.Emitent>();

            string url = string.Format("{0}{1}", this.ServerURL, "api/emitent/");

            this._apiClient.GetData(url).ContinueWith(t =>
            {
                string data = t.Result;

                var emitentArray = Windows.Data.Json.JsonValue.Parse(data).GetArray();

                for (int i = 0; i < emitentArray.Count; i++)
                {
                    var emitentValue = emitentArray[i];
                    Entities.Emitent emitent = this.GetObject(emitentValue);
                    emitentList.Add(emitent);
                }


                TCS.SetResult(emitentList);
            });

            return TCS.Task;
        }

        public override Task<string> Update(Emitent model)
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>();

            string url = string.Format("{0}{1}", this.ServerURL, "api/emitent/");

            this._apiClient.PostData(url, model.ConverToKeyValue()).ContinueWith(t =>
            {
                //string data = t.Result;

                TCS.SetResult(t.Result ? "success" : "fail");

            });

            return TCS.Task;
        }
    }
}
