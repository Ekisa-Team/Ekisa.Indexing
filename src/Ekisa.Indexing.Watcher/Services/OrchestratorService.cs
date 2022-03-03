using Ekisa.Indexing.Watcher.Enums;
using Ekisa.Indexing.Watcher.Models;
using Ekisa.Indexing.Watcher.Services;
using Ekisa.Indexing.Watcher.Utils;

namespace Ekisa.Indexing.Watcher.Core
{
    public class OrchestratorService
    {
        private readonly Config _config;
        private readonly ConfigService _configService;
        private readonly HttpService _httpService;

        public OrchestratorService(Config config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _configService = new ConfigService();
            _httpService = new HttpService();
        }

        public void Start()
        {
            _configService.Locations.TryGetValue(_config.Folder, out string? folderFullPath);
            string listeningPath = folderFullPath!;

            if (_config.PathSuffix != null)
            {
                listeningPath = Path.Combine(listeningPath, string.Join(@"\", _config.PathSuffix.Select(p => p)));
            }

            FileWatcherService fw = new(listeningPath);
            fw.DirectoryChanged += HandleDirectoryChanged;
            fw.Watch();
            Console.WriteLine($"Listening for file changes on {listeningPath}");
        }

        private void HandleDirectoryChanged(string fullPath, TriggerEventKind triggerEventKind)
        {
            ConfigTriggerEvent? @event = _config.TriggerEvents.Where(x => x.Kind == triggerEventKind.ToString()).FirstOrDefault();

            if (@event == null)
            {
                throw new Exception("Trigger event wasn't found");
            }

            _ = Task.Run(async () =>
                {
                    string? response = @event.WebhookHttpMethod switch
                    {
                        nameof(WebhookHttpMethod.Get) => await _httpService.PerformGetRequest(@event.WebhookUrl, @event.WebhookRequestHeaders),
                        nameof(WebhookHttpMethod.Post) => await _httpService.PerformPostRequest(@event.WebhookUrl, @event.WebhookRequestHeaders, @event.WebhookRequestBody),
                        nameof(WebhookHttpMethod.Put) => await _httpService.PerformPutRequest(@event.WebhookUrl, @event.WebhookRequestHeaders, @event.WebhookRequestBody),
                        nameof(WebhookHttpMethod.Patch) => await _httpService.PerformPatchRequest(@event.WebhookUrl, @event.WebhookRequestHeaders, @event.WebhookRequestBody),
                        nameof(WebhookHttpMethod.Delete) => await _httpService.PerformDeleteRequest(@event.WebhookUrl, @event.WebhookRequestHeaders),
                        _ => throw new NotImplementedException()
                    };

                    Console.WriteLine(response);
                });
        }
    }
}
