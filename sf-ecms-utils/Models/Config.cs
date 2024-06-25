using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SF_ECMS_Utils.Models;
public class Config {

    /***********************************************************************************************************/
    /********************************************** PROPERTIES *************************************************/
    /***********************************************************************************************************/

    public string Action { get; set; }
    public string SourceFiles_FolderPath { get; set; }
    public string? PackagedFiles_ZipFilePath { get; set; }
    public string PackagedFiles_FolderPath { get; set; }
    public bool CreateZipFiles { get; set; }
    public int ZipFileSplitLevel { get; set; }

    /***********************************************************************************************************/
    /*********************************************** CONSTRUCTOR ***********************************************/
    /***********************************************************************************************************/

    public Config(string Action, string? SourceFiles_FolderPath = null, string? PackagedFiles_ZipFilePath = null, string? PackagedFiles_FolderPath = null, bool CreateZipFiles = false, int ZipFileSplitLevel = 0) {

        this.Action = Action == "PackageFiles" ? "PackageFiles" : "AnalyzeFiles";
        this.SourceFiles_FolderPath = SourceFiles_FolderPath ?? "";
        this.PackagedFiles_ZipFilePath = PackagedFiles_ZipFilePath;
        this.PackagedFiles_FolderPath = PackagedFiles_FolderPath ?? "";
        this.CreateZipFiles = CreateZipFiles;
        this.ZipFileSplitLevel = ZipFileSplitLevel;

        // VALIDATE THE CONFIG FILE

        if (this.Action == "AnalyzeFiles") {

            // IF WE HAVE A VALID ZIP FILE

            if (PackagedFiles_ZipFilePath != null && File.Exists(PackagedFiles_ZipFilePath)) {

                this.PackagedFiles_ZipFilePath = PackagedFiles_ZipFilePath;
                this.PackagedFiles_FolderPath = Path.GetDirectoryName(this.PackagedFiles_ZipFilePath) ?? "";

            } else if (PackagedFiles_FolderPath != null && Directory.Exists(PackagedFiles_FolderPath)) {

                this.PackagedFiles_FolderPath = PackagedFiles_FolderPath;

            } else {

                throw new Exception("Error, when taking an action of 'AnalyzeFiles', the config needs to contain either a valid 'PackagedFiles_ZipFilePath' or 'PackagedFiles_FolderPath'.");

            }

        } else {

            if (SourceFiles_FolderPath == null || PackagedFiles_FolderPath == null) {

                throw new Exception("Error, when taking an action of 'PackageFiles', the config needs to contain both 'SourceFiles_FolderPath' and 'PackagedFiles_FolderPath'.");

            } else {

                if (!Directory.Exists(this.SourceFiles_FolderPath)) {

                    throw new Exception("Error, the 'SourceFiles_FolderPath' file path is invalid.");

                }

                if (!Directory.Exists(this.PackagedFiles_FolderPath)) {

                    Directory.CreateDirectory(this.SourceFiles_FolderPath);

                }

            }

        }

    }

    /***********************************************************************************************************/
    /************************************************* METHODS *************************************************/
    /***********************************************************************************************************/

}

[JsonSerializable(typeof(Config))]
internal partial class ConfigContext : JsonSerializerContext {
}
