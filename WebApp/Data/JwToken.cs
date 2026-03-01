using Newtonsoft.Json;

namespace WebApp.Data
{
    public class JwToken
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}
