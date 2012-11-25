using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hue
{
    /// <summary>
    /// Send Async REST Json Requests
    /// </summary>
    internal static class HttpRestHelper
    {
        public static async Task<string> Post(string url, string body)
        {
            var result = await (new HttpClient()).PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"))
                                  .ContinueWith(response => response.Result.Content.ReadAsStringAsync());
            string responseFromServer = result.Result;
            return responseFromServer;
        }

        public static async Task<string> Put(string url, string body)
        {
            var result = await (new HttpClient()).PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            if (result.IsSuccessStatusCode)
            {
                var r = await result.Content.ReadAsStringAsync();
                if (r.Contains("error"))
                    System.Diagnostics.Trace.Write(r);
                return r;
            }
            return "";
        }
    }
}