using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        private const string shirtsApiName = "ShirtsApi";
        private const string authApiName = "AuthorityApi";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebApiExecuter(IHttpClientFactory httpClientFactory, 
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor)
        {
            this._httpClientFactory = httpClientFactory;
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(shirtsApiName);
            await AddJwtTokenToHeader(httpClient);
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);
            await HandlePotentialError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T data)
        {
            var httpClient = _httpClientFactory.CreateClient(shirtsApiName);
            await AddJwtTokenToHeader(httpClient);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, data);
            
            await HandlePotentialError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T data)
        {
            var httpClient = _httpClientFactory.CreateClient(shirtsApiName);
            await AddJwtTokenToHeader(httpClient);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, data);
            await HandlePotentialError(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(shirtsApiName);
            await AddJwtTokenToHeader(httpClient);
            var response = await httpClient.DeleteAsync(relativeUrl);
            await HandlePotentialError(response);
        }

        public async Task HandlePotentialError(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorJson = await httpResponse.Content.ReadAsStringAsync();
                throw new WebApiException(errorJson);
            }
        }

        private async Task AddJwtTokenToHeader(HttpClient httpClient)
        {
            JwToken? token = null;
            string? strToken = _httpContextAccessor.HttpContext?.Session.GetString("access_token");

            if(!string.IsNullOrWhiteSpace(strToken))
            {
                token = JsonConvert.DeserializeObject<JwToken>(strToken);
            }

            // Ceck if the token string is null or token is expired, if so, we need to get a new one
            if (token == null || 
                token.ExpiresAt <= DateTime.UtcNow)
            {
                var clientId = _configuration.GetValue<string>("ClientId");
                var secret = _configuration.GetValue<string>("Secret"); // ?? string.Empty

                //Authenticate
                var authClient = _httpClientFactory.CreateClient(authApiName);
                var response = await authClient.PostAsJsonAsync("auth", new AppCredential
                {
                    ClientId = clientId,
                    Secret = secret
                });
                response.EnsureSuccessStatusCode();

                //Get the JWT
                strToken = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<JwToken>(strToken);

                _httpContextAccessor.HttpContext?.Session.SetString("access_token", strToken);
            }

            //Pass the JWT to the endpoints through the http headers
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }
    }
}
