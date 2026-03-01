using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        private const string shirtsApiName = "ShirtsApi";
        private const string authApiName = "AuthorityApi";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public WebApiExecuter(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this._httpClientFactory = httpClientFactory;
            this._configuration = configuration;
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
            //await AddJwtTokenToHeader(httpClient);
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
            var clientId = _configuration.GetValue<string>("ClientId");
            var secret = _configuration.GetValue<string>("Secret");

            //Authenticate
            var authClient = _httpClientFactory.CreateClient(authApiName);
            var response = await authClient.PostAsJsonAsync("auth", new AppCredential
            {
                ClientId = clientId,
                Secret = secret
            });
            response.EnsureSuccessStatusCode();

            //Get the JWT
            string strToken = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<JwToken>(strToken);

            //Pass the JWT to the endpoints through the http headers
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }
    }
}
