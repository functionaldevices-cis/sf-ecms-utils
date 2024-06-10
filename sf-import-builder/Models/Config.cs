using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SF_Import_Builder.Models;
public class Config {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string Action { get; set; }
    public string SourceFolderPath { get; set; }
    public string PackagedFolderPath { get; set; }
    public bool CreateZipFile { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public Config(string Action, string SourceFolderPath, string PackagedFolderPath, bool CreateZipFile = false) {
        
        this.Action = Action == "PackageFiles" ? "PackageFiles" : "AnalyzeFiles";
        this.SourceFolderPath = SourceFolderPath;
        this.PackagedFolderPath = PackagedFolderPath;
        this.CreateZipFile = CreateZipFile;

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

}

[JsonSerializable(typeof(Config))]
internal partial class ConfigContext : JsonSerializerContext {
}
