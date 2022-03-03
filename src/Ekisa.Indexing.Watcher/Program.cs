using Ekisa.Indexing.Watcher.Constants;
using Ekisa.Indexing.Watcher.Core;
using Ekisa.Indexing.Watcher.Models;
using Ekisa.Indexing.Watcher.Services;

ConfigService configService = new(ConfigConstants.CONFIG_FILE_LOCATION);
ConfigModel? configModel = await configService.ReadConfigFile();

OrchestratorService orchestrator = new();
orchestrator.Start();

Console.ReadKey();
