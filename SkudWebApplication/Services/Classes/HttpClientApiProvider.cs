using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json;
using SkudWebApplication.Extensions;
using SkudWebApplication.Services.Interfaces;
using System.Text;

namespace SkudWebApplication.Services.Classes
{
    public class HttpClientApiProvider : IApiProvider
    {
        private readonly string _apiDomain;
        public HttpClientApiProvider(IApiDomainsManager apiDomainsManager) 
        {
            var directory = System.IO.Directory.GetCurrentDirectory();
            //if (directory.StartsWith("D:\\Repos\\"))
            if (directory.StartsWith("C:\\Users\\"))
            {
                _apiDomain = apiDomainsManager.GetDomain("own-local");
            }
            if (directory.StartsWith("D:\\iis\\"))
            {
                _apiDomain = apiDomainsManager.GetDomain("own-iis-local");
            }
            if (directory.StartsWith("C:\\skud\\"))
            {
                _apiDomain = apiDomainsManager.GetDomain("own-prod");
            }
        }
        public async Task<TResponse> SendGetRequestAsync<TRequest, TResponse>(string apiMethod, TRequest request) where TResponse : class, new()
        {
            var uri = $"{_apiDomain}/{apiMethod}";

            if(typeof(TRequest) == typeof(Requests.Auth.LoginRequest))
            {
                var loginRequest = request as SkudWebApplication.Requests.Auth.LoginRequest;
                if(loginRequest!=null)
                    uri = $"{uri}?Login={loginRequest.Login}&Password={loginRequest.Password}";
            }

            using HttpClient client = new();
            using HttpResponseMessage response = await client.GetAsync(uri);
            await response.EnsureAndThrow();
            return await response.Content.ReadFromJsonAsync<TResponse>() ?? new TResponse();
        }
        public async Task SendAddRequestAsync<TRequest>(string apiMethod, TRequest request) where TRequest : class 
        {
            var uri = $"{_apiDomain}/{apiMethod}";
            using HttpClient client = new();
            using HttpResponseMessage response = await client.PostAsJsonAsync(uri, request);
            await response.EnsureAndThrow();
        }
        public async Task SendEditRequestAsync<TRequest>(string apiMethod, TRequest request) where TRequest : class
        {
            var uri = $"{_apiDomain}/{apiMethod}";
            using HttpClient client = new();
            using HttpResponseMessage response = await client.PutAsJsonAsync(uri, request);
            await response.EnsureAndThrow();
        }

        public async Task SendDeleteRequestAsync<TRequest>(string apiMethod, TRequest request) where TRequest : class
        {
            var uri = $"{_apiDomain}/{apiMethod}";
            using HttpClient client = new();
            using HttpResponseMessage response = await client.DeleteAsJsonAsync(uri, request);
            await response.EnsureAndThrow();
        }
    }

    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data, CancellationToken cancellationToken)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data, CancellationToken cancellationToken)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

        private static HttpContent Serialize(object data) => new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    }
}
