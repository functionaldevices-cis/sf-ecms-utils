using SF_ECMS_Utils.Models;
using System.Diagnostics;
using System.Text.Json;

namespace SF_ECMS_Utils;

internal class Program {

    static void Main(string[] args) {

        // ATTEMPT TO LOAD DATA

        string configFilePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? "", "sf-ecms-utils-config.json");
        string configFileContent = File.ReadAllText(configFilePath);

        // PARSE DATA

        Config config = JsonSerializer.Deserialize(configFileContent, ConfigContext.Default.Config) ?? throw new Exception("Error, unable to parse the config file. Please check that the file exists and has correctly formatted JSON data.");

        // RUN IMPORT BUILDER

        new SFECMSUtils(config);

    }

}

