﻿using System.Net.Http.Json;
using System.Text.Json;

namespace Client.BuildingBlocks.Http
{
    public class HttpClientService
    {
        private HttpClient httpClient;
        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient(HttpClientConstants.DefaultHttpClient);
            httpClient.BaseAddress = new Uri(httpClient.BaseAddress.ToString());
        }
        public async Task<T> GetFromAPIAsync<T>(string route)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("/api" + route);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(await httpResponseMessage.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch (Exception ex)
                {

                }
            }
            if (httpResponseMessage.IsSuccessStatusCode is false)
            {
            }
            return default;
        }

        public async Task<T> PostToAPIAsync<T>(string route, T t)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("/api" + route, t, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(await httpResponseMessage.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch (Exception ex)
                {

                }
            }
            if (httpResponseMessage.IsSuccessStatusCode is false)
            {
            }
            return default;
        }

        public async Task DeleteFromAPIAsync(string route, Guid id)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync("/api" + route + "/" + id);
            if (httpResponseMessage.IsSuccessStatusCode)
            {

            }
            if (httpResponseMessage.IsSuccessStatusCode is false)
            {
            }
        }

        public void AddDefaultHeader(string name, string value)
        {
            httpClient.DefaultRequestHeaders.Add(name, value);
        }
    }
}
