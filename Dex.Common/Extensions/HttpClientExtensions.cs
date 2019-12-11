using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dex.Common.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Content = new StringContent(dataAsString, Encoding.UTF8, "application/json"),
                RequestUri = new Uri(httpClient.BaseAddress + url)
            };

            return httpClient.SendAsync(request);
        }

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                Content = new StringContent(dataAsString, Encoding.UTF8, "application/json"),
                RequestUri = new Uri(httpClient.BaseAddress + url)
            };

            return httpClient.SendAsync(request);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(dataAsString);
        }
    }
}