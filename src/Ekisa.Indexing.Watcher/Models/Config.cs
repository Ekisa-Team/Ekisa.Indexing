using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ekisa.Indexing.Watcher.Models
{
    public class Config
    {
        [JsonProperty("folder")]
        public string? Folder { get; set; }
        
        [JsonProperty("path_suffix")]
        public string[]? PathSuffix { get; set; }   
        
        [JsonProperty("@trigger_events")]
        public IEnumerable<ConfigTriggerEvent>? TriggerEvents { get; set; }
    }

    public class ConfigTriggerEvent
    {
        [JsonProperty("kind")]
        public string? Kind { get; set; }
        
        [JsonProperty("webhook_url")]
        public string? WebhookUrl { get; set; }

        [JsonProperty("webhook_http_method")]
        public string? WebhookHttpMethod { get; set; }

        [JsonProperty("webhook_request_headers")]
        public JObject? WebhookRequestHeaders { get; set; }

        [JsonProperty("webhook_request_body")]
        public JObject? WebhookRequestBody { get; set; }
    }
}
