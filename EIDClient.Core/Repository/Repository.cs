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
            return null;
        }
        public virtual void Create(G model)
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
