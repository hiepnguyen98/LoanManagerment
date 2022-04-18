using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace Client.Services.ApiServices
{
    public abstract class ApiServiceBase
    {
        private readonly HttpClient _httpClient;

        public ApiServiceBase(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var res = await _httpClient.GetAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<T>();
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<U> PostAsync<U,T>(string url,T data)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync(url, data);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<U>();
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task PostAsync<T>(string url,T data)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync(url, data);
                if (res.IsSuccessStatusCode)
                {
                    return;
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> PostAsync<T>(string url)
        {
            try
            {
                var res = await _httpClient.PostAsync(url, null);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<T>();
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
