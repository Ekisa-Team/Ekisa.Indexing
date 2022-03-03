using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Ekisa.Indexing.Watcher.Services
{
    public class HttpService
    {
        #region Private Attributes
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructor
        public HttpService()
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>()!;

        }
        #endregion

        #region Public Methods
        public async Task<string?> PerformGetRequest(string webhookUrl, JObject? webhookHeaders)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient();

                if (webhookHeaders != null)
                {
                    foreach (KeyValuePair<string, JToken?> header in webhookHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value?.ToString());
                    }
                }

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(new Uri(webhookUrl));
                httpResponseMessage.EnsureSuccessStatusCode();
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<string?> PerformPostRequest(string webhookUrl, JObject? webhookHeaders, JObject? webhookBody)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient();

                if (webhookHeaders != null)
                {
                    foreach (KeyValuePair<string, JToken?> header in webhookHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value?.ToString());
                    }
                }

                StringContent stringContent = new(JsonConvert.SerializeObject(webhookBody), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(new Uri(webhookUrl), stringContent);
                httpResponseMessage.EnsureSuccessStatusCode();
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<string?> PerformPutRequest(string webhookUrl, JObject? webhookHeaders, JObject? webhookBody)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient();

                if (webhookHeaders != null)
                {
                    foreach (KeyValuePair<string, JToken?> header in webhookHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value?.ToString());
                    }
                }

                StringContent stringContent = new(JsonConvert.SerializeObject(webhookBody), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(new Uri(webhookUrl), stringContent);
                httpResponseMessage.EnsureSuccessStatusCode();
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<string?> PerformPatchRequest(string webhookUrl, JObject? webhookHeaders, JObject? webhookBody)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient();

                if (webhookHeaders != null)
                {
                    foreach (KeyValuePair<string, JToken?> header in webhookHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value?.ToString());
                    }
                }

                StringContent stringContent = new(JsonConvert.SerializeObject(webhookBody), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await httpClient.PatchAsync(new Uri(webhookUrl), stringContent);
                httpResponseMessage.EnsureSuccessStatusCode();
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<string?> PerformDeleteRequest(string webhookUrl, JObject? webhookHeaders)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient();

                if (webhookHeaders != null)
                {
                    foreach (KeyValuePair<string, JToken?> header in webhookHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value?.ToString());
                    }
                }

                HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(new Uri(webhookUrl));
                httpResponseMessage.EnsureSuccessStatusCode();
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
        #endregion
    }
}
