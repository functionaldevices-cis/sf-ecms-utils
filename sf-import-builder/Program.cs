using SF_Import_Builder.Models;
using System.Diagnostics;
using System.Text.Json;

namespace SF_Import_Builder;

internal class Program {

    static void Main(string[] args) {

        // ATTEMPT TO LOAD DATA

        string configFilePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? "", "sf-import-builder-config.json");
        string configFileContent = File.ReadAllText(configFilePath);

        // PARSE DATA

        Config config = JsonSerializer.Deserialize<Config>(configFileContent) ?? throw new Exception("Error, unable to parse the config file. Please check that the file exists and has correctly formatted JSON data.");

        // RUN IMPORT BUILDER

        new SFImportBuilder(config);

    }

}

