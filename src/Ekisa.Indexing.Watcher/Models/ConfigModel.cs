using Newtonsoft.Json.Linq;

namespace Ekisa.Indexing.Watcher.Models
{
    public class ConfigModel
    {
        public string? IndexingFolder { get; set; }
        public string? WebhookUrl { get; set; }
        public string? WebhookHttpMethod { get; set; }
        public JObject? WebhookRequestHeaders { get; set; } 
        public JObject? WebhookRequestBody { get; set; } 
    }
}
