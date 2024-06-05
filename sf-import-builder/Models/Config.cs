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

    public string SourceFolderPath { get; set; }
    public string OutputFolderPath { get; set; }
    public bool CreateZipPackage { get; set; }
    public bool DeleteOutputFolder { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public Config(string SourceFolderPath, string? OutputFolderPath = null, bool CreateZipPackage = false, bool DeleteOutputFolder = false) {
        
        this.SourceFolderPath = SourceFolderPath;
        this.OutputFolderPath = OutputFolderPath ?? SourceFolderPath;
        this.CreateZipPackage = CreateZipPackage;
        this.DeleteOutputFolder = DeleteOutputFolder;

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

}

[JsonSerializable(typeof(Config))]
internal partial class ConfigContext : JsonSerializerContext {
}
