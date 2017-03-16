using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Repository
{
    public class Repository<G, Key>
            where G : Entities.Entity<Key>, new()
    {

        protected EIDClient.Core.WebApiClient _apiClient = null;

        protected string ServerURL = "http://localhost:99/";
        //protected string ServerURL = "http://localhost:61943/";
        //protected string ServerURL = "http://ravestudio-001-site1.htempurl.com/";

        protected string apiPath { get; set; }

        public Repository(EIDClient.Core.WebApiClient apiClient)
        {
            this._apiClient = apiClient;
        }

        public virtual async Task<G> GetById(Key id)
        {
            return null;
        }

        public virtual Task<IEnumerable<G>> GetAll()
        {
            TaskCompletionSource<IEnumerable<G>> TCS = new TaskCompletionSource<IEnumerable<G>>();

            IList<G> entityList = new List<G>();

            string url = string.Format("{0}{1}", this.ServerURL, this.apiPath);

            this._apiClient.GetData(url).ContinueWith(t =>
            {
                string data = t.Result;

                var emitentArray = Windows.Data.Json.JsonValue.Parse(data).GetArray();

                for (int i = 0; i < emitentArray.Count; i++)
                {
                    var value = emitentArray[i];
                    G entity = this.GetObject(value);
                    entityList.Add(entity);
                }


                TCS.SetResult(entityList);
            });

            return TCS.Task;
        }

        public virtual void Create(G model)
        {

        }

        public virtual void Update(G model)
        {

        }

        public G GetObject(Windows.Data.Json.IJsonValue jsonValue)
        {
            G obj = null;

            var jsonObj = jsonValue.GetObject();

            obj = new G();
            obj.ReadData(jsonObj);

            return obj;
        }

        public int GetErrorInfo(Windows.Data.Json.IJsonValue jsonValue)
        {
            int error = 0;

            var jsonObj = jsonValue.GetObject();

            error = (int)jsonObj["error"].GetNumber();

            return error;
        }
    }
}
