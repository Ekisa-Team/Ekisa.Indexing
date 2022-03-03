using Ekisa.Indexing.Watcher.Constants;
using Ekisa.Indexing.Watcher.Core;
using Ekisa.Indexing.Watcher.Models;
using Ekisa.Indexing.Watcher.Services;

try
{
    ConfigService configService = new();
    Config? config = await configService.ReadConfigFile(ConfigConstants.CONFIG_FILE_LOCATION);
    OrchestratorService orchestrator = new(config!);
    orchestrator.Start();
    Console.ReadLine();
}
catch (Exception ex)
{
    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    Environment.Exit(1);
}

