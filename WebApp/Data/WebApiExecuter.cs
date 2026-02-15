namespace WebApp.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        private const string apiName = "ShirtsApi";
        private readonly IHttpClientFactory httpClientFactory;

        public WebApiExecuter(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            return await httpClient.GetFromJsonAsync<T>(relativeUrl);
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T data)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, data);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
