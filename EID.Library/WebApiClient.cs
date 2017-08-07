using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EID.Library
{
    public class WebApiClient
    {
        public Task<string> GetData(string url)
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>();

            var uri = new Uri(url);
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();


            Task<System.Net.Http.HttpResponseMessage> response = httpClient.GetAsync(uri);

            response.ContinueWith(r =>
            {
                string msg = r.Result.Content.ReadAsStringAsync().Result;
                TCS.SetResult(msg);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            return TCS.Task;
        }

        public Task<bool> PostData(string url, List<KeyValuePair<string, string>> data)
        {
            TaskCompletionSource<bool> TCS = new TaskCompletionSource<bool>();

            var uri = new Uri(url);
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpContent content = new System.Net.Http.FormUrlEncodedContent(data);

            string text = string.Empty;


            Task<System.Net.Http.HttpResponseMessage> response = httpClient.PostAsync(uri, content);

            response.ContinueWith(r =>
            {
                TCS.SetResult(r.Result.IsSuccessStatusCode);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            return TCS.Task;
        }

        public Task<bool> PutData(string url, List<KeyValuePair<string, string>> data)
        {
            TaskCompletionSource<bool> TCS = new TaskCompletionSource<bool>();

            var uri = new Uri(url);
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpContent content = new System.Net.Http.FormUrlEncodedContent(data);

            string text = string.Empty;


            Task<System.Net.Http.HttpResponseMessage> response = httpClient.PutAsync(uri, content);

            response.ContinueWith(r =>
            {
                TCS.SetResult(r.Result.IsSuccessStatusCode);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            return TCS.Task;
        }
    }
}
